using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding; // Для ModelStateDictionary

namespace BookCatalogService.Exceptions
{
   public class InvalidInputException : Exception
   {
      // Этот конструктор для обычных ошибок валидации
      public InvalidInputException(string message) : base(message) {
         Details = null;
      }

      // Этот конструктор для ошибок, собранных ASP.NET Core из атрибутов валидации DTO
      public InvalidInputException(ModelStateDictionary modelState) : base("One or more validation errors occurred.")
      {
         Details = modelState
            .Where(x => x.Value != null && x.Value.Errors.Any()) // Только поля с ошибками
            .ToDictionary(
               kvp => kvp.Key, // Имя поля
               kvp => kvp.Value.Errors?.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>() // Список ошибок для поля
            );
      }
      public Dictionary<string, string[]>? Details { get; } // Подробности ошибок
   }
}