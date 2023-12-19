using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;
    const int maxCitiesPageSize = 20;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        /*[FromQuery]*/string? name, 
        string? searchQuery,
        int pageNumber = 1, int pageSize = 10)
    {

        if (pageSize > maxCitiesPageSize)
        {
            pageSize = maxCitiesPageSize;
        }

        var cityEntities = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
        var city = await _cityInfoRepository.GetCityByIdAsync(id, includePointsOfInterest);

        if (city == null)
        {
            return NotFound();
        }

        if (includePointsOfInterest)
        {
            return Ok(_mapper.Map<CityDto>(city));
        }

        return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));

    }
}
