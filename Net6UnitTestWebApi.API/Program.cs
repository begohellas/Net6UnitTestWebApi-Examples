using Microsoft.EntityFrameworkCore;
using Net6UnitTestWebApi.API.Data;
using Net6UnitTestWebApi.API.Filter;
using Net6UnitTestWebApi.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<FilterGlobalException>();
});

builder.Services.AddDbContext<MyWorldDbContext>(opts =>
{
    opts.UseInMemoryDatabase(nameof(Net6UnitTestWebApi));
});

builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.RoutePrefix = string.Empty;
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", nameof(Net6UnitTestWebApi));
    });
}

app.MapControllers();

app.Run();