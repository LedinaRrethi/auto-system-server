
namespace DTO.FineDTO
{
    public class FineCreateDTO
    {
        public string PlateNumber { get; set; } = null!;

        public decimal FineAmount { get; set; }

        public string? FineReason { get; set; }

        // Nese nuk ekziston makina ose perdoruesi
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        public string? PersonalId { get; set; }
        public DateTime? FineDate { get; set; } //nese nuk jepet , vendoset ne backend
    }

}
