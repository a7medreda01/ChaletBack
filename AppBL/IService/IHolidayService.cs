using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;

namespace AppBL.IService
{
    public interface IHolidayService
    {
        Task<IEnumerable<HolidayDto>> GetAll();

        Task<HolidayDto> GetById(int id);

        Task Add(CreateHolidayDto dto);

        Task Update(int id, CreateHolidayDto dto);

        Task Delete(int id);

        Task<bool> IsHoliday(DateTime date);
    }
}
