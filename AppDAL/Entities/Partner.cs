using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class Partner
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }   // رصيد الشريك

        public decimal TotalRevenue { get; set; }

        public ICollection<ChaletOwner> ChaletOwners { get; set; }
    }
}
