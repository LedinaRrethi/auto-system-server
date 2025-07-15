

namespace Domain.JWT
{
    public class RefreshToken
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
