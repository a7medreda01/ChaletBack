using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;
using Microsoft.AspNetCore.Http;

namespace AppBL.DTOs
{
    public class ChaletUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ChaletStatus Status { get; set; }

        public bool HasMorning { get; set; }
        public bool HasEvening { get; set; }
        public bool HasFullDay { get; set; }

        // صور جديدة
        public List<IFormFile>? NewImages { get; set; }

        // IDs الصور اللي عايز تمسحها
        public List<int>? RemovedImageIds { get; set; }
    }
}
