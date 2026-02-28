using BookCatalogService.Services;
using BookCatalogService.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 1. RequestIdMiddleware - генерирует ID запроса
app.UseMiddleware<RequestIdMiddleware>();
// 2. TimingMiddleware - измеряет время выполнения всего, что ниже
app.UseMiddleware<TimingMiddleware>();
// 3. ExceptionHandlerMiddleware - ловит все ошибки, которые произойдут ниже
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();