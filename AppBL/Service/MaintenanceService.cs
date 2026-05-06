using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.Service
{
    using System.Runtime.InteropServices;
    using AppBL.DTOs;
    using AppBL.IService;
    using AppDAL.Entities;
    using AppDAL.IRepo;

    public class MaintenanceService : IMaintenanceService
    {
        private readonly IGenericRepository<Maintenance> _repo;
        private readonly IGenericRepository<Chalet> _chaletRepo;

        public MaintenanceService(
            IGenericRepository<Maintenance> repo,
            IGenericRepository<Chalet> chaletRepo)
        {
            _repo = repo;
            _chaletRepo = chaletRepo;
        }

        public async Task<IEnumerable<MaintenanceDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();

            return data.Select(x => new MaintenanceDto
            {
                Id = x.Id,
                ChaletId = x.ChaletId,
                Description = x.Description,
                Status = x.Status.ToString(),
                //ChaletName = x?.Chalet.Name,          // ✅ مهم
                //CreatedAt = x.CreatedAt
            });
        }

        public async Task<MaintenanceDto> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Maintenance not found");

            return new MaintenanceDto
            {
                Id = entity.Id,
                ChaletId = entity.ChaletId,
                Description = entity.Description,
                Status = entity.Status.ToString()
            };
        }

        public async Task<MaintenanceDto> CreateAsync(CreateMaintenanceDto dto)
        {
            var entity = new Maintenance
            {
                ChaletId = dto.ChaletId,
                Description = dto.Description,
                Status = MaintenanceStatus.Open,
                CreatedAt = DateTime.Now // لو عندك العمود
            };

            await _repo.AddAsync(entity);

            var chalet = await _chaletRepo.GetByIdAsync(dto.ChaletId);
            if (chalet != null)
            {
                chalet.Status = ChaletStatus.Maintenance;
                _chaletRepo.Update(chalet);
            }

            await _repo.SaveAsync();

            return new MaintenanceDto
            {
                Id = entity.Id,
                ChaletId = entity.ChaletId,
                Description = entity.Description,
                Status = entity.Status.ToString(),
                ChaletName = chalet?.Name,          // ✅ مهم
                CreatedAt = entity.CreatedAt        // ✅ مهم
            };
        }
        public async Task<MaintenanceDto> UpdateAsync(int id, UpdateMaintenanceDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Maintenance not found");

            entity.Description = dto.Description;
            entity.Status = dto.Status;

            _repo.Update(entity);

            // 🔥 جلب الشاليه
            var chalet = await _chaletRepo.GetByIdAsync(entity.ChaletId);

            if (chalet != null)
            {
                if (dto.Status == MaintenanceStatus.Closed)
                {
                    // لما الصيانة تخلص ➜ متاح
                    chalet.Status = ChaletStatus.Available;
                }
                else
                {
                    // لو لسه صيانة
                    chalet.Status = ChaletStatus.Maintenance;
                }

                _chaletRepo.Update(chalet);
            }

            await _repo.SaveAsync();

            return new MaintenanceDto
            {
                Id = entity.Id,
                ChaletId = entity.ChaletId,
                Description = entity.Description,
                Status = entity.Status.ToString()
            };
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Maintenance not found");

            var chalet = await _chaletRepo.GetByIdAsync(entity.ChaletId);

            // لو هتحذف طلب الصيانة → رجّع الشاليه Available
            if (chalet != null)
            {
                chalet.Status = ChaletStatus.Available;
                _chaletRepo.Update(chalet);
            }

            _repo.Delete(entity);
            await _repo.SaveAsync();
        }
    }
}