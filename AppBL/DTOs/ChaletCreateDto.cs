using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;
using Microsoft.AspNetCore.Http;

namespace AppBL.DTOs
{
    public class ChaletCreateDto
    {
        public string Name { get; set; }
        public ChaletType Type { get; set; }
        public ChaletStatus Status { get; set; }
        public int? PartnerId { get; set; }

        public IFormFileCollection Images { get; set; }
    }
}
