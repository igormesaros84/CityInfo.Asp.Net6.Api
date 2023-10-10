using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;
[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController : ControllerBase
{
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _localMailService;
    private readonly CitiesDataStore _citiesDataStore;

    public PointsOfInterestController(
        ILogger<PointsOfInterestController> logger, 
        IMailService localMailService,
        CitiesDataStore citiesDataStore)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
        _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
    }

    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        try
        {
            var city = _citiesDataStore.Cities.Find(c => c.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id: {cityId} could not be found");
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Critical error occured while trying to get city with Id: {cityId}.", ex);
            return StatusCode(500, "A problem happened while trying to handle request");
        }
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
         var city = _citiesDataStore.Cities.Find(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
        if (pointOfInterest == null)
        {
             return NotFound();
        }

        return Ok(pointOfInterest);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, 
        PointOfInterestForCreationDto pointOfInterestForCreationDto)
    {
        var city = _citiesDataStore.Cities.Find(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        // for demo puposses only
        var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

        var finalPointOfInterest = new PointOfInterestDto()
        {
            Id = ++maxPointOfInterestId,
            Name = pointOfInterestForCreationDto.Name,
            Description = pointOfInterestForCreationDto.Description
        };

        city.PointsOfInterest.Add(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, pointOfInterestId = finalPointOfInterest.Id}, finalPointOfInterest);
    }

    [HttpPut("{pointOfInterestId}")]
    public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
    {
        var city = _citiesDataStore.Cities.Find(c => c.Id == cityId);
        if( city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        pointOfInterestFromStore.Name = pointOfInterest.Name;
        pointOfInterestFromStore.Description = pointOfInterest.Description;

        return NoContent();
    }

    [HttpPatch("{pointOfInterestId}")]
    public ActionResult PartiallypdatePointOfInterest(
        int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        var city = _citiesDataStore.Cities.Find(c => c.Id == cityId);
        if( city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        var pointOfinterestToPatch = new PointOfInterestForUpdateDto()
        {
            Name = pointOfInterestFromStore.Name,
            Description = pointOfInterestFromStore.Description,
        };
        patchDocument.ApplyTo(pointOfinterestToPatch, ModelState);

        // This validates if the JsonPatchDocument is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Need to alidate the Dto as the above check wont let us know if for example the name is set to null however that is arequired field
        if (!TryValidateModel(pointOfinterestToPatch))
        {
            return BadRequest(ModelState);
        }

        pointOfInterestFromStore.Name = pointOfinterestToPatch.Name;
        pointOfInterestFromStore.Description = pointOfinterestToPatch.Description;

        return NoContent();
    }

    [HttpDelete("{pointOfInterestId}")]
    public ActionResult DeletePointOfInterst(int cityId, int pointOfInterestId)
    {
        var city = _citiesDataStore.Cities.Find(c => c.Id == cityId);
        if( city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        city.PointsOfInterest.Remove(pointOfInterestFromStore);
        _localMailService.Send("Point of interest deleted", $"Point of interest {pointOfInterestId} was deleted from city Id: {cityId}");
        return NoContent();
    }
}
