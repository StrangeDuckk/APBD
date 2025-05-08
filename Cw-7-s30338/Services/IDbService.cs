using Cw_7_s30338.Exceptions;
using Cw_7_s30338.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace Cw_7_s30338.Services;

public interface IDbService
{
    public Task<IEnumerable<TripGetDTO>> GetTripsInfo();
    public Task<IEnumerable<TripCountryGetDTO>> GetTripsInfoAndCountries();
    public Task<IEnumerable<ClientsTripGetDTO>> GetClientsTrips(int id);
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

    public async Task<IEnumerable<ClientsTripGetDTO>> GetClientsTrips(int id)
    {
        var result = new List<ClientsTripGetDTO>();
        
        var connectionString = configuration.GetConnectionString("Default");
        await using var connection = new SqlConnection(connectionString);
        
        //----- sprawdzenie istnienia klienta -------
        var sqlklient = @"select 1 from client where IdClient = @id";
        await using (var commandKlient = new SqlCommand(sqlklient, connection))
        {
            commandKlient.Parameters.Add(new SqlParameter("@id", id));
            await connection.OpenAsync();
            await using var readerKlient = await commandKlient.ExecuteReaderAsync();
            if (!await readerKlient.ReadAsync())
            {
                throw new NotFoundException($"klient o id {id} nie istnieje");
            }
        }


        // ------------ szukanie wycieczek dla isteniejacego klietna -----------
        var sql = @"Select t.IdTrip, t.Name, t.Description,t.DateFrom, t.DateTo,t.MaxPeople, ct.RegisteredAt, ct.PaymentDate
                    from Trip t join Client_Trip ct on t.IdTrip=ct.IdTrip
                    where ct.IdClient = @id"; 
        // polaczenie tabel trip i client_trip na podstawie idClient w tabeli Client_Trip
        
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        
        await using var reader = await command.ExecuteReaderAsync();
        
        if (!await reader.ReadAsync())
        {
            throw new NotFoundException($"klient o id {id} nie jedzie na zadna wycieczke");
        }

        do //dla nie stracenia pierwszego rekordu w linijce 122 podczas sprawdzania
        {
            result.Add(new ClientsTripGetDTO
            {
                IdTrip = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                DateFrom = reader.GetDateTime(3),
                DateTo = reader.GetDateTime(4),
                MaxPeople = reader.GetInt32(5),
                RegisteredAt = reader.GetInt32(6),
                PaymentDate = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7)
            });
        } while (await reader.ReadAsync());
        return result;
    }
}