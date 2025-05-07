namespace ApiGateway.Dto
{
    public class KeycloakOptions
    {
        public string ServerUrl { get; set; } = string.Empty;
        public Dictionary<string, RealmOptions> Realms { get; set; } = new();
    }
}
