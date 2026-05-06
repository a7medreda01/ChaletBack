using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppBL.IService;
using AppDAL.Entities;
using AppDAL.IRepo;
using AutoMapper;

namespace AppBL.Service
{
    public class PricingService : IPricingService
    {
        private readonly IGenericRepository<Pricing> _repo;
        private readonly IMapper _mapper;

        public PricingService(IGenericRepository<Pricing> repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PricingDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();

            return data.Select(x => new PricingDto
            {
                Id = x.Id,
                ChaletType = x.ChaletType.ToString(),
                Period = x.Period.ToString(),
                Price = x.Price,
                DayType = x.DayType.ToString()
            });
        }

        public async Task<PricingDto> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);

            if (x == null) return null;

            return new PricingDto
            {
                Id = x.Id,
                ChaletType = x.ChaletType.ToString(),
                Period = x.Period.ToString(),
                Price = x.Price,
                DayType = x.DayType.ToString()
            };
        }

        public async Task AddAsync(CreatePricingDto dto)
        {
            //var entity = new Pricing
            //{
            //    ChaletType = dto.ChaletType,
            //    Period = dto.Period,
            //    Price = dto.Price,
            //    DayType = dto.DayType
            //};
            var entity =_mapper.Map<Pricing>(dto);
            await _repo.AddAsync(entity);
            _repo.SaveAsync();
        }

        public async Task UpdateAsync(int id, CreatePricingDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Pricing not found");

            entity.ChaletType = dto.ChaletType;
            entity.Period = dto.Period;
            entity.Price = dto.Price;
            entity.DayType = dto.DayType;

            _repo.Update(entity);
            _repo.SaveAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Pricing not found");

            _repo.Delete(entity);
            _repo.SaveAsync();

        }

        // 🔥 Pricing Engine Core
        public async Task<Pricing> GetPricing(ChaletType type, BookingPeriod period, DayType dayType)
        {
            var data = await _repo.GetAllAsync();

            var price = data.FirstOrDefault(x =>
                x.ChaletType == type &&
                x.Period == period &&
                x.DayType == dayType);

            if (price == null)
                throw new Exception("No pricing configured");

            return price;
        }
    }
}
