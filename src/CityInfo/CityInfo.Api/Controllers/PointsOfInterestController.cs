using AutoMapper;
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
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public PointsOfInterestController(
        ILogger<PointsOfInterestController> logger, 
        IMailService localMailService,
        ICityInfoRepository cityInfoRepository,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
             _logger.LogInformation($"City with id: {cityId} could not be found");
             return NotFound();
        }

        var pointsOfInterestForAcity = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId);

        return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForAcity));
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
             _logger.LogInformation($"City with id: {cityId} could not be found");
             return NotFound();
        }

        var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
    }

    //[HttpPost]
    //public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, 
    //    PointOfInterestForCreationDto pointOfInterestForCreationDto)
    //{
    //    var city = _cityInfoRepository.Cities.Find(c => c.Id == cityId);
    //    if (city == null)
    //    {
    //        return NotFound();
    //    }

    //    // for demo puposses only
    //    var maxPointOfInterestId = _cityInfoRepository.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

    //    var finalPointOfInterest = new PointOfInterestDto()
    //    {
    //        Id = ++maxPointOfInterestId,
    //        Name = pointOfInterestForCreationDto.Name,
    //        Description = pointOfInterestForCreationDto.Description
    //    };

    //    city.PointsOfInterest.Add(finalPointOfInterest);

    //    return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, pointOfInterestId = finalPointOfInterest.Id}, finalPointOfInterest);
    //}

    //[HttpPut("{pointOfInterestId}")]
    //public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
    //{
    //    var city = _cityInfoRepository.Cities.Find(c => c.Id == cityId);
    //    if( city == null)
    //    {
    //        return NotFound();
    //    }

    //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
    //    if (pointOfInterestFromStore == null)
    //    {
    //        return NotFound();
    //    }

    //    pointOfInterestFromStore.Name = pointOfInterest.Name;
    //    pointOfInterestFromStore.Description = pointOfInterest.Description;

    //    return NoContent();
    //}

    //[HttpPatch("{pointOfInterestId}")]
    //public ActionResult PartiallypdatePointOfInterest(
    //    int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    //{
    //    var city = _cityInfoRepository.Cities.Find(c => c.Id == cityId);
    //    if( city == null)
    //    {
    //        return NotFound();
    //    }

    //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
    //    if (pointOfInterestFromStore == null)
    //    {
    //        return NotFound();
    //    }

    //    var pointOfinterestToPatch = new PointOfInterestForUpdateDto()
    //    {
    //        Name = pointOfInterestFromStore.Name,
    //        Description = pointOfInterestFromStore.Description,
    //    };
    //    patchDocument.ApplyTo(pointOfinterestToPatch, ModelState);

    //    // This validates if the JsonPatchDocument is valid
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    // Need to alidate the Dto as the above check wont let us know if for example the name is set to null however that is arequired field
    //    if (!TryValidateModel(pointOfinterestToPatch))
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    pointOfInterestFromStore.Name = pointOfinterestToPatch.Name;
    //    pointOfInterestFromStore.Description = pointOfinterestToPatch.Description;

    //    return NoContent();
    //}

    //[HttpDelete("{pointOfInterestId}")]
    //public ActionResult DeletePointOfInterst(int cityId, int pointOfInterestId)
    //{
    //    var city = _cityInfoRepository.Cities.Find(c => c.Id == cityId);
    //    if( city == null)
    //    {
    //        return NotFound();
    //    }

    //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
    //    if (pointOfInterestFromStore == null)
    //    {
    //        return NotFound();
    //    }

    //    city.PointsOfInterest.Remove(pointOfInterestFromStore);
    //    _localMailService.Send("Point of interest deleted", $"Point of interest {pointOfInterestId} was deleted from city Id: {cityId}");
    //    return NoContent();
    //}
}
