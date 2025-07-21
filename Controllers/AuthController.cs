using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Repositories.Interfaces;
using PokemonReviewApp.Services;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto dto)
        {
            if (await _repo.UserExists(dto.Username))
            {
                return BadRequest("Username already exists");
            }
            var user = await _repo.Register(dto.Username, dto.Password);
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto dto)
        {
            var user = await _repo.Login(dto.Username, dto.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }
            var token = await _tokenService.CreateToken(user);
            return Ok(new { Token = token });
        }
    }
}
