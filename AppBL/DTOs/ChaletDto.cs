using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    public class ChaletDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public List<string> Images { get; set; }
        public List<ChaletImageDto> ImageObjects { get; set; } // ← جديد

        public int? PartnerId { get; set; }
        public decimal SharePercentage { get; set; } // نسبة الشريك في الشاليه
        public bool HasMorning { get; set; } = false;
        public bool HasEvening { get; set; } = false;
        public bool HasFullDay { get; set; } = false;
    }
}
