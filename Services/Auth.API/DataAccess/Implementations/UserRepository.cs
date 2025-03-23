using Auth.API.DataAccess.DataContext;
using Auth.API.DataAccess.Interfaces;
using Auth.API.Domain.Dtos;
using Auth.API.Domain.Dtos.PaginatedResult;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.DataAccess.Implementations
{
    public class UserRepository : GenericRepository<Domain.Entities.User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, ReadDbContext readDbContext) : base(context, readDbContext)
        {
        }

        public async Task<PagingResponseDto> GetPasignatedResult(UserFilterDto dto)
        {
            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                query = query.Where(x => x.UserName.ToLower().Contains(dto.UserName.ToLower()));

            if (!string.IsNullOrWhiteSpace(dto.Email))
                query = query.Where(x => x.Email.ToLower().Contains(dto.Email.ToLower()));
             
            var result = await (from user in query
                                .OrderByDescending(x => x.Id)
                                .Skip(dto.Skip)
                                .Take(dto.PageSize)
                                select new UserDto
                                {
                                    Id = user.Id,
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    Role = user.Role.ToString(),
                                    Status = user.Status.ToString()
                                })
                                .ToListAsync();

            var totalRecords = await query.CountAsync();
            return new PagingResponseDto(result, totalRecords, dto.PageNumber, dto.PageSize);
        }
    }
}
