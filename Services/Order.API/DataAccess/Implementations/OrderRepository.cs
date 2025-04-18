using Order.API.DataAccess.DataContext;
using Order.API.DataAccess.Interfaces;
using Order.API.Domain.Dtos;
using Order.API.Domain.Dtos.PaginatedResult;
using Microsoft.EntityFrameworkCore;

namespace Order.API.DataAccess.Implementations
{
    public class OrderRepository : GenericRepository<Domain.Entities.Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context, ReadDbContext readDbContext) : base(context, readDbContext)
        {
        }

        public async Task<PagingResponseDto> GetPasignatedResult(OrderFilterDto dto)
        {
            var query = _dbContext.Orders.AsQueryable();

            //if (!string.IsNullOrWhiteSpace(dto.UserName))
            //    query = query.Where(x => x.Name.ToLower().Contains(dto.Name.ToLower()));

            //if (!string.IsNullOrWhiteSpace(dto.SKU))
            //    query = query.Where(x => x.SKU.ToLower().Contains(dto.SKU.ToLower()));

            var result = await (from order in query
                                .OrderByDescending(x => x.Id)
                                .Skip(dto.Skip)
                                .Take(dto.PageSize)
                                select new OrderDto
                                {
                                    Id = order.Id,
                                    //Name = order.Name,
                                    //SKU = product.SKU,
                                    //Description = product.Description,
                                    ////Quantity = product.Quantity,
                                    ////InventoryHistory = product.InventoryHistory,
                                    //InventoryId = product.InventoryId,
                                    //Price = product.Price,
                                    //Status = product.Status.ToString()
                                })
                                .ToListAsync();

            var totalRecords = await query.CountAsync();
            return new PagingResponseDto(result, totalRecords, dto.PageNumber, dto.PageSize);
        }
    }
}
