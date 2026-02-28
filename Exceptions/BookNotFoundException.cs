using System;

namespace BookCatalogService.Exceptions
{
   public class BookNotFoundException : Exception
   {
      public BookNotFoundException(Guid id) : base($"Book with ID '{id}' not found.") { }
   }
}