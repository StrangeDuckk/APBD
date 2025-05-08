using Cw_7_s30338.Exceptions;
using Cw_7_s30338.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace Cw_7_s30338.Services;

public interface IDbService
{
    public Task<IEnumerable<TripGetDTO>> GetTripsInfo();
    public Task<IEnumerable<TripCountryGetDTO>> GetTripsInfoAndCountries();
}

public class DbService(IConfiguration configuration): IDbService
{
    public async Task<IEnumerable<TripGetDTO>> GetTripsInfo()
    {
        // ---------- wypisanie informacji o wycieczkach (bez krajow) -----------
        
        var result = new List<TripGetDTO>();
        
        var connectionString = configuration.GetConnectionString("Default");
        
        await using var connection = new SqlConnection(connectionString);
        var sql = "select IdTrip, Name, Description,DateFrom, DateTo,MaxPeople from Trip";
        
        await using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new NotFoundException("Nie znaleziono zadnej wycieczki");
        }
        while (await reader.ReadAsync())
        {
            result.Add(new TripGetDTO
            {
                IdTrip = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                DateFrom = reader.GetDateTime(3),
                DateTo = reader.GetDateTime(4),
                MaxPeople = reader.GetInt32(5)
            });
        }
        return result;
    }
    public async Task<IEnumerable<TripCountryGetDTO>> GetTripsInfoAndCountries()
    {
        var result = new List<TripCountryGetDTO>();
        
        var connectionString = configuration.GetConnectionString("Default");
        
        await using var connection = new SqlConnection(connectionString);
        //potrzebne byly wszystkie informacje o panstwie więc dodaje też idPanstwa
        //laczenie join pomiedzy trip - trip_country - country, z aliasami tabel
        var sql = @"select t.IdTrip, t.Name, t.Description,t.DateFrom, t.DateTo,t.MaxPeople, coun.IdCountry, coun.Name 
                  from Trip t 
                      join Country_Trip ct on t.IdTrip = ct.IdTrip
                      join Country coun on ct.IdCountry=coun.IdCountry";
        
        await using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        
        await using var reader = await command.ExecuteReaderAsync();
        
        if (!await reader.ReadAsync())
        {
            throw new NotFoundException("Nie znaleziono zadnej wycieczki");
        }
        
        while (await reader.ReadAsync())
        {
            result.Add(new TripCountryGetDTO
            {
                IdTrip = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                DateFrom = reader.GetDateTime(3),
                DateTo = reader.GetDateTime(4),
                MaxPeople = reader.GetInt32(5),
                IdCountry = reader.GetInt32(6),
                Country = reader.GetString(7)
            });
        }
        return result;
    }
}