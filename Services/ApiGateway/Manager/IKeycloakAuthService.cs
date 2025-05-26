using ApiGateway.Dto;

namespace ApiGateway.Manager
{
    public interface IKeycloakAuthService
    {
        Task<AuthorizeResponse> GetToken(TokenRequestDto dto);
        Task<AuthorizeResponse> ValidateToken(ValidateTokenRequestDto dto); 
    }
}
