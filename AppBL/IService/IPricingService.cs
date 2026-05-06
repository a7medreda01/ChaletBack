using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppDAL.Entities;

namespace AppBL.IService
{
    public interface IPricingService
    {
        Task<IEnumerable<PricingDto>> GetAllAsync();
        Task<PricingDto> GetByIdAsync(int id);
        Task AddAsync(CreatePricingDto dto);
        Task UpdateAsync(int id, CreatePricingDto dto);
        Task DeleteAsync(int id);

        // 🔥 دي أهم واحدة
        Task<Pricing> GetPricing(ChaletType type, BookingPeriod period, DayType dayType);
    }
}
