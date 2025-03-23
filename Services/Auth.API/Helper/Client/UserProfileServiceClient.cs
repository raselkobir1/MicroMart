
namespace Auth.API.Helper.Client
{
    public class UserProfileServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserProfileServiceClient> _logger;

        public UserProfileServiceClient(HttpClient httpClient, ILogger<UserProfileServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> CreateUserProfileAsync(Domain.Entities.User user) 
        {
            var userProfileRequest = new
            {
                AuthUserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Status = 1,
            };
            var response = await _httpClient.PostAsJsonAsync("api/User/Add", userProfileRequest);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogError("Failed to create UserProfile ID {ProductId}", user.Id);
            return false;
        }
    }

}
