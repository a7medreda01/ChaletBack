using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppDAL.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AppBL.Helpers
{
    public class ChaletImageResolver : IValueResolver<Chalet, ChaletDto, List<string>>
    {
        private readonly string _baseUrl;

        public ChaletImageResolver(IConfiguration configuration)
        {
            _baseUrl = configuration["BaseUrl"];

        }

        public List<string> Resolve(Chalet source, ChaletDto destination, List<string> destMember, ResolutionContext context)
        {
            return source.Images?
                .Select(i => $"{_baseUrl}/uploads/chalets{i.ImageUrl}")
                .ToList();
        }
    }

    //public class CreateChaletImageResolver : IValueResolver<Chalet, ChaletCreateDto, List<string>>
    //{
    //    private readonly IHttpContextAccessor _httpContextAccessor;

    //    public CreateChaletImageResolver(IHttpContextAccessor httpContextAccessor)
    //    {
    //        _httpContextAccessor = httpContextAccessor;
    //    }

    //    public List<string> Resolve(Chalet source, ChaletCreateDto destination, List<string> destMember, ResolutionContext context)
    //    {
    //        var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

    //        return source.Images?.Select(i => baseUrl + i.ImageUrl).ToList();
    //    }
    //}

    public class ChaletImageObjectResolver : IValueResolver<Chalet, ChaletDto, List<ChaletImageDto>>
    {
        private readonly IConfiguration _config;
        public ChaletImageObjectResolver(IConfiguration config)
        {
            _config = config;
        }
        public List<ChaletImageDto> Resolve(Chalet source, ChaletDto destination,
            List<ChaletImageDto> destMember, ResolutionContext context)
        {
            var baseUrl = _config["BaseUrl"] ?? "";
            return source.Images?.Select(img => new ChaletImageDto
            {
                Id = img.Id,
                ImageUrl = $"{baseUrl}/uploads/chalets/{img.ImageUrl}"  // ← أضف /uploads/chalets/
            }).ToList() ?? new List<ChaletImageDto>();
        }
    }
}
