using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewersDto = _mapper.Map<List<ReviewerDto>>(reviewers);
            return Ok(reviewersDto);
        }
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewerDto = _mapper.Map<ReviewerDto>(reviewer);
            return Ok(reviewerDto);
        }
        [HttpGet("{reviewerId}/Reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            var reviews = _reviewerRepository.GetReviewsOfReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);
            return Ok(reviewsDto);
        }
    }
}
