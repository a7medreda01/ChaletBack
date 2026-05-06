using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class ChaletOwner
    {
        public int Id { get; set; }

        public int ChaletId { get; set; }
        public Chalet Chalet { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }

        // نسبة الملكية
        public decimal SharePercentage { get; set; }
    }
}
