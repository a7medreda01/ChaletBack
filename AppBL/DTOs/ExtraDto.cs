using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    public class ExtraDto
    {
        public int Id { get; set; }

        public string Name { get; set; }   // زينة - فطار - مشروبات

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
