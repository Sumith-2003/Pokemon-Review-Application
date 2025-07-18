namespace PokemonReviewApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // You should store a hashed password in real apps
        public string Role { get; set; }
    }
}