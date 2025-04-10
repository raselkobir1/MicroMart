﻿using Auth.API.Domain.Dto.Common;
using Auth.WebAPI.Domain.Dto.Login;

namespace Auth.API.Manager.Interface
{
    public interface ILoginManager
    {
        Task<ResponseModel> Login(LoginRequestDto dto, string? userAgent, string? ipAddress);
        Task<ResponseModel> ValidateToken(string jwtToken);
        Task<ResponseModel> RefreshJwtToken(AccessTokenFromRefreshTokenDto dto);
        Task<ResponseModel> Logout(LogoutRequestDto dto);
    }
}
