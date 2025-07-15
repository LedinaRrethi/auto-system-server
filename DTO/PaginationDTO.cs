
namespace DTO
{
    public class PaginationDTO
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortField { get; set; } = "CreatedOn"; 
        public string SortOrder { get; set; } = "desc";          
    }

}
