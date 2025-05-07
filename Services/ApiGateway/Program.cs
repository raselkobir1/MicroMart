using ApiGateway.Dto;
using ApiGateway.Helper.Client;
using ApiGateway.Helper.Middleware;
using ApiGateway.Manager;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.Configure<KeycloakOptions>(
    builder.Configuration.GetSection("KeycloakOptions")
);
builder.Services.AddHttpClient(); // Needed for HttpClient
builder.Services.AddScoped<IKeycloakAuthService, KeycloakAuthService>();


// Add Authentication directly against Keycloak
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        //options.Authority = "http://keycloak:8081/realms/MicroMart"; // When dockerize both gateway and keycloak Realm URL
        options.Authority = "http://localhost:8081/realms/MicroMart"; // local dev Keycloak Realm URL
        options.RequireHttpsMetadata = false; // if not using HTTPS
        //options.Audience = "ocelot-client"; // Keycloak Client ID
        options.Audience = "account";         // Keycloak Client ID
    });

builder.Services.AddAuthorization();
//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//builder.Services.AddOcelot(builder.Configuration)
//    .AddDelegatingHandler<AddUserHeadersDelegatingHandler>(true)
//    .AddCacheManager(x => { x.WithDictionaryHandle(); });

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AddUserHeadersDelegatingHandler>();

//builder.Services.AddHttpClient<ValidateAuthorizeClient>(client =>
//{
//    client.BaseAddress = new Uri("http://localhost:4003/");
//});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<AuthorizationMiddleware>();
app.MapControllers();
//await app.UseOcelot();
app.Run();

