using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics; // Для Stopwatch
using System.Threading.Tasks;

namespace BookCatalogService.Middleware
{
   public class TimingMiddleware
   {
      private readonly RequestDelegate _next;
      private readonly ILogger<TimingMiddleware> _logger;

      public TimingMiddleware(RequestDelegate next, ILogger<TimingMiddleware> logger)
      {
         _next = next;
         _logger = logger;
      }

      public async Task InvokeAsync(HttpContext context)
      {
         var stopwatch = Stopwatch.StartNew(); // Запускаем таймер
         await _next(context);                 // Передаем управление по конвейеру
         stopwatch.Stop();                     // Останавливаем таймер

         // Получаем RequestId из контекста, который был добавлен RequestIdMiddleware
         var requestId = context.Items["RequestId"]?.ToString() ?? "N/A";

         // Логируем время выполнения
         _logger.LogInformation("Request processing time. RequestId: {RequestId}, ElapsedMilliseconds: {ElapsedMs}ms",
               requestId, stopwatch.ElapsedMilliseconds);
      }
   }
}