using Auth.API.DataAccess.UnitOfWorks;
using Auth.API.Domain.Dto.Common;
using Auth.API.Domain.Entities;
using Auth.API.Helper;
using Auth.API.Helper.Configuration;
using Auth.API.Helper.Enums;
using Auth.API.Helper.Resources;
using Auth.API.Manager.Interface;
using Auth.WebAPI.Domain.Dto.Login;
using BCrypt.Net;
using Mapster;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.API.Manager.Implementation
{
    public class LoginManager: ILoginManager
    {
        private readonly JwtTokenConfiguration _jwtTokenConfiguration;
        private readonly IUnitOfWork _unitOfWork;

        public LoginManager(IUnitOfWork unitOfWork, IOptionsSnapshot<JwtTokenConfiguration> tokenAppsettingConfig)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenConfiguration = tokenAppsettingConfig.Value;
        }

        public async Task<ResponseModel> Login(LoginRequestDto dto)
        {
            #region User Validation
            if (string.IsNullOrEmpty(dto.Email))
                return Utilities.ValidationErrorResponse(CommonMessage.InvalidEmail);

            if (string.IsNullOrEmpty(dto.Password))
                return Utilities.ValidationErrorResponse(CommonMessage.InvalidPassword);

            var user = await _unitOfWork.Users.GetWhere(x => x.Email.ToLower().Trim() == dto.Email.ToLower().Trim());
            if (user == null)
                return Utilities.ValidationErrorResponse(CommonMessage.IncorrectUser);

            if (user.Status != AccountStatus.ACTIVE)
                return Utilities.ValidationErrorResponse(CommonMessage.InactiveUser);

            if (!user.Verified)
                return Utilities.ValidationErrorResponse(CommonMessage.NotVerifiedUser);

            var passwordVerificationResult = BCrypt.Net.BCrypt.EnhancedVerify(dto.Password, user.Password, HashType.SHA512);

            if (!passwordVerificationResult)
                return Utilities.ValidationErrorResponse(CommonMessage.IncorrectPassword);
            #endregion


            var (jwtToken, jwtExpiry) = GenerateJWTTokensAsync(user);
            var (refreshToken, refreshExpiry) = GenerateRefreshToken();

            var token = new UserToken
            {
                JWTToken = jwtToken,
                JWTExpires = jwtExpiry,
                RefreshToken = refreshToken,
                RefreshExpires = refreshExpiry,
                UserId = user.Id,
                IsRevoked = false
            };

            await _unitOfWork.Login.SaveUserToken(token);

            var authResponse = token.Adapt<AuthResponse>();
            return Utilities.SuccessResponse("Login successful", authResponse);
        }

        public async Task<ResponseModel> Logout(LogoutRequestDto dto)
        {
            var token = await _unitOfWork.Login.GetUserToken(dto.RefreshToken);

            if (token != null)
            {
                await _unitOfWork.Login.RevokeToken(dto.RefreshToken);
                return Utilities.SuccessResponse("Logout successful");
            }

            return Utilities.NotFoundResponse();
        }

        public async Task<ResponseModel> RefreshJwtToken(AccessTokenFromRefreshTokenDto dto)
        {
            var token = await _unitOfWork.Login.GetUserToken(dto.RefreshToken);

            if (token == null || token.IsRevoked || token.RefreshExpires <= CommonMethods.GetCurrentTime())
                return Utilities.ValidationErrorResponse(CommonMessage.InvalidToken);

            var user = await _unitOfWork.Users.GetById(token.UserId);

            var (jwtToken, jwtExpiry) = GenerateJWTTokensAsync(user);
            var (refreshToken, _) = GenerateRefreshToken();

            token.JWTToken = jwtToken;
            token.JWTExpires = jwtExpiry;
            token.RefreshToken = refreshToken;

            await _unitOfWork.Login.UpdateUserToken(token);

            var finalResponse = token.Adapt<AuthResponse>();
            return Utilities.SuccessResponse(null, finalResponse);
        }

        private (string, DateTime) GenerateJWTTokensAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenConfiguration.SigningKey);
            var expiryTime = CommonMethods.GetCurrentTime().AddMinutes(Convert.ToDouble(_jwtTokenConfiguration.JWTTokenExpirationMinutes));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("RoleId", Convert.ToString((int)user.Role)),
                    new Claim("UserName", user.UserName.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                ]),

                Expires = expiryTime,
                Issuer = _jwtTokenConfiguration.Issuer,
                Audience = _jwtTokenConfiguration.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };
            var accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            return (accessToken, (DateTime)tokenDescriptor.Expires);
        }

        private (string, DateTime) GenerateRefreshToken()
        {
            var expiryTime = CommonMethods.GetCurrentTime()
                .AddMinutes(Convert.ToDouble(_jwtTokenConfiguration.RefreshTokenExpirationMinutes));

            byte[] randomData = new byte[32];
            RandomNumberGenerator.Fill(randomData);

            byte[] timeData = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            byte[] combinedData = new byte[randomData.Length + timeData.Length];

            Buffer.BlockCopy(randomData, 0, combinedData, 0, randomData.Length);
            Buffer.BlockCopy(timeData, 0, combinedData, randomData.Length, timeData.Length);

            var refreshToken = Convert.ToBase64String(combinedData);
            return (refreshToken, expiryTime);
        }
    }
}
