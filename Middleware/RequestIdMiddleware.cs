using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookCatalogService.Middleware
{
   public class RequestIdMiddleware
   {
      private readonly RequestDelegate _next; // Ссылка на следующий middleware в конвейере
      private readonly ILogger<RequestIdMiddleware> _logger; // Сервис логирования

      public RequestIdMiddleware(RequestDelegate next, ILogger<RequestIdMiddleware> logger)
      {
         _next = next;
         _logger = logger;
      }

      // Метод InvokeAsync вызывается для каждого запроса
      public async Task InvokeAsync(HttpContext context)
      {
         // Генерируем уникальный ID для каждого запроса
         var requestId = Guid.NewGuid().ToString();
         // Сохраняем ID в контексте HTTP-запроса, чтобы другие Middleware могли к нему получить доступ
         context.Items["RequestId"] = requestId;

         // Логируем начало запроса
         _logger.LogInformation("Request started. RequestId: {RequestId}, Method: {Method}, Path: {Path}",
               requestId, context.Request.Method, context.Request.Path);

         await _next(context); // Передаем управление следующему middleware

         // Логируем завершение запроса
         _logger.LogInformation("Request finished. RequestId: {RequestId}, Status: {StatusCode}",
               requestId, context.Response.StatusCode);
      }
   }
}