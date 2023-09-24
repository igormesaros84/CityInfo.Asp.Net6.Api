using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    [HttpGet]
    public ActionResult<CityDto[]> GetCities()
    {
        return Ok(CitiesDataStore.Instance.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var city = CitiesDataStore.Instance.Cities.Find(c => c.Id == id);

        if (city == null)
        {
            return NotFound();
        }

        return Ok(city);
            
    }
}
