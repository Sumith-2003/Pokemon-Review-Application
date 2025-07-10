namespace PokemonReviewApp.Middleware
{
    public class ErrorResponse  
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o"); // ISO 8601 format
        public string? Details { get; set; } // optional stack trace
        //This class is used to format the error response sent to the client
    }
}
