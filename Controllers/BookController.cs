using AutoMapper;
using dotnet.DTOs;
using dotnet.Interfaces;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
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

            // optional: dublikato tikrinimas (Title + PublicationDate)
            var existingBook = _bookRepository.GetBooks()
                .FirstOrDefault(b =>
                    b.BookTitle.Trim().ToUpper() == title.ToUpper() &&
                    b.BookPublicationDate == bookCreate.BookPublicationDate);

            if (existingBook != null)
            {
                ModelState.AddModelError("", "Book already exists");
                return StatusCode(422, ModelState);
            }

            // find or create author (pagal FirstName + LastName)
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
                    BookAuthors = new List<BookAuthor>() // nes Author.BookAuthors yra required
                };

                if (!_authorRepository.CreateAuthor(author))
                {
                    ModelState.AddModelError("", "Something went wrong while saving author");
                    return StatusCode(500, ModelState);
                }
            }

            // find or create genre (pagal GenreName)
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

            // create book entity
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
    }
}
