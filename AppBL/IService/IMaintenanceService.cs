using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;

namespace AppBL.IService
{
    public interface IMaintenanceService
    {
        Task<IEnumerable<MaintenanceDto>> GetAllAsync();
        Task<MaintenanceDto> GetByIdAsync(int id);
        Task<MaintenanceDto> CreateAsync(CreateMaintenanceDto dto);
        Task<MaintenanceDto> UpdateAsync(int id, UpdateMaintenanceDto dto);
        Task DeleteAsync(int id);
    }
}
