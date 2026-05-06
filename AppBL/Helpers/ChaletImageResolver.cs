using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppDAL.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AppBL.Helpers
{
    public class ChaletImageResolver : IValueResolver<Chalet, ChaletDto, List<string>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChaletImageResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<string> Resolve(Chalet source, ChaletDto destination, List<string> destMember, ResolutionContext context)
        {
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            return source.Images?.Select(i => baseUrl+ "/uploads/chalets/" + i.ImageUrl).ToList();
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
}
