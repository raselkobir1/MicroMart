using FluentValidation.AspNetCore;
using FluentValidation;
using Auth.API;
using Auth.API.Helper.Extensions;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Auth.API.Helper.ServiceFilter;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Auth.API.Helper.Client;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddControllers(options => options.Filters.Add<ModelValidatorFilter>())
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddPersistenceService(builder.Configuration);

//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
//    options.ForwardedForHeaderName = "X-Forwarded-For";
//    options.KnownProxies.Add(IPAddress.Parse("127.0.0.1")); // Localhost (for dev) or API Gateway IP (docker container)
//    options.ForwardedHeaders = ForwardedHeaders.All;
//    //options.ForwardedHeadersRemoval = Microsoft.AspNetCore.HttpOverrides.ForwardedHeadersRemoval.None;
//});

builder.Services.AddHttpClient<UserProfileServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:6600/");
    //client.BaseAddress = new Uri(builder.Configuration["apiBaseUrl:userApiUrl"]);
});
builder.Services.AddHttpClient<SendVerificationEmailClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5101/");
    //client.BaseAddress = new Uri(builder.Configuration["apiBaseUrl:notificationApiUrl"]);
});


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddInfrastructure();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseForwardedHeaders();

app.MapHealthChecks("/health");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
