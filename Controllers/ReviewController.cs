using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        public ReviewController(
            IReviewRepository reviewRepository,
            IMapper mapper,
            IPokemonRepository pokemonRepository,
            IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(500)]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews();
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);
            return Ok(reviewsDto);
        }

        [HttpGet("{reviewId:int}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int reviewId)
        {
            var review = _reviewRepository.GetReview(reviewId);
            if (review == null)
            {
                return NotFound();
            }
            var reviewDto = _mapper.Map<ReviewDto>(review);
            return Ok(reviewDto);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var reviews = _reviewRepository.GetReviewsOfAPokemon(pokeId);
            if (reviews == null || !reviews.Any())
            {
                return NotFound();
            }
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);
            return Ok(reviewsDto);
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery]int reviewerId,[FromQuery]int pokemonId,[FromBody] ReviewDto createReview)
        {
            if (createReview == null) return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(createReview);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
    }
}
