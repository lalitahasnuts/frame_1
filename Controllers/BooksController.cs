using Microsoft.AspNetCore.Mvc;
using BookCatalogService.Models;
using BookCatalogService.Services;
using BookCatalogService.DTOs;
using BookCatalogService.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookCatalogService.Controllers
{
   [ApiController] // Указывает, что это API-контроллер
   [Route("api/[controller]")] // Определяет маршрут: /api/books
   public class BooksController : ControllerBase // Базовый класс для контроллеров API
   {
      private readonly IBookRepository _repository; // Зависимость на репозиторий

      // Конструктор, через который ASP.NET Core внедрит IBookRepository
      public BooksController(IBookRepository repository)
      {
         _repository = repository;
      }

      // GET /api/books
      [HttpGet]
      public ActionResult<IEnumerable<Book>> GetBooks()
      {
         return Ok(_repository.GetAll()); // 200 OK со списком книг
      }

      // GET /api/books/{id}
      // Например: /api/books/a1b2c3d4-e5f6-7890-1234-567890abcdef
      [HttpGet("{id}")]
      public ActionResult<Book> GetBook(Guid id)
      {
         var book = _repository.GetById(id);
         if (book == null)
         {
               // Если книга не найдена, выбрасываем наше исключение
               throw new BookNotFoundException(id);
         }
         return Ok(book); // 200 OK с найденной книгой
      }

      // POST /api/books
      // Тело запроса: { "title": "...", "author": "...", "publicationYear": ... }
      [HttpPost]
      public ActionResult<Book> CreateBook([FromBody] CreateBookRequest request)
      {
         // ModelState.IsValid проверяет атрибуты валидации из CreateBookRequest
         if (!ModelState.IsValid)
         {
               // Если данные невалидны, выбрасываем наше исключение
               throw new InvalidInputException(ModelState);
         }

         // Дополнительное бизнес-правило: год публикации не может быть в будущем
         if (request.PublicationYear > DateTime.UtcNow.Year)
         {
               throw new InvalidInputException("Publication year cannot be in the future.");
         }

         var newBook = new Book
         {
               Title = request.Title,
               Author = request.Author,
               PublicationYear = request.PublicationYear
         };

         _repository.Add(newBook);
         // 201 Created - стандартный ответ для успешного создания ресурса
         // Возвращаем ссылку на новый ресурс и сам ресурс
         return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
      }
   }
}