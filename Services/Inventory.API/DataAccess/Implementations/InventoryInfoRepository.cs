using Inventory.API.DataAccess.DataContext;
using Inventory.API.DataAccess.Interfaces;
using Inventory.API.Domain.Dtos;
using Inventory.API.Domain.Dtos.PaginatedResult;
using Inventory.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.DataAccess.Implementations
{
    public class InventoryInfoRepository : GenericRepository<InventoryInfo>, IInventoryInfoRepository
    {
        public InventoryInfoRepository(ApplicationDbContext context, ReadDbContext readDbContext) : base(context, readDbContext)
        {
        }

        public async Task<InventoryHistory> GetLastInventoryHistoryByInventoryId(long inventoryId)
        {
            var history = await _dbContext.InventoryHistory.Where(x => x.InventoryInfoId == inventoryId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return history;
        }

        public async Task<PagingResponseDto> GetPasignatedResult(InventoryInfoFilterDto dto)
        {
            var query = _dbContext.InventoryInfo.Include(x => x.InventoryHistory).AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Name))
                query = query.Where(x => x.Name.ToLower().Contains(dto.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(dto.SKU))
                query = query.Where(x => x.SKU.ToLower().Contains(dto.SKU.ToLower()));

            var result = await (from inventory in query
                                .OrderByDescending(x => x.Id)
                                .Skip(dto.Skip)
                                .Take(dto.PageSize)
                                select new InventoryInfoDto
                                {
                                    Id = inventory.Id,
                                    Name = inventory.Name,
                                    SKU = inventory.SKU,
                                    Description = inventory.Description,
                                    ProductId = inventory.ProductId,
                                    Quantity = inventory.Quantity,
                                    InventoryHistory = inventory.InventoryHistory,
                                })
                                .ToListAsync();

            var totalRecords = await query.CountAsync();
            return new PagingResponseDto(result, totalRecords, dto.PageNumber, dto.PageSize);
        }
    }
}
