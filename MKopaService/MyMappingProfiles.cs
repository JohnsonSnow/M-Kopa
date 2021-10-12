using AutoMapper;
using MKopa.Core.Dtos;
using MKopa.Core.Models;

namespace MKopaService
{
    public class MyMappingProfiles : Profile
    {
        public MyMappingProfiles()
        {
            // Source -> Target
            CreateMap<Sms, SmsSenderDto>();
            CreateMap<PublishedDto, SmsSenderDto>();
        }
    }
}