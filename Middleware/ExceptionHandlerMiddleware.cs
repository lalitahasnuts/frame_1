using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net; // Для HttpStatusCode
using System.Text.Json; // Для сериализации JSON
using System.Threading.Tasks;
using BookCatalogService.Exceptions; // Наши исключения
using BookCatalogService.Models;     // Наша модель ErrorResponse
using Microsoft.AspNetCore.Hosting;  // Для IWebHostEnvironment
using Microsoft.Extensions.Hosting;  // Для IsDevelopment()
using Microsoft.Extensions.DependencyInjection; // Для GetService

namespace BookCatalogService.Middleware
{
   public class ExceptionHandlerMiddleware
   {
      private readonly RequestDelegate _next;
      private readonly ILogger<ExceptionHandlerMiddleware> _logger;

      public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
      {
         _next = next;
         _logger = logger;
      }

      public async Task InvokeAsync(HttpContext context)
      {
         try
         {
               await _next(context); // Пытаемся выполнить весь оставшийся конвейер
         }
         catch (Exception ex) // Если где-то произошла ошибка...
         {
               var requestId = context.Items["RequestId"]?.ToString() ?? "N/A";
               _logger.LogError(ex, "An unhandled exception occurred. RequestId: {RequestId}", requestId);

               context.Response.ContentType = "application/json"; // Устанавливаем тип контента
               var errorResponse = new ErrorResponse { RequestId = requestId };

               // Определяем тип исключения и формируем соответствующий ответ
               switch (ex)
               {
                  case BookNotFoundException:
                     context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                     errorResponse.ErrorCode = "NOT_FOUND";
                     errorResponse.Message = ex.Message;
                     break;
                  case InvalidInputException invalidEx: // Для наших ошибок валидации
                     context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                     errorResponse.ErrorCode = "BAD_REQUEST";
                     errorResponse.Message = invalidEx.Message;
                     errorResponse.Details = invalidEx.Details; // Детали ошибок валидации
                     break;
                  default: // Для всех остальных, неожиданных ошибок
                     context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                     errorResponse.ErrorCode = "INTERNAL_SERVER_ERROR";
                     errorResponse.Message = "An unexpected error occurred.";
                     
                     // В режиме разработки можем добавить больше деталей
                     // В продакшене не стоит раскрывать внутренние детали ошибок
                     var env = context.RequestServices.GetService<IWebHostEnvironment>();
                     if (env != null && env.IsDevelopment())
                     {
                           errorResponse.Message += $" Details: {ex.Message}";
                     }
                     break;
               }

               // Сериализуем наш объект ошибки в JSON и записываем в ответ
               await context.Response.WriteAsJsonAsync(errorResponse);
         }
      }
   }
}