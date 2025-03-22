using User.API.DataAccess.DataContext;
using User.API.DataAccess.Interfaces;
using User.API.Domain.Dtos;
using User.API.Domain.Dtos.PaginatedResult;
using Microsoft.EntityFrameworkCore;

namespace User.API.DataAccess.Implementations
{
    public class UserRepository : GenericRepository<Domain.Entities.User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, ReadDbContext readDbContext) : base(context, readDbContext)
        {
        }

        public async Task<PagingResponseDto> GetPasignatedResult(UserFilterDto dto)
        {
            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Name))
                query = query.Where(x => x.Name.ToLower().Contains(dto.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(dto.Email))
                query = query.Where(x => x.Email.ToLower().Contains(dto.Email.ToLower()));
             
            var result = await (from user in query
                                .OrderByDescending(x => x.Id)
                                .Skip(dto.Skip)
                                .Take(dto.PageSize)
                                select new UserDto
                                {
                                    Id = user.Id,
                                    Name = user.Name,
                                    Email = user.Email,
                                    Phone = user.Phone,
                                    Address = user.Address,
                                    Status = user.Status.ToString()
                                })
                                .ToListAsync();

            var totalRecords = await query.CountAsync();
            return new PagingResponseDto(result, totalRecords, dto.PageNumber, dto.PageSize);
        }
    }
}
