using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, ITokenService tokenService, IMapper mapper)
        {
            _authService = authService;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(RegisterDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }
            if (await _authService.UserExists(dto.Username))
            {
                return BadRequest("Username is taken");
            }
            var registeredUser = await _authService.Register(dto.Username, dto.Password);
            var user = await _authService.GetUserByUsername(dto.Username);
            var token = await _tokenService.CreateToken(user);
            return Ok(user);
        }
        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login(RegisterDto dto)
        {
            var loginUser = await _authService.Login(dto.Username, dto.Password);
            if (loginUser == null)
            {
                return Unauthorized("Invalid credentials");
            }
            var user = await _authService.GetUserByUsername(dto.Username);
            var token = await _tokenService.CreateToken(user);
            return Ok(new { Token = token });
        }
    }
}