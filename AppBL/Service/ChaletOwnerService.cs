using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppBL.DTOs.ChaletOwnerDTO;
using AppBL.IService;
using AppDAL.Entities;
using AppDAL.IRepo;
using AppDAL.Repo;
using Microsoft.AspNetCore.Identity;

namespace AppBL.Service
{
    public class ChaletOwnerService : IChaletOwnerService
    {
        private readonly IGenericRepository<ChaletOwner> _repo;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IChaletRepository chaletRepository;

        public ChaletOwnerService(
            IGenericRepository<ChaletOwner> repo,
            IGenericRepository<Booking> bookingRepo,
            UserManager<AppUser> userManager,
            IChaletRepository chaletRepository
            )
        {
            _repo = repo;
            _bookingRepo = bookingRepo;
            _userManager = userManager;
            this.chaletRepository = chaletRepository;
        }

        public async Task<ChaletOwnerResponseDto> AddOwnerAsync(AddChaletOwnerDto dto)
        {
            // validation
            if (dto.SharePercentage <= 0 || dto.SharePercentage > 100)
                throw new Exception("النسبة يجب أن تكون بين 0 و 100");

            var entity = new ChaletOwner
            {
                ChaletId = dto.ChaletId,
                UserId = dto.UserId,
                SharePercentage = dto.SharePercentage
            };

            await _repo.AddAsync(entity);

            return new ChaletOwnerResponseDto
            {
                Id = entity.Id,
                ChaletId = entity.ChaletId,
                UserId = entity.UserId,
                SharePercentage = entity.SharePercentage
            };
        }

        //public async Task<List<ChaletOwnerResponseDto>> GetOwnersByChalet(int chaletId)
        //{
        //    var data = await _repo.GetAllAsync(x => x.ChaletId == chaletId);

        //    return data.Select(x => new ChaletOwnerResponseDto
        //    {
        //        Id = x.Id,
        //        ChaletId = x.ChaletId,
        //        UserId = x.UserId,
        //        SharePercentage = x.SharePercentage
        //    }).ToList();
        //}

        // 🔥 توزيع الأرباح
        //public async Task DistributeProfitAsync(int bookingId)
        //{
        //    var booking = await _bookingRepo.GetByIdAsync(bookingId);

        //    if (booking == null)
        //        throw new Exception("الحجز غير موجود");

        //    var owners = await _repo.GetAllAsync(x => x.ChaletId == booking.ChaletId);

        //    foreach (var owner in owners)
        //    {
        //        var user = await _userManager.FindByIdAsync(owner.UserId.ToString());

        //        if (user == null) continue;

        //        var profit = booking.TotalPrice * (owner.SharePercentage / 100);

        //        user.Balance += profit;
        //        user.TotalRevenue += profit;

        //        await _userManager.UpdateAsync(user);
        //    }
        //}

        public async Task<List<ChaletDto>> GetChaletsByPartnerAsync(int userId)
        {
            var chalets = await chaletRepository.GetChaletsByPartnerAsync(userId);

            var result = chalets.Select(c =>
            {
                var owner = c.ChaletOwners.FirstOrDefault(o => o.UserId == userId);

                return new ChaletDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type.ToString(),
                    Status = c.Status.ToString(),
                    SharePercentage = owner?.SharePercentage ?? 0
                };
            }).ToList();

            return result;
        }
        public async Task<List<ChaletWithPartnersDto>> GetChaletsWithPartnersAsync()
        {
            var chalets = await chaletRepository.GetAllWithOwnersAsync();
            // مهم: ترجع Chalet ومعاه ChaletOwners + User

            var result = new List<ChaletWithPartnersDto>();

            foreach (var chalet in chalets)
            {
                var dto = new ChaletWithPartnersDto
                {
                    ChaletId = chalet.Id,
                    ChaletName = chalet.Name,
                    Partners = chalet.ChaletOwners.Select(o => new ChaletPartnerDto
                    {
                        UserId = o.UserId,
                        UserName = o.User?.UserName, // تأكد إن User معمول Include
                        SharePercentage = (double)o.SharePercentage
                    }).ToList()
                };

                result.Add(dto);
            }

            return result;
        }
    }
}
