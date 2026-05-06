using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    public class MaintenanceDto
    {
        public int Id { get; set; }

        public int ChaletId { get; set; }

        public string ChaletName { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }   // Open / InProgress / Closed

        public DateTime? CreatedAt { get; set; }
    }
}
