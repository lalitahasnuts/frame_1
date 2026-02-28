using System;

namespace BookCatalogService.Models
{
   public class Book
   {
      public Guid Id { get; set; } = Guid.NewGuid(); // Автоматически генерируем ID
      public required string Title { get; set; }
      public required string Author { get; set; }
      public int PublicationYear { get; set; } // Год публикации
   }
}
