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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly IPokemonService _pokemonService;
        private readonly IReviewerService _reviewerService;
        public ReviewController(
            IReviewService reviewService,
            IMapper mapper,
            IPokemonService pokemonService,
            IReviewerService reviewerService)
        {
            _reviewService = reviewService;
            _mapper = mapper;
            _pokemonService = pokemonService;
            _reviewerService = reviewerService;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(500)]
        public IActionResult GetReviews()
        {
            var reviews = _reviewService.GetReviews();
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);
            return Ok(reviewsDto);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{reviewId:int}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int reviewId)
        {
            var review = _reviewService.GetReview(reviewId);
            if (review == null)
            {
                return NotFound();
            }
            var reviewDto = _mapper.Map<ReviewDto>(review);
            return Ok(reviewDto);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var reviews = _reviewService.GetReviewsOfAPokemon(pokeId);
            if (reviews == null || !reviews.Any())
            {
                return NotFound();
            }
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);
            return Ok(reviewsDto);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto createReview)
        {
            if (createReview == null) return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(createReview);
            reviewMap.Reviewer = _reviewerService.GetReviewer(reviewerId);
            reviewMap.Pokemon = _pokemonService.GetPokemon(pokemonId);
            if (!_reviewService.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPut("{reviewId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updateReview)
        {
            if (updateReview == null || reviewId != updateReview.Id)
            {
                return BadRequest(ModelState);
            }
            var reviewMap = _mapper.Map<Review>(updateReview);
            if (!_reviewService.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{reviewId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            var review = _reviewService.GetReview(reviewId);
            if (review == null)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewService.DeleteReview(review))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}
