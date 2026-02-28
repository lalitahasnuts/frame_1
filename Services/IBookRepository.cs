using System;
using System.Collections.Generic;
using BookCatalogService.Models; // Используем нашу модель Book

namespace BookCatalogService.Services
{
   public interface IBookRepository
   {
      IEnumerable<Book> GetAll(); // Получить все книги
      Book? GetById(Guid id);     // Получить книгу по ID
      Book Add(Book book);       // Добавить новую книгу
   }
}
