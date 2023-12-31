﻿using CityInfo.Api.DbContexts;
using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.Services;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoContext _context;

    public CityInfoRepository(CityInfoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities.OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
    {
        var collection = _context.Cities as IQueryable<City>;

        if(!string.IsNullOrWhiteSpace(name))
        {
            name = name.Trim();
            collection = collection.Where(c => c.Name == name);
        }

        if(!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection.Where(c => c.Name.Contains(searchQuery) || c.Description != null && c.Description.Contains(searchQuery));
        }
        
        var totalItemCount = await collection.CountAsync();

        var paginationMetadata = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

        var collectionToReturn =  await collection
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .OrderBy(c => c.Name).ToListAsync();

        return (collectionToReturn, paginationMetadata);
    }

    public async Task<City?> GetCityByIdAsync(int id, bool includePointsOfInteret)
    {
        if (includePointsOfInteret)
        {
            return await _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        return await _context.Cities.Where(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId);
    }
    public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId)
    {
        return await _context.PointsOfInterest.Where(p => p.CityId ==  cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await _context.PointsOfInterest.Where(p => p.CityId ==  cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
    }

    public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityByIdAsync(cityId, false);

        if (city != null)
        {
            city.PointsOfInterest.Add(pointOfInterest);
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        _context.PointsOfInterest.Remove(pointOfInterest);
    }

    public async Task<bool> CityNameMatchesCityid(string? cityName, int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
    }
}
