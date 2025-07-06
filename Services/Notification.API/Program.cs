using FluentValidation.AspNetCore;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Notification.API.Helper.ServiceFilter;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Notification.API.MessageBroker;
using Notification.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddControllers(options => options.Filters.Add<ModelValidatorFilter>())
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<RabbitMQConsummer>();
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
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
app.MapHealthChecks("/health");
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
