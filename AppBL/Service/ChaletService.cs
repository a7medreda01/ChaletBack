using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppBL.Helpers;
using AppDAL.Context;
using AppDAL.Entities;
using AppDAL.Repo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppBL.Service
{
    public class ChaletService : IChaletService
    {
        private readonly HotelDbContext _context;
        private readonly IMapper _mapper;
        private readonly IChaletRepository chaletRepository;

        public ChaletService(HotelDbContext context, IMapper mapper,IChaletRepository chaletRepository)
        {
            _context = context;
            _mapper = mapper;
            this.chaletRepository = chaletRepository;
        }

        public async Task<ChaletDto> CreateAsync(ChaletCreateDto dto)
        {
            var chalet = new Chalet
            {
                Name = dto.Name,
                Type = dto.Type,
                Status = dto.Status,
                Images = new List<ChaletImage>()
            };

            // مهم: تأكد أنها موجودة
            chalet.Images = new List<ChaletImage>();

            _context.Chalets.Add(chalet);
            await _context.SaveChangesAsync();

            if (dto.Images != null)
            {
                foreach (var img in dto.Images)
                {
                    var path = await FileHelper.SaveImageAsync(img, "uploads/chalets");

                    chalet.Images.Add(new ChaletImage
                    {
                        ChaletId = chalet.Id,
                        ImageUrl = path
                    });
                }

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<ChaletDto>(chalet);
        }
        public async Task<List<ChaletDto>> GetAllAsync()
        {
            var data = await _context.Chalets
                .Include(x => x.Images)
                .ToListAsync();

            return _mapper.Map<List<ChaletDto>>(data);
        }
        public async Task<ChaletDto?> GetByIdAsync(int id)
        {
            var chalet = await _context.Chalets
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (chalet == null)
                return null;

            return _mapper.Map<ChaletDto>(chalet);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var chalet = await _context.Chalets
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (chalet == null)
                return false;

            // حذف الصور من السيرفر (physical files)
            if (chalet.Images != null && chalet.Images.Any())
            {
                foreach (var img in chalet.Images)
                {
                    var fullPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        img.ImageUrl.TrimStart('/')
                    );

                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                }
            }

            _context.Chalets.Remove(chalet);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<ChaletDto> UpdateAsync(ChaletUpdateDto dto)
        {
            var chalet = await _context.Chalets
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (chalet == null)
                throw new Exception("الكوخ غير موجود");

            // ✅ تحديث البيانات الأساسية
            chalet.Name = dto.Name;
            chalet.Status = dto.Status;

            chalet.HasMorning = dto.HasMorning;
            chalet.HasEvening = dto.HasEvening;
            chalet.HasFullDay = dto.HasFullDay;

            // ⚠️ Validation مهم
            if (!chalet.HasMorning && !chalet.HasEvening && !chalet.HasFullDay)
                throw new Exception("لازم الكوخ يكون له فترة واحدة على الأقل");

            // =========================================
            // ❌ حذف الصور
            // =========================================
            if (dto.RemovedImageIds != null && dto.RemovedImageIds.Any())
            {
                var imagesToRemove = chalet.Images
                    .Where(i => dto.RemovedImageIds.Contains(i.Id))
                    .ToList();

                _context.ChaletImages.RemoveRange(imagesToRemove);
            }

            // =========================================
            // ➕ إضافة صور جديدة
            // =========================================
            if (dto.NewImages != null && dto.NewImages.Any())
            {
                foreach (var img in dto.NewImages)
                {
                    var path = await FileHelper.SaveImageAsync(img, "uploads/chalets");

                    chalet.Images.Add(new ChaletImage
                    {
                        ChaletId = chalet.Id,
                        ImageUrl = path
                    });
                }
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<ChaletDto>(chalet);
        }
        ///partinar
        public async Task<List<ChaletDto>> GetChaletsByPartnerAsync(int userId)
        {
            var chalets = await chaletRepository.GetChaletsByPartnerAsync(userId);
            var res = _mapper.Map<List<ChaletDto>>(chalets);
            return res;
        }


        public async Task<List<ChaletDto>> GetAllChaletsByTypeAndPeriodAsync(
            ChaletType type,
            BookingPeriod period)
        {
            var chalets = await chaletRepository
                .GetAllChaletsByTypeAndPeriodAsync(type, period);

            // Mapping بسيط (ممكن تستخدم AutoMapper لو حابب)
            return chalets.Select(c => new ChaletDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type.ToString(),
            }).ToList();
        }
    }
}
