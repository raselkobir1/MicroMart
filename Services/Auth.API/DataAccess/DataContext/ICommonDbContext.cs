using Auth.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.DataAccess.DataContext
{
    public interface ICommonDbContext
    {
        DbSet<Domain.Entities.User> Users { get; set; }  
        DbSet<VerificationCode> VerificationCode { get; set; } 
        DbSet<UserToken> UserTokens { get; set; } 
        
    }
}
