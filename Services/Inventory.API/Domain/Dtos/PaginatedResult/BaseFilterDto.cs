namespace Inventory.API.Domain.Dto.Common.PaginatedResult
{
    public class BaseFilterDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        internal int Skip 
        { 
            get 
            {
                return (this.PageNumber - 1) * this.PageSize; 
            } 
        }
    }
}
