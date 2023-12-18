using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;

namespace CityInfo.Api.Profies;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<PointOfInterest, PointOfInterestDto>();
    }
}
