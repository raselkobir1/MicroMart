
using System.Text.Json;

namespace Order.API.Helper.Client
{
    public class ProductServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductServiceClient> _logger;

        public ProductServiceClient(HttpClient httpClient, ILogger<ProductServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProductApiResponse> GetProductAsync(string Id)
        {
            var response = await _httpClient.GetAsync($"api/Product/Get/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseBody))
                {
                    _logger.LogError("Response body is empty");
                    return new ProductApiResponse();
                }

                var inventoryResponse = JsonSerializer.Deserialize<ProductApiResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                inventoryResponse.IsSuccess = true;
                return inventoryResponse;
            }
            else
            {
                return new ProductApiResponse();
            }
        }
    }
    public class ProductApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = default!;
        public ProductDto Data { get; set; } = default!;
    }
    public class ProductDto
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long? InventoryId { get; set; }
    }
}
