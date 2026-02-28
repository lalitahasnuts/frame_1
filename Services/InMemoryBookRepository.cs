using System;
using System.Collections.Concurrent; // Для потокобезопасного словаря
using System.Collections.Generic;
using BookCatalogService.Models; // Используем нашу модель Book

namespace BookCatalogService.Services
{
   public class InMemoryBookRepository : IBookRepository
   {
      // ConcurrentDictionary - это потокобезопасный словарь,
      // который позволяет безопасно работать с данными из нескольких потоков
      // (важно для веб-сервисов, где запросы обрабатываются параллельно).
      private readonly ConcurrentDictionary<Guid, Book> _books = new();

      // Конструктор: добавляем несколько тестовых книг при создании репозитория
      public InMemoryBookRepository()
      {
         Add(new Book { Title = "The Hitchhiker's Guide to the Galaxy", Author = "Douglas Adams", PublicationYear = 1979 });
         Add(new Book { Title = "1984", Author = "George Orwell", PublicationYear = 1949 });
         Add(new Book { Title = "Dune", Author = "Frank Herbert", PublicationYear = 1965 });
      }

      public IEnumerable<Book> GetAll() => _books.Values; // Возвращаем все книги из словаря
      public Book? GetById(Guid id) => _books.GetValueOrDefault(id); // Получаем книгу по ID

      public Book Add(Book book)
      {
         if (book.Id == Guid.Empty) // Если ID не задан, генерируем новый
         {
            book.Id = Guid.NewGuid();
         }
         _books.TryAdd(book.Id, book); // Добавляем книгу в словарь
         return book;
      }
   }
}