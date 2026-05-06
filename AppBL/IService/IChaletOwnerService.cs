using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppBL.DTOs.ChaletOwnerDTO;

namespace AppBL.IService
{
    public interface IChaletOwnerService
    {
        Task<ChaletOwnerResponseDto> AddOwnerAsync(AddChaletOwnerDto dto);
        //Task<List<ChaletOwnerResponseDto>> GetOwnersByChalet(int chaletId);

        //Task DistributeProfitAsync(int bookingId);
        Task<List<ChaletDto>> GetChaletsByPartnerAsync(int userId);
        Task<List<ChaletWithPartnersDto>> GetChaletsWithPartnersAsync();

    }
}
