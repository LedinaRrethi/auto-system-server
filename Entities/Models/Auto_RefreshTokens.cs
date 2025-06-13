using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Auto_RefreshTokens
    {
        [Key]
        public Guid IDPK_RefreshToken { get; set; }

        [Required]
        public string Token { get; set; } = default!;

        [Required]
        public string JwtId { get; set; } = default!;

        [Required]
        public string IDFK_User { get; set; } = default!;

        [ForeignKey(nameof(IDFK_User))]
        public Auto_Users User { get; set; } = default!;

        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string CreatedByIp { get; set; } = default!;

        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}