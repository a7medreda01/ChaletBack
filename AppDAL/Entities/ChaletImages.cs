using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class ChaletImage
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public int ChaletId { get; set; }
        public Chalet Chalet { get; set; }
    }
}
