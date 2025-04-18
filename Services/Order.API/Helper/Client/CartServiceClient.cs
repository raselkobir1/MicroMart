
using System.Text.Json;

namespace Order.API.Helper.Client
{
    public class CartServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CartServiceClient> _logger;

        public CartServiceClient(HttpClient httpClient, ILogger<CartServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ApiResponse> GetCartItemsAsync(string sessionId)
        {
            var response = await _httpClient.GetAsync($"api/Cart/GetCart/{sessionId}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseBody))
                {
                    _logger.LogError("Response body is empty");
                    return new ApiResponse();
                }

                var inventoryResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                inventoryResponse.IsSuccess = true;
                return inventoryResponse;
            }
            else
            {
                return new ApiResponse();
            }
        }
    }
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = default!;
        public List<CartItemDto> Data { get; set; } = default!;
    }
    public class CartItemDto
    {
        public string ProductId { get; set; } = default!;
        public string InventoryId { get; set; } = default!;
        public int Quantity { get; set; }
    }
}
