using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.Service
{
    using AppBL.DTOs;
    using AppBL.DTOs.BookingDTOs;
    using AppBL.IService;
    using AppDAL.Entities;
    using AppDAL.IRepo;

    public class BookingExtraService : IBookingExtraService
    {
        private readonly IGenericRepository<BookingExtra> _repo;
        private readonly IGenericRepository<Extra> _extraRepo;

        public BookingExtraService(
            IGenericRepository<BookingExtra> repo,
            IGenericRepository<Extra> extraRepo)
        {
            _repo = repo;
            _extraRepo = extraRepo;
        }

        public async Task<IEnumerable<BookingExtraDto>> GetByBookingId(int bookingId)
        {
            var data = await _repo.GetAllAsync();
            var extras = await _extraRepo.GetAllAsync();

            return data
                .Where(x => x.BookingId == bookingId)
                .Select(x => new BookingExtraDto
                {
                    Id = x.Id,
                    BookingId = x.BookingId,
                    ExtraId = x.ExtraId,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    ExtraName = extras.FirstOrDefault(e => e.Id == x.ExtraId)?.Name
                });
        }

        public async Task<BookingExtraDto> AddAsync(CreateBookingExtraDto dto)
        {
            // 🔥 نجيب السعر من Extra (snapshot)
            var extra = await _extraRepo.GetByIdAsync(dto.ExtraId);
            if (extra == null)
                throw new Exception("Extra not found");

            var entity = new BookingExtra
            {
                BookingId = dto.BookingId,
                ExtraId = dto.ExtraId,
                Quantity = dto.Quantity,
                Price = extra.Price // snapshot هنا
            };

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();

            return new BookingExtraDto
            {
                Id = entity.Id,
                BookingId = entity.BookingId,
                ExtraId = entity.ExtraId,
                Price = entity.Price,
                Quantity = entity.Quantity
            };
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("BookingExtra not found");

            _repo.Delete(entity);
            await _repo.SaveAsync();
        }
    }
}
