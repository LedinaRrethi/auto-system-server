using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InspectionDTO
{
    public class InspectionReviewDTO
    {
        public Guid IDFK_InspectionRequest { get; set; }
        public bool IsPassed { get; set; }
        public string? Comment { get; set; }
    }
}

 
