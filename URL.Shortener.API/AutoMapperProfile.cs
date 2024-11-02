using AutoMapper;
using URL.Shortener.Model;
using URL.Shortener.Model.ViewModels;

namespace URL.Shortener.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ShortenedUrl, ShortenedUrlView>()
                .ForMember(o => o.LongUrl, map => map.MapFrom(o => o.LongUrl))
                .ForMember(o => o.ShortUrl, map => map.MapFrom(o => o.ShortUrl))
                .ForMember(o => o.Code, map => map.MapFrom(o => o.Code))
                .ReverseMap();
        }
    }
}