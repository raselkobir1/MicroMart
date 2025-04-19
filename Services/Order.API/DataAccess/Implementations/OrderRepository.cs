using Order.API.DataAccess.DataContext;
using Order.API.DataAccess.Interfaces;
using Order.API.Domain.Dtos;
using Order.API.Domain.Dtos.PaginatedResult;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Order.API.DataAccess.Implementations
{
    public class OrderRepository : GenericRepository<Domain.Entities.Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context, ReadDbContext readDbContext) : base(context, readDbContext)
        {
        }

        public async Task<PagingResponseDto> GetPasignatedResult(OrderFilterDto dto)
        {
            var query = _dbContext.Orders.Include(x=> x.OrderItems).AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                query = query.Where(x => x.UserName.ToLower().Contains(dto.UserEmail.ToLower()));

            if (!string.IsNullOrWhiteSpace(dto.UserEmail))
                query = query.Where(x => x.UserEmail.ToLower().Contains(dto.UserEmail.ToLower()));

            var result = await (from order in query
                                .OrderByDescending(x => x.Id)
                                .Skip(dto.Skip)
                                .Take(dto.PageSize)
                                select new OrderDto
                                {
                                    Id = order.Id,
                                    UserName = order.UserName,
                                    UserEmail = order.UserEmail,
                                    Status = order.Status,
                                    GrandTotal = order.GrandTotal,
                                    SubTotal = order.SubTotal,
                                    Tax = order.Tax,
                                    OrderItems = order.OrderItems.Adapt(new List<OrderItemDto>())

                                })
                                .ToListAsync();

            var totalRecords = await query.CountAsync();
            return new PagingResponseDto(result, totalRecords, dto.PageNumber, dto.PageSize);
        }
    }
}
