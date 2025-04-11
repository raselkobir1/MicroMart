namespace ApiGateway.Dto
{
    public class AuthorizeResponse
    {
        public bool IsSuccess { get; set; } = false;
        public AuthorizeData Data { get; set; }
        public string Message { get; set; }
    }

    public class AuthorizeData
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
    }
}
