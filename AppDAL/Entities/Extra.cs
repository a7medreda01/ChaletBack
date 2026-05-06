using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class Extra
    {
        public int Id { get; set; }

        public string Name { get; set; } // زينة - فطار - اغاني
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
