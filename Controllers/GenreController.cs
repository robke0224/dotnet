using AutoMapper;
using dotnet.DTOs;
using dotnet.Interfaces;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Genre>))]
        public IActionResult GetGenres()
        {
            var genres = _mapper.Map<List<GenreDTO>>(_genreRepository.GetGenres());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(genres);
        }

        [HttpGet("{genreId}")]
        [ProducesResponseType(200, Type = typeof(Genre))]
        [ProducesResponseType(404)]
        public IActionResult GetGenre(int genreId)
        {
            if (!_genreRepository.GenreExists(genreId))
            {
                return NotFound();
            }
            var genre = _mapper.Map<GenreDTO>(_genreRepository.GetGenre(genreId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(genre);
        }


        [HttpGet("books/{genreId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        [ProducesResponseType(404)]
        public IActionResult GetBooksByGenre(int genreId)
        {
            if (!_genreRepository.GenreExists(genreId))
            {
                return NotFound();
            }

            var books = _mapper.Map<List<BookDTO>>(_genreRepository.GetBooksByGenre(genreId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(books);
        }
    }
}