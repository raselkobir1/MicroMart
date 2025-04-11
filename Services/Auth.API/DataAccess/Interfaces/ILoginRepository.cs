using Auth.API.Domain.Entities;

namespace Auth.API.DataAccess.Interfaces
{
    public interface ILoginRepository
    {
        Task SaveUserToken(UserToken token);
        Task SaveLoginHistory(LoginHistory loginHistory); 
        Task<UserToken> GetUserToken(string refreshToken);
        Task UpdateUserToken(UserToken token);
        Task RevokeToken(string refreshToken);
        Task RevokeAllTokensByUserId(long userId);
    }
}
