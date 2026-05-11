using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs.BookingDTOs;
using AppBL.IService;
using AppDAL.IRepo;
using AutoMapper;

namespace AppBL.Service
{
    public class BookingNoteService : IBookingNoteService
    {
        private readonly IGenericRepository<BookingNote> _noteRepo;
        private readonly IMapper _mapper;

        public BookingNoteService(
            IGenericRepository<BookingNote> noteRepo,
            IMapper mapper)
        {
            _noteRepo = noteRepo;
            _mapper = mapper;
        }

        public async Task AddNoteAsync(int bookingId, string note, string userName)
        {
            var entity = new BookingNote
            {
                BookingId = bookingId,
                Note = note,
                UserName = userName,
                CreatedAt = DateTime.UtcNow
            };

            await _noteRepo.AddAsync(entity);
            await _noteRepo.SaveAsync();
        }

        public async Task<IEnumerable<BookingNoteDTO>> GetByBookingIdAsync(int bookingId)
        {
            var notes = await _noteRepo.GetAllAsync();

            return notes
                .Where(n => n.BookingId == bookingId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new BookingNoteDTO
                {
                    Id = n.Id,
                    BookingId = n.BookingId,
                    Note = n.Note,
                    UserName = n.UserName,
                    CreatedAt = n.CreatedAt
                });
        }
    }
}
