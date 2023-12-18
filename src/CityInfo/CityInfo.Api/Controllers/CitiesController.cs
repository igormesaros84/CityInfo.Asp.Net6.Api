using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;

    public CitiesController(ICityInfoRepository cityInfoRepository)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();

        var results = new List<CityWithoutPointsOfInterestDto>();
        foreach ( var city in cityEntities)
        {
            results.Add(new CityWithoutPointsOfInterestDto()
                {
                    Id = city.Id,
                    Description = city.Description,
                    Name = city.Name,
                });
        }

        return Ok(results);
    }

    //[HttpGet("{id}")]
    //public ActionResult<CityDto> GetCity(int id)
    //{
    //    var city = _cityInfoRepository.Cities.Find(c => c.Id == id);

    //    if (city == null)
    //    {
    //        return NotFound();
    //    }

    //    return Ok(city);
            
    //}
}
