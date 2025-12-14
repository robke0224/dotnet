namespace dotnet.Controllers
{
    using AutoMapper;
    using dotnet.DTOs;
    using dotnet.Interfaces;
    using dotnet.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetAuthors()
        {
            var authors = _mapper.Map<List<AuthorDTO>>(_authorRepository.GetAuthors());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(authors);
        }

        [HttpGet("{authorId:int}")]
        [ProducesResponseType(200, Type = typeof(AuthorDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var author = _mapper.Map<AuthorDTO>(_authorRepository.GetAuthor(authorId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(author);
        }

        [HttpGet("{authorId:int}/books")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var books = _mapper.Map<List<BookDTO>>(_authorRepository.GetBooksByAuthor(authorId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(books);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateAuthor([FromBody] AuthorDTO authorCreate)
        {
            if (authorCreate == null)
                return BadRequest(ModelState);

            var exists = _authorRepository.GetAuthors()
                .Any(a =>
                    a.FirstName.Trim().ToUpper() == authorCreate.FirstName.Trim().ToUpper() &&
                    a.LastName.Trim().ToUpper() == authorCreate.LastName.Trim().ToUpper());

            if (exists)
            {
                ModelState.AddModelError("", "Author already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorMap = _mapper.Map<Author>(authorCreate);

            // nes Author.BookAuthors yra required tavo modelyje
            authorMap.BookAuthors ??= new List<BookAuthor>();

            if (!_authorRepository.CreateAuthor(authorMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        
        [HttpPut("{authorId:int}")]
        [ProducesResponseType(200, Type = typeof(AuthorDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateAuthor(int authorId, [FromBody] AuthorDTO authorUpdate)
        {
            if (authorUpdate == null)
                return BadRequest(ModelState);

            if (authorId != authorUpdate.Id)
            {
                ModelState.AddModelError("", "Author ID mismatch");
                return BadRequest(ModelState);
            }

            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var duplicate = _authorRepository.GetAuthors()
                .Any(a => a.Id != authorId &&
                          a.FirstName.Trim().ToUpper() == authorUpdate.FirstName.Trim().ToUpper() &&
                          a.LastName.Trim().ToUpper() == authorUpdate.LastName.Trim().ToUpper());

            if (duplicate)
            {
                ModelState.AddModelError("", "Author already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorMap = _mapper.Map<Author>(authorUpdate);

            
            authorMap.BookAuthors ??= new List<BookAuthor>();

            if (!_authorRepository.UpdateAuthor(authorMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            var updated = _mapper.Map<AuthorDTO>(_authorRepository.GetAuthor(authorId));
            return Ok(updated);
        }

            [HttpDelete("{authorId:int}")]
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            [ProducesResponseType(404)]
            [ProducesResponseType(500)]
            public IActionResult DeleteAuthor(int authorId)
            {
                if (!_authorRepository.AuthorExists(authorId))
                    return NotFound();

                var authorToDelete = _authorRepository.GetAuthor(authorId);
                if (authorToDelete == null)
                    return NotFound();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_authorRepository.DeleteAuthor(authorToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong while deleting");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully deleted!");
            }

    }

        
}
