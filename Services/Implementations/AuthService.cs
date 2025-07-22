using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Repositories.Interfaces;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        public AuthService(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be empty.");
            }
            var user = await _authRepository.GetUserByUsername(username);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<LoginDto> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }
            var user = await _authRepository.Login(username, password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            return _mapper.Map<LoginDto>(user);
        }

        public async Task<RegisterDto> Register(string username, string password)
        {
            var user = await _authRepository.Register(username, password);
            if (user == null)
            {
                throw new InvalidOperationException("Registration failed.");
            }
            return _mapper.Map<RegisterDto>(user);
        }

        public async Task<bool> UserExists(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be empty.");
            }
            var userExists = await _authRepository.UserExists(username);
            return userExists;
        }
    }
}
