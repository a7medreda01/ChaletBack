using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.Service
{
    using AppBL.DTOs;
    using AppBL.IService;
    using AppDAL.Entities;
    using AppDAL.IRepo;
    using AutoMapper;

    public class HolidayService : IHolidayService
    {
        private readonly IGenericRepository<Holiday> _holidayRepo;
        private readonly IMapper _mapper;

        public HolidayService(
            IGenericRepository<Holiday> holidayRepo,
            IMapper mapper)
        {
            _holidayRepo = holidayRepo;
            _mapper = mapper;
        }

        // =========================================
        // 📄 Get All
        // =========================================
        public async Task<IEnumerable<HolidayDto>> GetAll()
        {
            var data = await _holidayRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<HolidayDto>>(data);
        }

        // =========================================
        // 📄 Get By Id
        // =========================================
        public async Task<HolidayDto> GetById(int id)
        {
            var holiday = await _holidayRepo.GetByIdAsync(id);

            if (holiday == null)
                throw new Exception("Holiday not found");

            return _mapper.Map<HolidayDto>(holiday);
        }

        // =========================================
        // ➕ Add
        // =========================================
        public async Task Add(CreateHolidayDto dto)
        {
            var holiday = _mapper.Map<Holiday>(dto);

            await _holidayRepo.AddAsync(holiday);
            await _holidayRepo.SaveAsync();
        }

        // =========================================
        // ✏ Update
        // =========================================
        public async Task Update(int id, CreateHolidayDto dto)
        {
            var holiday = await _holidayRepo.GetByIdAsync(id);

            if (holiday == null)
                throw new Exception("Holiday not found");

            holiday.Name = dto.Name;
            holiday.Date = dto.Date;

            _holidayRepo.Update(holiday);
            await _holidayRepo.SaveAsync();
        }

        // =========================================
        // ❌ Delete
        // =========================================
        public async Task Delete(int id)
        {
            var holiday = await _holidayRepo.GetByIdAsync(id);

            if (holiday == null)
                throw new Exception("Holiday not found");

            _holidayRepo.Delete(holiday);
            await _holidayRepo.SaveAsync();
        }

        // =========================================
        // 🔍 Check Holiday
        // =========================================
        public async Task<bool> IsHoliday(DateTime date)
        {
            var holidays = await _holidayRepo.GetAllAsync();

            return holidays.Any(h => h.Date.Date == date.Date);
        }
    }
}
