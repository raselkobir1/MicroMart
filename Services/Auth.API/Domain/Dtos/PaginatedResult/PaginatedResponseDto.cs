namespace Auth.API.Domain.Dtos.PaginatedResult
{
    public class PagingResponseDto
    {
        public int TotalRecords { get; set; }
        public int CurrentPageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public object? Data { get; set; }

        public PagingResponseDto(object? data, int totalRecords, int currentPageNumber, int pageSize)
        {
            if (data != null)
            {
                Data = data;
                TotalRecords = totalRecords;
                CurrentPageNumber = currentPageNumber;
                PageSize = pageSize;
                TotalPages = Convert.ToInt32(Math.Ceiling(TotalRecords / (double)pageSize));
                HasNextPage = CurrentPageNumber < TotalPages;
                HasPreviousPage = CurrentPageNumber > 1;
            }
        }
    }
}
