using AutoMapper;
using dotnet.DTOs;
using dotnet.Interfaces;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public BookController(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            IGenreRepository genreRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooks()
        {
            var books = _mapper.Map<List<BookDTO>>(_bookRepository.GetBooks());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(books);
        }

        [HttpGet("{bookId:int}")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var book = _mapper.Map<BookDTO>(_bookRepository.GetBook(bookId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateBook([FromBody] BookDTO bookCreate)
        {
            if (bookCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var title = bookCreate.BookTitle.Trim();
            var authorFirstName = bookCreate.AuthorFirstName.Trim();
            var authorLastName = bookCreate.AuthorLastName.Trim();
            var genreName = bookCreate.GenreName.Trim();

            var existingBook = _bookRepository.GetBooks()
                .FirstOrDefault(b =>
                    b.BookTitle.Trim().ToUpper() == title.ToUpper() &&
                    b.BookPublicationDate == bookCreate.BookPublicationDate);

            if (existingBook != null)
            {
                ModelState.AddModelError("", "Book already exists");
                return StatusCode(422, ModelState);
            }

            var author = _authorRepository.GetAuthors()
                .FirstOrDefault(a =>
                    a.FirstName.Trim().ToUpper() == authorFirstName.ToUpper() &&
                    a.LastName.Trim().ToUpper() == authorLastName.ToUpper());

            if (author == null)
            {
                author = new Author
                {
                    FirstName = authorFirstName,
                    LastName = authorLastName,
                    BookAuthors = new List<BookAuthor>()
                };

                if (!_authorRepository.CreateAuthor(author))
                {
                    ModelState.AddModelError("", "Something went wrong while saving author");
                    return StatusCode(500, ModelState);
                }
            }

            var genre = _genreRepository.GetGenres()
                .FirstOrDefault(g => g.GenreName.Trim().ToUpper() == genreName.ToUpper());

            if (genre == null)
            {
                genre = new Genre { GenreName = genreName };

                if (!_genreRepository.CreateGenre(genre))
                {
                    ModelState.AddModelError("", "Something went wrong while saving genre");
                    return StatusCode(500, ModelState);
                }
            }

            var book = new Book
            {
                BookTitle = title,
                BookPublicationDate = bookCreate.BookPublicationDate
            };

            if (!_bookRepository.CreateBook(author.Id, genre.Id, book))
            {
                ModelState.AddModelError("", "Something went wrong while saving book");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        
        [HttpPut("{bookId:int}")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateBook(int bookId, [FromBody] BookDTO bookUpdate)
        {
            if (bookUpdate == null)
                return BadRequest(ModelState);

            if (bookId != bookUpdate.Id)
            {
                ModelState.AddModelError("", "Book ID mismatch");
                return BadRequest(ModelState);
            }

            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var title = bookUpdate.BookTitle.Trim();
            var authorFirstName = bookUpdate.AuthorFirstName.Trim();
            var authorLastName = bookUpdate.AuthorLastName.Trim();
            var genreName = bookUpdate.GenreName.Trim();

            var duplicate = _bookRepository.GetBooks()
                .Any(b => b.Id != bookId &&
                          b.BookTitle.Trim().ToUpper() == title.ToUpper() &&
                          b.BookPublicationDate == bookUpdate.BookPublicationDate);

            if (duplicate)
            {
                ModelState.AddModelError("", "Book already exists");
                return StatusCode(422, ModelState);
            }

            var author = _authorRepository.GetAuthors()
                .FirstOrDefault(a =>
                    a.FirstName.Trim().ToUpper() == authorFirstName.ToUpper() &&
                    a.LastName.Trim().ToUpper() == authorLastName.ToUpper());

            if (author == null)
            {
                author = new Author
                {
                    FirstName = authorFirstName,
                    LastName = authorLastName,
                    BookAuthors = new List<BookAuthor>()
                };

                if (!_authorRepository.CreateAuthor(author))
                {
                    ModelState.AddModelError("", "Something went wrong while saving author");
                    return StatusCode(500, ModelState);
                }
            }

            var genre = _genreRepository.GetGenres()
                .FirstOrDefault(g => g.GenreName.Trim().ToUpper() == genreName.ToUpper());

            if (genre == null)
            {
                genre = new Genre { GenreName = genreName };

                if (!_genreRepository.CreateGenre(genre))
                {
                    ModelState.AddModelError("", "Something went wrong while saving genre");
                    return StatusCode(500, ModelState);
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = new Book
            {
                BookTitle = title,
                BookPublicationDate = bookUpdate.BookPublicationDate
            };

            if (!_bookRepository.UpdateBook(bookId, author.Id, genre.Id, book))
            {
                ModelState.AddModelError("", "Something went wrong while updating book");
                return StatusCode(500, ModelState);
            }

            var updated = _mapper.Map<BookDTO>(_bookRepository.GetBook(bookId));
            return Ok(updated);
        }

     
        [HttpPatch("{bookId:int}")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult PatchBook(int bookId, [FromBody] BookPatchDTO patch)
        {
            if (patch == null)
                return BadRequest(ModelState);

            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var existing = _bookRepository.GetBook(bookId);
            if (existing == null)
                return NotFound();

            
            var existingAuthor = existing.BookAuthors?.FirstOrDefault()?.Author;
            var existingGenre = existing.BookGenres?.FirstOrDefault()?.Genre;

            if (existingAuthor == null || existingGenre == null)
            {
                ModelState.AddModelError("", "Book relations (author/genre) not found.");
                return BadRequest(ModelState);
            }

           
            var newTitle = patch.BookTitle != null ? patch.BookTitle.Trim() : existing.BookTitle;
            var newYear = patch.BookPublicationDate.HasValue ? patch.BookPublicationDate.Value : existing.BookPublicationDate;

           
            var author = existingAuthor;
            if (patch.AuthorFirstName != null || patch.AuthorLastName != null)
            {
                if (string.IsNullOrWhiteSpace(patch.AuthorFirstName) || string.IsNullOrWhiteSpace(patch.AuthorLastName))
                {
                    ModelState.AddModelError("", "To update author, provide both AuthorFirstName and AuthorLastName.");
                    return BadRequest(ModelState);
                }

                var first = patch.AuthorFirstName.Trim();
                var last = patch.AuthorLastName.Trim();

                author = _authorRepository.GetAuthors()
                    .FirstOrDefault(a =>
                        a.FirstName.Trim().ToUpper() == first.ToUpper() &&
                        a.LastName.Trim().ToUpper() == last.ToUpper());

                if (author == null)
                {
                    author = new Author
                    {
                        FirstName = first,
                        LastName = last,
                        BookAuthors = new List<BookAuthor>()
                    };

                    if (!_authorRepository.CreateAuthor(author))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving author");
                        return StatusCode(500, ModelState);
                    }
                }
            }

            
            var genre = existingGenre;
            if (patch.GenreName != null)
            {
                var gname = patch.GenreName.Trim();

                genre = _genreRepository.GetGenres()
                    .FirstOrDefault(g => g.GenreName.Trim().ToUpper() == gname.ToUpper());

                if (genre == null)
                {
                    genre = new Genre { GenreName = gname };

                    if (!_genreRepository.CreateGenre(genre))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving genre");
                        return StatusCode(500, ModelState);
                    }
                }
            }

            
            var duplicate = _bookRepository.GetBooks()
                .Any(b => b.Id != bookId &&
                          b.BookTitle.Trim().ToUpper() == newTitle.ToUpper() &&
                          b.BookPublicationDate == newYear);

            if (duplicate)
            {
                ModelState.AddModelError("", "Book already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookEntity = new Book
            {
                BookTitle = newTitle,
                BookPublicationDate = newYear
            };

            if (!_bookRepository.UpdateBook(bookId, author.Id, genre.Id, bookEntity))
            {
                ModelState.AddModelError("", "Something went wrong while updating book");
                return StatusCode(500, ModelState);
            }

            var updated = _mapper.Map<BookDTO>(_bookRepository.GetBook(bookId));
            return Ok(updated);
        }

            [HttpDelete("{bookId:int}")]
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            [ProducesResponseType(404)]
            [ProducesResponseType(500)]
            public IActionResult DeleteBook(int bookId)
            {
                if (!_bookRepository.BookExists(bookId))
                    return NotFound();

                var bookToDelete = _bookRepository.GetBook(bookId);
                if (bookToDelete == null)
                    return NotFound();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_bookRepository.DeleteBook(bookToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong while deleting book");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully deleted!");
            }

    }

    
}
