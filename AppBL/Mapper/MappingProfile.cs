using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppBL.DTOs.BookingDTOs;
using AppBL.Helpers;
using AppDAL.Entities;
using AutoMapper;

namespace AppBL.Mapper
{


    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // =================================
            // 🏡 Chalet
            // =================================

            CreateMap<Chalet, ChaletDto>()
    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
    .ForMember(dest => dest.Images, opt => opt.MapFrom<ChaletImageResolver>()).ReverseMap();

            CreateMap<ChaletCreateDto, Chalet>().ReverseMap().ForMember(dest => dest.Images, opt => opt.Ignore()).ReverseMap();

            // =================================
            // 📅 Booking
            // =================================
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ChaletName, opt => opt.MapFrom(src => src.Chalet.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedByUser.FullName))
                .ForMember(dest => dest.Extras, opt => opt.MapFrom(src => src.BookingExtras)).ReverseMap();

            CreateMap<CreateBookingDto, Booking>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // =================================
            // 💰 Payment
            // =================================
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())).ReverseMap();

            // =================================
            // ➕ Extra
            // =================================
            CreateMap<Extra, ExtraDto>().ReverseMap();

            CreateMap<CreateExtraDto, Extra>();

            CreateMap<UpdateExtraDto, Extra>();

            // =================================
            // 🧾 Booking Extra
            // =================================

            CreateMap<BookingExtra, BookingExtraDto>()
                .ForMember(dest => dest.ExtraName, opt => opt.MapFrom(src => src.Extra.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<BookingExtraDto, BookingExtra>()
                .ForMember(dest => dest.Price, opt => opt.Ignore());

            // =================================
            // 🧑 Partner
            // =================================
            CreateMap<Partner, PartnerDto>();
            CreateMap<PartnerDto, Partner>();

            // =================================
            // 🛠 Maintenance
            // =================================
            CreateMap<Maintenance, MaintenanceDto>();
            CreateMap<MaintenanceDto, Maintenance>();

            // =================================
            // ⏳ Waiting List
            // =================================
            CreateMap<WaitingList, WaitingListDto>()
                .ForMember(dest => dest.ChaletName, opt => opt.MapFrom(src => src.Chalet.Name)).ReverseMap();


            //CreateHolidayDto
            CreateMap<Holiday, HolidayDto>().ReverseMap();
            CreateMap<Holiday, CreateHolidayDto>().ReverseMap();
            //pricing
            CreateMap<Pricing, PricingDto>().ReverseMap();
            CreateMap<Pricing, CreatePricingDto>().ReverseMap();
            CreateMap<BookingNote, BookingNoteDTO>().ReverseMap();


        }
    }
}
