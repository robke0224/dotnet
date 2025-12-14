namespace dotnet.Controllers
{
    using AutoMapper;
    using dotnet.DTOs;
    using dotnet.Interfaces;
    using dotnet.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(
            IReviewRepository reviewRepository,
            IBookRepository bookRepository,
            IReviewerRepository reviewerRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("{reviewId:int}")]
        [ProducesResponseType(200, Type = typeof(ReviewDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("bybook/{bookId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByBook(int bookId)
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviewsByBook(bookId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview([FromBody] ReviewDTO reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            var created = _reviewRepository.CreateReview(
                reviewCreate.ReviewerFirstName,
                reviewCreate.ReviewerLastName,
                reviewMap);

            if (!created)
            {
                ModelState.AddModelError("", "Something went wrong while saving (maybe book title not found).");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{reviewId:int}")]
        [ProducesResponseType(200, Type = typeof(ReviewDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDTO reviewUpdate)
        {
            if (reviewUpdate == null)
                return BadRequest(ModelState);

            if (reviewId != reviewUpdate.Id)
            {
                ModelState.AddModelError("", "Review ID mismatch");
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var title = reviewUpdate.BookTitle.Trim();
            var book = _bookRepository.GetBook(title);
            if (book == null)
            {
                ModelState.AddModelError("", "Book not found");
                return NotFound(ModelState);
            }

            var first = reviewUpdate.ReviewerFirstName.Trim();
            var last = reviewUpdate.ReviewerLastName.Trim();

            var reviewer = _reviewerRepository.GetReviewers()
                .FirstOrDefault(r =>
                    r.FirstName.Trim().ToUpper() == first.ToUpper() &&
                    r.LastName.Trim().ToUpper() == last.ToUpper());

            if (reviewer == null)
            {
                reviewer = new Reviewer
                {
                    FirstName = first,
                    LastName = last,
                    Reviews = new List<Review>()
                };

                if (!_reviewerRepository.CreateReviewer(reviewer))
                {
                    ModelState.AddModelError("", "Something went wrong while saving reviewer");
                    return StatusCode(500, ModelState);
                }
            }

            var reviewMap = _mapper.Map<Review>(reviewUpdate);
            reviewMap.Id = reviewId;
            reviewMap.Book = book;
            reviewMap.Reviewer = reviewer;

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating review");
                return StatusCode(500, ModelState);
            }

            var updated = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));
            return Ok(updated);
        }

        
        [HttpPatch("{reviewId:int}")]
        [ProducesResponseType(200, Type = typeof(ReviewDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult PatchReview(int reviewId, [FromBody] ReviewPatchDTO patch)
        {
            if (patch == null)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var existing = _reviewRepository.GetReview(reviewId);
            if (existing == null)
                return NotFound();

            
            if (patch.ReviewText != null)
                existing.ReviewText = patch.ReviewText.Trim();

            
            if (patch.BookTitle != null)
            {
                var title = patch.BookTitle.Trim();
                var book = _bookRepository.GetBook(title);
                if (book == null)
                {
                    ModelState.AddModelError("", "Book not found");
                    return NotFound(ModelState);
                }

                existing.BookTitle = title;
                existing.Book = book;
            }

            
            if (patch.ReviewerFirstName != null || patch.ReviewerLastName != null)
            {
                if (string.IsNullOrWhiteSpace(patch.ReviewerFirstName) || string.IsNullOrWhiteSpace(patch.ReviewerLastName))
                {
                    ModelState.AddModelError("", "To update reviewer, provide both ReviewerFirstName and ReviewerLastName.");
                    return BadRequest(ModelState);
                }

                var first = patch.ReviewerFirstName.Trim();
                var last = patch.ReviewerLastName.Trim();

                var reviewer = _reviewerRepository.GetReviewers()
                    .FirstOrDefault(r =>
                        r.FirstName.Trim().ToUpper() == first.ToUpper() &&
                        r.LastName.Trim().ToUpper() == last.ToUpper());

                if (reviewer == null)
                {
                    reviewer = new Reviewer
                    {
                        FirstName = first,
                        LastName = last,
                        Reviews = new List<Review>()
                    };

                    if (!_reviewerRepository.CreateReviewer(reviewer))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving reviewer");
                        return StatusCode(500, ModelState);
                    }
                }

                existing.Reviewer = reviewer;
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.UpdateReview(existing))
            {
                ModelState.AddModelError("", "Something went wrong while updating review");
                return StatusCode(500, ModelState);
            }

            var updated = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));
            return Ok(updated);
        }


                [HttpDelete("{reviewId:int}")]
                [ProducesResponseType(200)]
                [ProducesResponseType(400)]
                [ProducesResponseType(404)]
                [ProducesResponseType(500)]
                public IActionResult DeleteReview(int reviewId)
                {
                    if (!_reviewRepository.ReviewExists(reviewId))
                        return NotFound();

                    var reviewToDelete = _reviewRepository.GetReview(reviewId);
                    if (reviewToDelete == null)
                        return NotFound();

                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    if (!_reviewRepository.DeleteReview(reviewToDelete))
                    {
                        ModelState.AddModelError("", "Something went wrong while deleting review");
                        return StatusCode(500, ModelState);
                    }

                    return Ok("Successfully deleted!");
                }

    }
}
