using System.ComponentModel.DataAnnotations; // Для атрибутов валидации

namespace BookCatalogService.DTOs
{
   public class CreateBookRequest
   {
      [Required(ErrorMessage = "Title is required.")] // Поле обязательно
      [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
      public required string Title { get; set; }

      [Required(ErrorMessage = "Author is required.")]
      [StringLength(100, MinimumLength = 1, ErrorMessage = "Author must be between 1 and 100 characters.")]
      public required string Author { get; set; }

      [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100.")] // Проверяем год
      public int PublicationYear { get; set; }
   }
}
