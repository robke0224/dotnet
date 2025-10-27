namespace dotnet.Controllers
{
    using AutoMapper;
    using dotnet.DTOs;
    using dotnet.Interfaces;
    using dotnet.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Author>))]
        public IActionResult GetAuthors()
        {
            var authors = _mapper.Map<List<AuthorDTO>>(_authorRepository.GetAuthors());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(404)]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var author = _mapper.Map<AuthorDTO>(_authorRepository.GetAuthor(authorId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(author);
        }

        [HttpGet("{authorId}/books")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(404)]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var books = _mapper.Map<List<BookDTO>>(
                _authorRepository.GetBooksByAuthor(authorId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(books);
        }
    
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAuthor([FromBody] AuthorDTO authorCreate)
        {
            if (authorCreate == null)
            {
                return BadRequest(ModelState);
            }
            var author = _authorRepository.GetAuthors()
                .Where(g => g.FirstName.Trim().ToUpper() == authorCreate.FirstName.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (author != null)
            {
                ModelState.AddModelError("", "Author already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authorMap = _mapper.Map<Author>(authorCreate);
            if (!_authorRepository.CreateAuthor(authorMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }
          
        }
}