
using System.Text;
using System.Text.Json;

namespace Cart.API.Helper.Client
{
    public class InventoryServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<InventoryServiceClient> _logger;

        public InventoryServiceClient(HttpClient httpClient, ILogger<InventoryServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> UpdateInventoryAsync(object UpdateInventory)
        {
            var jsonContent = new StringContent(
                                    JsonSerializer.Serialize(UpdateInventory),
                                    Encoding.UTF8,
                                    "application/json"
                                    );
            var response = await _httpClient.PutAsync("api/Inventory/Update", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogError("Failed to update inventory");
            return false;
        }
        public async Task<ApiResponse> GetInventoryById(long id)
        {
            var response = await _httpClient.GetAsync($"api/Inventory/Get/{id}");
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
        public InventoryResponse Data { get; set; }
        public string Message { get; set; }
    }
    public class InventoryResponse
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }

}
