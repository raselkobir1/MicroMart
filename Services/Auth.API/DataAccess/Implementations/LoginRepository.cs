using Auth.API.DataAccess.DataContext;
using Auth.API.DataAccess.Interfaces;
using Auth.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.DataAccess.Implementations
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public LoginRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveUserToken(UserToken token)
        {
            _dbContext.Add(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserToken> GetUserToken(string refreshToken)
        {
            return await _dbContext.UserTokens.Where(x => x.RefreshToken == refreshToken).FirstOrDefaultAsync();
        }
        public async Task RevokeToken(string refreshToken)
        {
            await _dbContext.UserTokens.Where(x => x.RefreshToken == refreshToken).ExecuteUpdateAsync(x => x.SetProperty(y => y.IsRevoked, true));
        }
        public async Task RevokeAllTokensByUserId(long userId)
        {
            await _dbContext.UserTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsRevoked, true));
        }
        public async Task UpdateUserToken(UserToken token)
        {
            _dbContext.Update(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetDetailUserInfoByUserId(long id)
        {
            return await _dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task SaveLoginHistory(LoginHistory loginHistory)
        {
            _dbContext.Add(loginHistory);
            await _dbContext.SaveChangesAsync();
        }
    }
}
