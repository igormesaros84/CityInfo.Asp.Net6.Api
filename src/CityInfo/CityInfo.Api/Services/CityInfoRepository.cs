using CityInfo.Api.DbContexts;
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

    public async Task<City?> GetCityByIdAsync(int id, bool includePointsOfInteret)
    {
        if (includePointsOfInteret)
        {
            return await _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        return await _context.Cities.Where(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId)
    {
        return await _context.PointsOfInterest.Where(p => p.CityId ==  cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await _context.PointsOfInterest.Where(p => p.CityId ==  cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
    }
}
