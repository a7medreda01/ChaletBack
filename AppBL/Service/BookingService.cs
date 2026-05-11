using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.Helpers;
using AppBL.IService;
using AppDAL.Entities;
using AppDAL.IRepo;
using AutoMapper;

namespace AppBL.Service
{

    using AppBL.DTOs;
    using AppBL.DTOs.BookingDTOs;
    using AppBL.IService;
    using AppDAL.Context;
    using AppDAL.Entities;
    using AppDAL.Entities;
    using AppDAL.IRepo;
    using AutoMapper;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IGenericRepository<WaitingList> _waitingRepo;
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IGenericRepository<Pricing> _pricingRepo;
        private readonly IGenericRepository<Chalet> _chaletRepo;
        private readonly IGenericRepository<Holiday> _holidayRepo;
        private readonly IGenericRepository<Extra> _extraRepo;
        private readonly IGenericRepository<BookingExtra> _bookingExtraRepo;
        private readonly HotelDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IBookingNoteService _bookingNoteService;

        public BookingService(
            IBookingRepository bookingRepo,
            IGenericRepository<WaitingList> waitingRepo,
            IGenericRepository<Payment> paymentRepo,
            IGenericRepository<Pricing> pricingRepo,
            IGenericRepository<Chalet> chaletRepo,
            IGenericRepository<Holiday> holidayRepo,
            IGenericRepository<Extra> extraRepo,
            IGenericRepository<BookingExtra> bookingExtraRepo,
            HotelDbContext context,
            IMapper mapper,
            INotificationService notificationService,
            IBookingNoteService bookingNoteService)
        {
            _bookingRepo = bookingRepo;
            _waitingRepo = waitingRepo;
            _paymentRepo = paymentRepo;
            _pricingRepo = pricingRepo;
            _chaletRepo = chaletRepo;
            _holidayRepo = holidayRepo;
            _extraRepo = extraRepo;
            _bookingExtraRepo = bookingExtraRepo;
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
            _bookingNoteService = bookingNoteService;
        }

        // =========================================
        // 🔍 Check Availability
        // =========================================
        public async Task<bool> CheckAvailability(int chaletId, DateTime date, BookingPeriod period)
        {
            return !await _bookingRepo.ExistsAsync(chaletId, date, period);
        }



        // =========================================
        // 🔥 Create Booking
        // =========================================
        public async Task<BookingResponseDto> CreateBooking(CreateBookingDto dto,int userId)
        {
            // Validation
            if (dto.Date < DateTime.Today)
                throw new Exception("التاريخ غير صالح");

            if (string.IsNullOrEmpty(dto.Phone))
                throw new Exception("برجاء ادخال رقم الهاتف");

            // كل الاكواخ من نفس النوع والفترة 
            var existingChalets = await _bookingRepo.RepoGetChaletsByTypeAndPeriodAsync(dto.ChaletType,dto.Period);
            //عدد الاكواخ 
            var existingChaletsCount = existingChalets.Count();
            //عدد المحجوز في اليوم اللي اختاره
            var bookingsSameTypePeriodTime = await _bookingRepo.GetByTypeDatePeriodAsync(dto.ChaletType, dto.Date, dto.Period);
            var bookingsSameTypePeriodTimeCount= bookingsSameTypePeriodTime.Count();
            // 🔁 لو فيه Pending → حوّلهم للـ Waiting List


            if (bookingsSameTypePeriodTimeCount >= existingChaletsCount)
            {
                    var waiting = new WaitingList
                    {
                        CustomerName = dto.CustomerName,
                        Phone = dto.Phone,
                        ChaletType = dto.ChaletType,
                        Date = dto.Date,
                        Period = dto.Period,
                        Status = WaitingStatus.Pending,
                        //Notes = "تم التحويل من حجز Pending",
                        CreatedByUserId = userId,
                        CreatedAt=DateTime.UtcNow,
                        NumOfGuests=dto.NumOfGuests,
                        DiscountAmount=dto.DiscountAmount,
                        AdditionalPhone=dto.AdditionalPhone,
                    };

                    await _waitingRepo.AddAsync(waiting);

                await _waitingRepo.SaveAsync();

                return new BookingResponseDto
                {
                    Success = true,
                    Message = "✓ تم تحويل الحجز لقائمة الانتظار"
                };
            }

            // ✅ Create Booking
            var booking = _mapper.Map<Booking>(dto);
            booking.CreatedByUserId = userId;
            booking.Status = BookingStatus.Pending;
            booking.CreatedAt = DateTime.UtcNow;
            booking.ExpireAt = DateTime.UtcNow.AddMinutes(120);
            booking.ChaletType = dto.ChaletType;
            booking.AdditionalPhone = dto.AdditionalPhone;
            booking.DiscountAmount = dto.DiscountAmount;
            var price = await CalculatePriceDetails(
    dto.ChaletType,
    dto.Period,
    dto.Date,
    dto.Extras
);

            booking.ChaletPrice = price.chaletPrice;
            booking.ExtrasTotal = price.extrasTotal;
            booking.Price = price.total;
            booking.TotalPrice = price.total - dto.DiscountAmount;

            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveAsync();

            if (dto.Extras != null && dto.Extras.Any())
            {
                foreach (var ex in dto.Extras)
                {
                    var extraEntity = await _extraRepo.GetByIdAsync(ex.ExtraId);

                    if (extraEntity == null || !extraEntity.IsActive)
                        throw new Exception("Extra غير صالح");

                    var bookingExtra = new BookingExtra
                    {
                        BookingId = booking.Id,
                        ExtraId = extraEntity.Id,
                        Quantity = ex.Quantity,
                        Price = extraEntity.Price
                    };

                    await _bookingExtraRepo.AddAsync(bookingExtra);
                }

                await _extraRepo.SaveAsync();
            }
            await _notificationService.CreateAsync(
                "حجز جديد",
                $"تم إنشاء حجز عميل: {dto.CustomerName} كوخ  {dto.ChaletType.ToString()} بتاريخ {dto.Date:yyyy-MM-dd} بواسطة {dto.UserName}",
                booking.Id
            );

            
            //حفظ الملاحظة
            if (!string.IsNullOrWhiteSpace(dto.Note))
            {
                var noteEntity = new BookingNote
                {
                    BookingId = booking.Id,
                    Note = $"{dto.Note}",
                    UserName = dto.UserName,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Set<BookingNote>().AddAsync(noteEntity);
                await _context.SaveChangesAsync();
            }
            if (booking.DiscountAmount >0)
            {
                await _bookingNoteService.AddNoteAsync(booking.Id, $"تم عمل خصم  {dto.DiscountAmount}", dto.UserName);
            }
            return new BookingResponseDto
            {
                Success = true,
                Message = "تم إنشاء الحجز بنجاح",
                BookingId = booking.Id
            };
        }
        // =========================================
        // 💰 Calculate Price (Pricing + Extras)
        // =========================================
        public async Task<(decimal chaletPrice, decimal extrasTotal, decimal total)> CalculatePriceDetails(ChaletType chaletType,BookingPeriod period,DateTime date,
     List<AddExtraTOBook> extrasDto = null)
        {
            var dayType = await GetDayType(date);

            var pricing = (await _pricingRepo.GetAllAsync())
                .FirstOrDefault(p =>
                    p.ChaletType == chaletType &&
                    p.Period == period &&
                    p.DayType == dayType);

            if (pricing == null)
                throw new Exception("No pricing configured");

            decimal chaletPrice = pricing.Price;
            decimal extrasTotal = 0;

            if (extrasDto != null && extrasDto.Any())
            {
                foreach (var ex in extrasDto)
                {
                    var extra = await _extraRepo.GetByIdAsync(ex.ExtraId);

                    if (extra != null && extra.IsActive)
                    {
                        extrasTotal += extra.Price * ex.Quantity;
                    }
                }
            }

            return (chaletPrice, extrasTotal, chaletPrice + extrasTotal);
        }
        // =========================================
        // 💳 Confirm Booking
        // =========================================
        public async Task ConfirmBooking(int bookingId, decimal deposit)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);

            if (booking == null)
                throw new Exception("Booking not found");

            if (deposit <= 0 || deposit > booking.TotalPrice)
                throw new Exception("Invalid deposit");

            booking.Status = BookingStatus.Confirmed;
            booking.Deposit = deposit;

            _bookingRepo.Update(booking);

            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = deposit,
                Method = PaymentMethod.Cash,
                Status = PaymentStatus.Paid,
                CreatedAt = DateTime.UtcNow,
                TransactionId = Guid.NewGuid().ToString()
            };

            await _paymentRepo.AddAsync(payment);


            await _bookingRepo.SaveAsync();
        }

        // =========================================
        // ❌ Cancel Booking
        // =========================================
        public async Task CancelBooking(int bookingId,string? notes,string UserName)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);
            if (!string.IsNullOrWhiteSpace(notes))
            {
                var noteEntity = new BookingNote
                {
                    BookingId = bookingId,
                    Note = $"إلغاء الحجز: {notes}",
                    UserName = UserName,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Set<BookingNote>().AddAsync(noteEntity);
                await _context.SaveChangesAsync();
            }
            if (booking == null)
                throw new Exception("Booking not found");
            booking.Status = BookingStatus.Cancelled;
            //booking.Notes = notes;
            _bookingRepo.Update(booking);

            await _bookingRepo.SaveAsync();
            await _notificationService.CreateAsync(
    "الغاء حجز",
    $"تم الغاء حجز  {notes}  رقم {bookingId} بتاريخ {DateTime.UtcNow:yyyy-MM-dd} , بواسطة {UserName}",bookingId
);

        }

        // =========================================
        // 📊 Get All
        // =========================================
        public async Task<PagedResult<BookingDto>> GetBookingsPagedAsync(
            int? userId, bool isPartner,
            int page, int pageSize,
            string? search, string? status,
            string? dateFrom, string? dateTo)
        {
            var (items, total) = await _bookingRepo.GetBookingsPagedAsync(
                userId, isPartner, page, pageSize, search, status, dateFrom, dateTo);

            return new PagedResult<BookingDto>
            {
                Data = _mapper.Map<List<BookingDto>>(items),
                Total = total,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)total / pageSize)
            };
        }

        // =========================================
        // 📄 Get Details
        // =========================================
        public async Task<BookingDto> GetDetails(int id)
        {
            var booking = await _bookingRepo.GetWithDetailsAsync(id);

            if (booking == null)
                throw new Exception("Not found");

            return _mapper.Map<BookingDto>(booking);
        }

        // =========================================
        // 📅 Day Type Logic
        // =========================================
        private async Task<DayType> GetDayType(DateTime date)
        {
            var holidays = await _holidayRepo.GetAllAsync();

            if (holidays.Any(h => h.Date.Date == date.Date))
                return DayType.Holiday;

            if (date.DayOfWeek == DayOfWeek.Friday ||
                date.DayOfWeek == DayOfWeek.Saturday)
                return DayType.Weekend;

            return DayType.Weekday;
        }
        public async Task AddExtraToBooking(AddExtraDto dto)
        {
            var booking = await _bookingRepo.GetByIdAsync(dto.BookingId);

            if (booking == null)
                throw new Exception("Booking not found");

            var extra = await _extraRepo.GetByIdAsync(dto.ExtraId);

            if (extra == null || !extra.IsActive)
                throw new Exception("Extra غير صالح");

            var bookingExtra = new BookingExtra
            {
                BookingId = dto.BookingId,
                ExtraId = dto.ExtraId,
                Quantity = dto.Quantity,
                Price = extra.Price // ✅ سعر الوحدة فقط
            };

            await _bookingExtraRepo.AddAsync(bookingExtra);
            await _bookingExtraRepo.SaveAsync();

            // 🔥 إعادة حساب
            await RecalculateBookingTotals(dto.BookingId);
            await _bookingRepo.SaveAsync();
        }

        public async Task<BookingResponseDto> UpdateBookingAsync(UpdateBookingDto dto,string userName)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.BookingExtras)
                    .FirstOrDefaultAsync(b => b.Id == dto.BookingId);

                if (booking == null)
                    throw new Exception("الحجز غير موجود");

                if (booking.DiscountAmount != dto.DiscountAmount)
                {
                    await _bookingNoteService.AddNoteAsync(dto.BookingId,$"تم تعديل الخصم الي {dto.DiscountAmount}",userName);
                }
                // 1️⃣ تحديث بيانات العميل
                booking.CustomerName = dto.CustomerName;
                booking.Phone = dto.Phone;
                booking.Deposit = dto.Deposit;
                booking.AdditionalPhone = dto.AdditionalPhone;
                booking.DiscountAmount = dto.DiscountAmount;
                // 2️⃣ حذف Extras
                if (dto.RemovedExtraIds != null && dto.RemovedExtraIds.Any())
                {
                    var extrasToRemove = booking.BookingExtras
                        .Where(be => dto.RemovedExtraIds.Contains(be.ExtraId))
                        .ToList();

                    _context.BookingExtras.RemoveRange(extrasToRemove);
                }
                //لو ضاف فلوس
                if (dto.PayMoney > 0)
                {
                    var payment = new Payment
                    {
                        BookingId = booking.Id,
                        Amount = dto.PayMoney,
                        PaymentReson = PaymentReson.Price,
                        Method = PaymentMethod.Cash,
                        Status = PaymentStatus.Paid,
                        CreatedAt = DateTime.UtcNow,
                        TransactionId = Guid.NewGuid().ToString()
                    };
                    await _paymentRepo.AddAsync(payment);
                }

                await _bookingNoteService.AddNoteAsync(dto.BookingId,"تم تعديل الحجز",userName);

                // 🔥 حفظ التغييرات أولًا (عشان الحذف يتم فعليًا)
                await _context.SaveChangesAsync();

                // 3️⃣ إعادة حساب الأسعار
                await RecalculateBookingTotals(booking.Id);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new BookingResponseDto
                {
                    Message = "تم تعديل الحجز بنجاح",
                    BookingId = booking.Id,
                    TotalPrice = booking.TotalPrice
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task RecalculateBookingTotals(int bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                throw new Exception("Booking not found");

            var extras = await _context.BookingExtras
                .Where(x => x.BookingId == bookingId)
                .ToListAsync();

            // 🔥 حساب إجمالي الإضافات
            booking.ExtrasTotal = extras.Sum(x => x.Price * x.Quantity);

            // 🔥 حساب الإجمالي النهائي
            booking.Price = booking.ChaletPrice + (decimal)booking.ExtrasTotal;
            booking.TotalPrice = booking.Price - booking.DiscountAmount;
            _context.Bookings.Update(booking);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByPartnerAsync(int userId)
        {
            var data = await _bookingRepo.GetBookingsByPartnerAsync(userId);
            return _mapper.Map<IEnumerable<BookingDto>>(data);
        }

        public async Task<BookingResponseDto> MarkAsDone(int bookingId, int payMoney, int chaletId, string UserName)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);

            if (booking == null)
                throw new Exception("الحجز غير موجود");

            if (booking.Status == BookingStatus.Cancelled)
                throw new Exception("لا يمكن إنهاء حجز ملغي");

            if (booking.Status == BookingStatus.Done)
                throw new Exception("الحجز منتهي بالفعل");

            var chalet = await _chaletRepo.GetByIdAsync(chaletId);
            if (chalet.Status == ChaletStatus.Maintenance)
                throw new Exception("الكوخ في الصيانة");

            if (chalet == null)
                throw new Exception("الكوخ غير موجود");

            // ✅ 1. لازم نفس النوع
            if (chalet.Type != booking.ChaletType)
                throw new Exception("نوع الكوخ لا يطابق الحجز");

            // ✅ 2. تحقق من الفترة
            bool supportsPeriod =
                (booking.Period == BookingPeriod.Morning && chalet.HasMorning) ||
                (booking.Period == BookingPeriod.Evening && chalet.HasEvening) ||
                (booking.Period == BookingPeriod.Full && chalet.HasFullDay);

            if (!supportsPeriod)
                throw new Exception("الكوخ لا يدعم هذه الفترة");

            // ✅ 3. تحقق إنه مش محجوز
            var isBooked = await _bookingRepo.ExistsAsync(chaletId, booking.Date, booking.Period);

            if (isBooked)
                throw new Exception("الكوخ محجوز بالفعل");

            // ✅ assign chalet
            booking.ChaletId = chaletId;
            booking.Status = BookingStatus.Done;

            _bookingRepo.Update(booking);

            await _bookingRepo.SaveAsync();

            // 💳 payment
            if (payMoney > 0)
            {
                var payment = new Payment
                {
                    BookingId = booking.Id,
                    Amount = payMoney,
                    PaymentReson = PaymentReson.Price,
                    Method = PaymentMethod.Cash,
                    Status = PaymentStatus.Paid,
                    CreatedAt = DateTime.UtcNow,
                    TransactionId = Guid.NewGuid().ToString()
                };

                await _paymentRepo.AddAsync(payment);
                await _paymentRepo.SaveAsync();
            }

            
                var noteEntity = new BookingNote
                {
                    BookingId = bookingId,
                    Note = $"تم تسليم الكوخ",
                    UserName = UserName,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Set<BookingNote>().AddAsync(noteEntity);
                await _context.SaveChangesAsync();
            

            return new BookingResponseDto
            {
                Success = true,
                Message = "تم إنهاء الحجز بنجاح",
                BookingId = booking.Id
            };
        }




        public async Task<IEnumerable<BookingDto>> GetUpcomingBookingsAsync()
        {
            var bookings = await _bookingRepo.GetUpcomingBookingsAsync();

            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        public async Task<IEnumerable<BookingDto>> GetByTypeDatePeriodAsync(
    ChaletType chaletType,
    DateTime date,
    BookingPeriod period)
        {
            var bookings = await _bookingRepo
                .GetByTypeDatePeriodAsync(chaletType, date, period);

            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        public async Task<DashboardDto> GetDashboardAsync(
    int? userId, bool isPartner,
    DateTime from, DateTime to,
    DateTime prevFrom, DateTime prevTo)
        {
            var baseQuery = _context.Bookings
                .Include(b => b.Chalet)
                .Include(b => b.BookingExtras).ThenInclude(e => e.Extra)
                .Include(b => b.CreatedByUser)
                .Include(b => b.Payments)
                .AsNoTracking()
                .AsQueryable();

            if (isPartner && userId.HasValue)
                baseQuery = baseQuery.Where(b =>
                    b.Chalet.ChaletOwners.Any(o => o.UserId == userId));

            var current = await baseQuery.Where(b => b.Date >= from && b.Date <= to).ToListAsync();
            var previous = await baseQuery.Where(b => b.Date >= prevFrom && b.Date <= prevTo).ToListAsync();

            var recent = await baseQuery
                .OrderByDescending(b => b.CreatedAt)
                .Take(8)
                .ToListAsync();

            var chaletsQuery = _context.Chalets.AsNoTracking().AsQueryable();
            if (isPartner && userId.HasValue)
                chaletsQuery = chaletsQuery.Where(c =>
                    c.ChaletOwners.Any(o => o.UserId == userId));
            var chalets = await chaletsQuery.ToListAsync();

            var confirmedDone = current
                .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Done)
                .ToList();

            var prevConfirmedDone = previous
                .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Done)
                .ToList();

            return new DashboardDto
            {
                TotalBookings = current.Count,
                ConfirmedBookings = current.Count(b => b.Status == BookingStatus.Confirmed),
                DoneBookings = current.Count(b => b.Status == BookingStatus.Done),
                PendingBookings = current.Count(b => b.Status == BookingStatus.Pending),
                CancelledBookings = current.Count(b => b.Status == BookingStatus.Cancelled),
                TotalRevenue = confirmedDone.Sum(b => b.TotalPrice),
                ChaletRevenue = confirmedDone.Sum(b => b.ChaletPrice),
                ExtrasRevenue = confirmedDone.Sum(b => (decimal)(b.ExtrasTotal ?? 0)),
                DepositSum = current.Where(b => b.Deposit != null).Sum(b => b.Deposit ?? 0),
                DiscountSum = current.Sum(b => b.DiscountAmount),

                PrevTotalBookings = previous.Count,
                PrevCancelledBookings = previous.Count(b => b.Status == BookingStatus.Cancelled),
                PrevTotalRevenue = prevConfirmedDone.Sum(b => b.TotalPrice),

                RecentBookings = _mapper.Map<List<BookingDto>>(recent),
                Chalets = chalets.Select(c => new ChaletStatusDto
                {
                    Name = c.Name,
                    Status = c.Status.ToString(),
                    Type = c.Type.ToString(),
                }).ToList(),
            };
        }


        public async Task<List<BookingDto>> GetBookingsForExportAsync(
    int? userId, bool isPartner,
    int year, int month)
        {
            var from = new DateTime(year, month, 1);
            var to = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

            var query = _context.Bookings
                .Include(b => b.Chalet)
                .Include(b => b.BookingExtras).ThenInclude(e => e.Extra)
                .Include(b => b.CreatedByUser)
                .Include(b => b.Payments)
                .Include(b => b.Notes)
                .AsNoTracking()
                .Where(b => b.Date >= from && b.Date <= to);

            if (isPartner && userId.HasValue)
                query = query.Where(b =>
                    b.Chalet.ChaletOwners.Any(o => o.UserId == userId));

            var data = await query
                .OrderBy(b => b.Date)
                .ToListAsync();

            return _mapper.Map<List<BookingDto>>(data);
        }


        public  List<CustomerDto> GetCustomersAsync()
        {
            var raw =  _bookingRepo.GetCustomersRawAsync();
            return raw.Select(r => new CustomerDto
            {
                CustomerName = r.CustomerName,
                Phone = r.Phone,
                BookingsCount = r.Count,
                LastBookingDate = r.LastDate.ToString("yyyy-MM-dd")
            }).ToList();
        }
    }
}
