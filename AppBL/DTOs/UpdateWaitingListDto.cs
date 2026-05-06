using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.DTOs
{
    public class UpdateWaitingListDto
    {
        public WaitingStatus Status { get; set; }
    }
}
