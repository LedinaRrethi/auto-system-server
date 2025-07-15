using System.ComponentModel.DataAnnotations;

namespace DTO.DirectorateDTO
{
    public class DirectorateDTO
    {
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string DirectoryName { get; set; } = null!;
    }

}
