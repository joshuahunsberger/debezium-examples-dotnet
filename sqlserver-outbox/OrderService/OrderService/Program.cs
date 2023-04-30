using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderService.DependencyInjection;
using OrderService.Models;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOrderServices(builder.Configuration)
    .AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OrderService"))
        .AddAspNetCoreInstrumentation(options => options.RecordException = true)
        .AddSqlClientInstrumentation(options =>
        {
            options.SetDbStatementForText = true;
            options.RecordException = true;
        })
        .AddConsoleExporter()
        .AddOtlpExporter());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/orders", async (CreateOrderRequest request, IOrderCreationService orderCreationService) =>
{
    await orderCreationService.CreateOrder(request);
    return Results.NoContent();
})
.WithName("CreateOrder")
.WithOpenApi();

app.Run();

