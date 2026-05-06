using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppBL.DTOs.BookingDTOs;

namespace AppBL.IService
{
    public interface IWaitingListService
    {
        Task<IEnumerable<WaitingListDto>> GetAllAsync();
        Task<WaitingListDto> CreateAsync(CreateWaitingListDto dto);
        Task UpdateWaitingAsync(int id, UpdateWaitingListDto dto);
        Task<BookingResponseDto> ConvertWaitingToBookingAsync(int waitingId);

    }
}
