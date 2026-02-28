using System.Collections.Generic;

namespace BookCatalogService.Models
{
   public class ErrorResponse
   {
      public string RequestId { get; set; } = string.Empty; // ID запроса для трассировки
      public string ErrorCode { get; set; } = string.Empty; // Код ошибки (например, NOT_FOUND, BAD_REQUEST)
      public string Message { get; set; } = string.Empty;   // Понятное сообщение об ошибке
      // Для ошибок валидации, чтобы показать, какие поля не прошли проверку
      public Dictionary<string, string[]>? Details { get; set; }
   }
}