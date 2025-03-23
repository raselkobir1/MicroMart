using FluentValidation.AspNetCore;
using FluentValidation;
using Auth.API;
using Auth.API.Helper.Extensions;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Auth.API.Helper.ServiceFilter;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Auth.API.Helper.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddControllers(options => options.Filters.Add<ModelValidatorFilter>())
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddPersistenceService(builder.Configuration);


builder.Services.AddHttpClient<UserProfileServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:4004/");
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
app.MapHealthChecks("/health");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
