
namespace DTO.FineDTO
{
    public class FineFilterDTO : PaginationDTO
    {
        public string? PlateNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
