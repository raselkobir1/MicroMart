using Auth.API.DataAccess.DataContext;
using Auth.API.DataAccess.Interfaces;
using Auth.API.Domain.Entities;

namespace Auth.API.DataAccess.Implementations
{
    public class VerificationCodeRepository : GenericRepository<VerificationCode>, IVerificationCodeRepository
    {
        public VerificationCodeRepository(ApplicationDbContext context, ReadDbContext readDbContext) : base(context, readDbContext)
        {
        }
    }
}
