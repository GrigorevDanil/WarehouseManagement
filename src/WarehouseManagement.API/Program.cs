using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API;
using WarehouseManagement.API.Middlewares;
using WarehouseManagement.Core.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(Envelope<object?>), StatusCodes.Status500InternalServerError));
});

builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true; 
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddModules(builder.Configuration);

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();  