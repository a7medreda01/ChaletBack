using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Context;
using AppDAL.Entities;
using AppDAL.IRepo;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.Repo
{
    public interface IChaletRepository : IGenericRepository<Chalet>
    {
        Task<Chalet> GetChaletWithDetails(int id);
        Task<List<Chalet>> GetChaletsByPartnerAsync(int userId);
        Task<List<Chalet>> GetAllWithOwnersAsync();
        Task<List<Chalet>> GetAllChaletsByTypeAndPeriodAsync(ChaletType type,BookingPeriod period);

    }

    public class ChaletRepository : GenericRepository<Chalet>, IChaletRepository
    {
        private readonly HotelDbContext _context;

        public ChaletRepository(HotelDbContext context) : base(context)
        {
            _context = context;
        }

        // 🔍 شاليه بالتفاصيل
        public async Task<Chalet> GetChaletWithDetails(int id)
        {
            return await _context.Chalets
                .Include(c => c.ChaletOwners)
                    .ThenInclude(o => o.User)
                .Include(c => c.Images)
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        // 🤝 الشاليهات الخاصة بشريك معين
        public async Task<List<Chalet>> GetChaletsByPartnerAsync(int userId)
        {
            var res = await _context.ChaletOwners.Include(c=>c.Chalet).ThenInclude(c=>c.Images)
    .Where(c => c.UserId ==userId)
    .ToListAsync();
            var data = res.Select(r => r.Chalet).ToList();
            return data;
        }
        // 📄 كل الشاليهات مع الشركاء
        public async Task<List<Chalet>> GetAllWithOwnersAsync()
        {
            return await _context.Chalets
                .Include(c => c.ChaletOwners)
                    .ThenInclude(o => o.User)
                .Include(c => c.Images)
                .ToListAsync();
        }


        public async Task<List<Chalet>> GetAllChaletsByTypeAndPeriodAsync(
            ChaletType type,
            BookingPeriod period)
        {
            return await _context.Chalets
                .Where(c => c.Type == type)
                .Where(c =>
                    (period == BookingPeriod.Morning && c.HasMorning) ||
                    (period == BookingPeriod.Evening && c.HasEvening) ||
                    (period == BookingPeriod.Full && c.HasFullDay)
                )
                .ToListAsync();
        }

    }
}
