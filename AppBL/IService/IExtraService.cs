using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;

namespace AppBL.IService
{
    public interface IExtraService
    {
        Task<IEnumerable<ExtraDto>> GetAllAsync();
        Task<ExtraDto> GetByIdAsync(int id);
        Task<ExtraDto> CreateAsync(CreateExtraDto dto);
        Task<ExtraDto> UpdateAsync(int id, UpdateExtraDto dto);
        Task DeleteAsync(int id);
    }
}
