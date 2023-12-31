﻿using CityInfo.Api.Entities;

namespace CityInfo.Api.Services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
    Task<City?> GetCityByIdAsync(int id, bool includePointsOfInteret);
    Task<bool> CityExistsAsync(int cityId);
    Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
    // This is an in memory operation so it doesn't need to be async
    void DeletePointOfInterest(PointOfInterest pointOfInterest);
    Task<bool> CityNameMatchesCityid(string? cityName, int cityId);
    Task<bool> SaveChangesAsync();
}
