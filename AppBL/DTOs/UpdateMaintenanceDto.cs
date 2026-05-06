using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.DTOs
{
public class UpdateMaintenanceDto
    {
        public string Description { get; set; }
        public MaintenanceStatus Status { get; set; }
    }
}
