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
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GenreDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetGenres()
        {
            var genres = _mapper.Map<List<GenreDTO>>(_genreRepository.GetGenres());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(genres);
        }

        [HttpGet("{genreId:int}")]
        [ProducesResponseType(200, Type = typeof(GenreDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetGenre(int genreId)
        {
            if (!_genreRepository.GenreExists(genreId))
                return NotFound();

            var genre = _mapper.Map<GenreDTO>(_genreRepository.GetGenre(genreId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(genre);
        }

        [HttpGet("books/{genreId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBooksByGenre(int genreId)
        {
            if (!_genreRepository.GenreExists(genreId))
                return NotFound();

            var books = _mapper.Map<List<BookDTO>>(_genreRepository.GetBooksByGenre(genreId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(books);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateGenre([FromBody] GenreDTO genreCreate)
        {
            if (genreCreate == null)
                return BadRequest(ModelState);

            var genre = _genreRepository.GetGenres()
                .FirstOrDefault(g => g.GenreName.Trim().ToUpper() == genreCreate.GenreName.TrimEnd().ToUpper());

            if (genre != null)
            {
                ModelState.AddModelError("", "Genre already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genreMap = _mapper.Map<Genre>(genreCreate);

            if (!_genreRepository.CreateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{genreId:int}")]
        [ProducesResponseType(200, Type = typeof(GenreDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateGenre(int genreId, [FromBody] GenreDTO genreUpdate)
        {
            if (genreUpdate == null)
                return BadRequest(ModelState);

            if (genreId != genreUpdate.Id)
            {
                ModelState.AddModelError("", "Genre ID mismatch");
                return BadRequest(ModelState);
            }

            if (!_genreRepository.GenreExists(genreId))
                return NotFound();

            var duplicate = _genreRepository.GetGenres()
                .Any(g => g.Id != genreId &&
                          g.GenreName.Trim().ToUpper() == genreUpdate.GenreName.Trim().ToUpper());

            if (duplicate)
            {
                ModelState.AddModelError("", "Genre already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genreMap = _mapper.Map<Genre>(genreUpdate);

            if (!_genreRepository.UpdateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            var updated = _mapper.Map<GenreDTO>(_genreRepository.GetGenre(genreId));
            return Ok(updated);
        }
    }
}
