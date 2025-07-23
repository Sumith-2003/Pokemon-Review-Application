using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerService _reviewerService;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerService reviewerService, IMapper mapper)
        {
            _reviewerService = reviewerService;
            _mapper = mapper;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerService.GetReviewers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewersDto = _mapper.Map<List<ReviewerDto>>(reviewers);
            return Ok(reviewersDto);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        public ActionResult<ReviewerDto> GetReviewer(int reviewerId)
        {
            if (!_reviewerService.ReviewerExists(reviewerId))
                return NotFound();
            var reviewer = _reviewerService.GetReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewerDto = _mapper.Map<ReviewerDto>(reviewer);
            return Ok(reviewerDto);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{reviewerId}/Reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfReviewer(int reviewerId)
        {
            if (!_reviewerService.ReviewerExists(reviewerId))
                return NotFound();
            var reviews = _reviewerService.GetReviewsOfReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);
            return Ok(reviewsDto);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto createReviewer)
        {
            if (createReviewer == null) return BadRequest(ModelState);
            var reviewer = _reviewerService.GetReviewers()
                                        .Where(o => o.LastName.Trim().ToUpper() == createReviewer.LastName.TrimEnd().ToUpper())
                                        .FirstOrDefault();
            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }
            var reviewerMap = _mapper.Map<Reviewer>(createReviewer);
            if (!_reviewerService.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the reviewer");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updateReviewer)
        {
            if (updateReviewer == null) return BadRequest(ModelState);
            if (reviewerId != updateReviewer.Id) return BadRequest(ModelState);
            if (!_reviewerService.ReviewerExists(reviewerId)) return NotFound();
            var reviewerMap = _mapper.Map<Reviewer>(updateReviewer);
            if (!_reviewerService.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the reviewer");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerService.ReviewerExists(reviewerId)) return NotFound();
            var reviewer = _reviewerService.GetReviewer(reviewerId);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_reviewerService.DeleteReviewer(reviewer))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the reviewer");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}