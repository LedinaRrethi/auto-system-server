

namespace Domain.JWT
{
    public class TokenData
    {
        public string Token { get; set; } = null!;
        public string JwtId { get; set; } = null!;
        public DateTime Expiry { get; set; }
    }

}
