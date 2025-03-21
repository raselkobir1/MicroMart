using Product.API.DataAccess.DataContext;
using Product.API.DataAccess.Interfaces;
using Product.API.Domain.Dtos;
using Product.API.Domain.Dtos.PaginatedResult;
using Microsoft.EntityFrameworkCore;

namespace Product.API.DataAccess.Implementations
{
    public class ProductRepository : GenericRepository<Domain.Entities.Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context, ReadDbContext readDbContext) : base(context, readDbContext)
        {
        }

        public async Task<PagingResponseDto> GetPasignatedResult(ProductFilterDto dto)
        {
            var query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Name))
                query = query.Where(x => x.Name.ToLower().Contains(dto.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(dto.SKU))
                query = query.Where(x => x.SKU.ToLower().Contains(dto.SKU.ToLower()));

            var result = await (from product in query
                                .OrderByDescending(x => x.Id)
                                .Skip(dto.Skip)
                                .Take(dto.PageSize)
                                select new ProductDto
                                {
                                    Id = product.Id,
                                    Name = product.Name,
                                    SKU = product.SKU,
                                    Description = product.Description,
                                    //Quantity = product.Quantity,
                                    //InventoryHistory = product.InventoryHistory,
                                    InventoryId = product.InventoryId,
                                    Price = product.Price,
                                    Status = product.Status.ToString()
                                })
                                .ToListAsync();

            var totalRecords = await query.CountAsync();
            return new PagingResponseDto(result, totalRecords, dto.PageNumber, dto.PageSize);
        }

        public async Task RemoveProduct(Domain.Entities.Product product)
        {
            _dbContext.Products.Remove(product); 
            await _dbContext.SaveChangesAsync();
        }
    }
}
