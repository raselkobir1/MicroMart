
//namespace User.API.Helper.Client
//{
//    public class InventoryServiceClient
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ILogger<InventoryServiceClient> _logger;

//        public InventoryServiceClient(HttpClient httpClient, ILogger<InventoryServiceClient> logger)
//        {
//            _httpClient = httpClient;
//            _logger = logger;
//        }

//        public async Task<bool> CreateInventoryAsync(Domain.Entities.Product product)
//        {
//            var inventoryRequest = new
//            {
//                ProductId = product.Id,
//                Name = product.Name,
//                SKU = product.SKU,
//                Description = product.Description,
//                Quantity = 0,
//            };
//            var response = await _httpClient.PostAsJsonAsync("api/Inventory/Add", inventoryRequest);

//            if (response.IsSuccessStatusCode)
//            {
//                return true;
//            }

//            _logger.LogError("Failed to create inventory for Product ID {ProductId}", product.Id);
//            return false;
//        }
//    }

//}
