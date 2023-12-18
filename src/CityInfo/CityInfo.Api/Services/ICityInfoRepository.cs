﻿using CityInfo.Api.Entities;

namespace CityInfo.Api.Services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City?> GetCityByIdAsync(int id, bool includePointsOfInteret);
    Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
}