using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppBL.DTOs.BookingDTOs;
using AppBL.IService;
using AppDAL.Context;
using AppDAL.Entities;
using AppDAL.IRepo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppBL.Service
{
    public class WaitingListService : IWaitingListService
    {
        private readonly IGenericRepository<WaitingList> _repo;
        private readonly IWaitingListBookingRepo repoo;
        private readonly IMapper mapper;
        private readonly IBookingRepository bookingRepoo;
        private readonly IBookingService bookingService;
        private readonly HotelDbContext _context;

        public WaitingListService(IGenericRepository<WaitingList> repo,IWaitingListBookingRepo repoo,IMapper mapper,IBookingRepository bookingRepoo,
            IBookingService bookingService,HotelDbContext context)
        {
            _repo = repo;
            this.repoo = repoo;
            this.mapper = mapper;
            this.bookingRepoo = bookingRepoo;
            this.bookingService = bookingService;
            _context = context;
        }

        public async Task<IEnumerable<WaitingListDto>> GetAllAsync()
        {
            var data = await repoo.GetAllBookingsAsync();
            var bookings = mapper.Map<IEnumerable<WaitingListDto>>(data);
            return bookings;
        }

        public async Task<WaitingListDto> CreateAsync(CreateWaitingListDto dto)
        {
            var entity = new WaitingList
            {
                CustomerName = dto.CustomerName,
                Phone = dto.Phone,
                ChaletType=dto.ChaletType,
                Date = dto.Date,
                Period = dto.Period,
               
                Status = WaitingStatus.Pending
            };

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();

            return new WaitingListDto
            {
                Id = entity.Id,
                CustomerName = entity.CustomerName,
                Phone = entity.Phone,
                ChaletType = entity.ChaletType,
                Date = entity.Date,
                Period = entity.Period,
                Status = entity.Status.ToString(),
                
            };
        }



        public async Task<BookingResponseDto> ConvertWaitingToBookingAsync(int waitingId)
        {
            var waiting = await _repo.GetByIdAsync(waitingId);

            if (waiting == null)
                throw new Exception("Waiting record not found");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1️⃣ جلب كل الحجوزات لنفس (Chalet + Date + Period)
                // كل الاكواخ من نفس النوع والفترة 
                var existingChalets = await bookingRepoo.RepoGetChaletsByTypeAndPeriodAsync(waiting.ChaletType, waiting.Period);
                //عدد الاكواخ 
                var existingChaletsCount = existingChalets.Count();
                //عدد المحجوز في اليوم اللي اختاره
                var bookingsSameTypePeriodTime = await bookingRepoo.GetByTypeDatePeriodAsync(waiting.ChaletType, waiting.Date, waiting.Period);
                var bookingsSameTypePeriodTimeCount = bookingsSameTypePeriodTime.Count();
                // 🔁 لو فيه Pending → حوّلهم للـ Waiting List


                if (bookingsSameTypePeriodTimeCount >= existingChaletsCount)
                {
                    return new BookingResponseDto
                    {
                        Success = false,
                        Message = "لا يمكن التحويل، الموعد محجوز بالفعل"
                    };
                }

                // 3️⃣ حساب السعر
                var price = await bookingService.CalculatePriceDetails(
    waiting.ChaletType,
    waiting.Period,
    waiting.Date
);

                // 4️⃣ إنشاء الحجز الجديد
                var booking = new Booking
                {
                    CustomerName = waiting.CustomerName,
                    Phone = waiting.Phone,
                    ChaletType = waiting.ChaletType,
                    Date = waiting.Date.Date,
                    Period = waiting.Period,
                    Status = BookingStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddMinutes(120),
                    NumOfGuests = waiting.NumOfGuests,
                    BookingExtras = waiting.BookingExtras,
                    CreatedByUserId=waiting.CreatedByUserId,
                    ChaletPrice = price.chaletPrice,
                    ExtrasTotal = price.extrasTotal, // غالبًا = 0 هنا
                    TotalPrice = price.total
                };

                await bookingRepoo.AddAsync(booking);
                await bookingRepoo.SaveAsync();

                // 5️⃣ حذف من waiting list
                _repo.Delete(waiting);
                await _repo.SaveAsync();

                // 6️⃣ تأكيد العملية
                await transaction.CommitAsync();

                return new BookingResponseDto
                {
                    Success = true,
                    Message = "تم تحويل الحجز بنجاح",
                    BookingId = booking.Id
                };
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();

                // معالجة خاصة لخطأ الـ Unique Constraint
                return new BookingResponseDto
                {
                    Success = false,
                    Message = "الموعد تم حجزه بالفعل من مستخدم آخر"
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdateWaitingAsync(int id, UpdateWaitingListDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Not found");


            //entity.Notes = dto.Notes;
            entity.Status = dto.Status;

            _repo.Update(entity);
            await _repo.SaveAsync();
        }

    }
    
}
