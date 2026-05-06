using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppDAL.Entities;

namespace AppBL.Service
{
    public interface IChaletService
    {
        Task<ChaletDto> CreateAsync(ChaletCreateDto dto);
        Task<List<ChaletDto>> GetAllAsync();
        Task<ChaletDto?> GetByIdAsync(int id);
        Task<ChaletDto?> UpdateAsync(ChaletUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<ChaletDto>> GetChaletsByPartnerAsync(int userId);
        Task<List<ChaletDto>> GetAllChaletsByTypeAndPeriodAsync(
    ChaletType type,
    BookingPeriod period);

    }
}
