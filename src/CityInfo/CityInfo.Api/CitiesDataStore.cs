using CityInfo.Api.Models;

namespace CityInfo.Api;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }
    public static CitiesDataStore Instance { get; set; } = new CitiesDataStore();

    public CitiesDataStore()
    {
        Cities = new List<CityDto>(
            new CityDto[]
            {
                new CityDto() { Id = 1, Name = "New York", Description = "Big Apple"},
                new CityDto() { Id = 2, Name = "Antwerp", Description = "That weird german band? idk"},
                new CityDto() { Id = 3, Name = "Paris", Description = "Eiffel tower"}
            });
    }
}
