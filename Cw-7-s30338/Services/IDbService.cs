using Cw_7_s30338.Exceptions;
using Cw_7_s30338.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace Cw_7_s30338.Services;

public interface IDbService
{
    public Task<IEnumerable<TripCountryGetDTO>> GetTripsInfoAndCountries();//zadanie 1
    public Task<IEnumerable<ClientsTripGetDTO>> GetClientsTrips(int id);//zadanie 2
    public Task<ClientGetDTO> GetClientById(int id);//czesc do zadania 2
    public Task<ClientGetDTO> CreateClient(ClientCreateDTO client);//zadanie 3
    public Task<Client_TripPutGetDTO> putClientTrip(int id, int tripId);//zadanie 4
    public Task DeleteClientTrip(int id, int tripId);//zadanie 5
}

public class DbService(IConfiguration configuration): IDbService
{
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

        do//do while bo await reader.readerAsync() sciaga pierwszy rekord, prez while bylby nieprzetworzony
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
        } while (await reader.ReadAsync());
        return result;
    }
    public async Task<IEnumerable<ClientsTripGetDTO>> GetClientsTrips(int id)
    {
        var result = new List<ClientsTripGetDTO>();
        
        var connectionString = configuration.GetConnectionString("Default");
        await using var connection = new SqlConnection(connectionString);
        
        //----- sprawdzenie istnienia klienta -------
        var sqlklient = @"select 1 from client where IdClient = @id";//jesli jakikolwiek klient istnieje bedzie 1 -> nie to null
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

        do //dla nie stracenia pierwszego rekordu w linijce 90 podczas sprawdzania
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
    public async Task<ClientGetDTO> GetClientById(int id)
    {
        var connectionString = configuration.GetConnectionString("Default");
        
        await using var connection = new SqlConnection(connectionString);
        var sql = "select IdClient,FirstName,LastName,Email,Telephone,Pesel from Client where Id = @id";
        //sciagniecei rekordu o podanym id
        
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        
        await connection.OpenAsync();
        
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new NotFoundException($"Nie znaleziono klienta o id {id}");
        }
        
        return new ClientGetDTO
        {
            Id = reader.GetInt32(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            Email = reader.GetString(3),
            Telephone = reader.GetString(4),
            Pesel = reader.GetString(5),
        };
    }
    public async Task<ClientGetDTO> CreateClient(ClientCreateDTO client)
    {
        // ------------- dodatkowa walidacja danych ---------------
        if (string.IsNullOrEmpty(client.FirstName) || string.IsNullOrEmpty(client.LastName) || 
            string.IsNullOrEmpty(client.Email) || string.IsNullOrEmpty(client.Telephone) || 
            string.IsNullOrEmpty(client.Pesel))
        {
            throw new BadRequest("Wszystkie dane muszą być wypełnione, podaj poprawne dane.");
        }

        var connectionString = configuration.GetConnectionString("Default");
        
        await using var connection = new SqlConnection(connectionString);
        var sql = @"Insert into Client (FirstName, LastName, Email, Telephone, Pesel) 
                    values (@FirstName, @LastName, @Email, @Telephone, @Pesel)
                    select scope_identity()"; // scope_identity() zwraca id ostatnio wstawionego wiersza
        
        await using var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@FirstName", client.FirstName);
        command.Parameters.AddWithValue("@LastName", client.LastName);
        command.Parameters.AddWithValue("@Email", client.Email);
        command.Parameters.AddWithValue("@Telephone", client.Telephone);
        command.Parameters.AddWithValue("@Pesel", client.Pesel);
        
        await connection.OpenAsync();
        
        var newId = Convert.ToInt32(await command.ExecuteScalarAsync()); //sciagniecie maksymalnego indexu
        return new ClientGetDTO
        {
            Id = newId,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            Telephone = client.Telephone,
            Pesel = client.Pesel
        };
    }

    public async Task<Client_TripPutGetDTO> putClientTrip(int clientId, int tripId)
    {
        var connectionString = configuration.GetConnectionString("Default");
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // --------- sprawdzenie istnienia klient ---------
        var sqlKlient = @"SELECT 1 FROM Client WHERE IdClient = @clientid";
        var checkClient = new SqlCommand(sqlKlient, connection);
        checkClient.Parameters.AddWithValue("@clientid", clientId);
        if (await checkClient.ExecuteScalarAsync() is null)
        {
            throw new KeyNotFoundException($"Client with ID {clientId} does not exist.");
        }

        // --------- sprawdzenie istnienia wycieczki ---------
        var sqltrip = @"SELECT MaxPeople FROM Trip WHERE IdTrip = @tripId";
        var checkTrip = new SqlCommand(sqltrip, connection);
        checkTrip.Parameters.AddWithValue("@tripId", tripId);
        var maxPeopleObj = await checkTrip.ExecuteScalarAsync();
        if (maxPeopleObj is null)
        {
            throw new KeyNotFoundException($"Trip with ID {tripId} does not exist.");
        }

        // ------- sprawdzenie czy klient sie zmiesci ---------
        var maxPeople = Convert.ToInt32(maxPeopleObj);

        // -- ilosc zapisanych do tej wycieczki klientow --
        var count = new SqlCommand("SELECT COUNT(*) FROM Client_Trip WHERE IdTrip = @tripId", connection);
        count.Parameters.AddWithValue("@tripId", tripId);
        int currentCount = Convert.ToInt32(await count.ExecuteScalarAsync());
        if (currentCount >= maxPeople)
        {
            throw new InvalidOperationException("Wszystkie miejsca na ta wycieczke sa juz zajete.");
        }

        const string checkSql = @"SELECT 1 FROM Client_Trip WHERE IdClient = @IdClient AND IdTrip = @IdTrip";

        await using (var checkCmd = new SqlCommand(checkSql, connection))
        {
            checkCmd.Parameters.AddWithValue("@IdClient", clientId);
            checkCmd.Parameters.AddWithValue("@IdTrip", tripId);

            var exists = await checkCmd.ExecuteScalarAsync();
            if (exists != null)
                throw new InvalidOperationException("Klient jest już zapisany na tę wycieczkę.");
        }

        // ---------- dopisanie klienta do wycieczki --------
        var insertSql = @"INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt) VALUES (@id, @tripId, @date)";
        var insert = new SqlCommand(insertSql, connection);
        insert.Parameters.AddWithValue("@id", clientId);
        insert.Parameters.AddWithValue("@tripId", tripId);
        insert.Parameters.AddWithValue("@date", int.Parse(DateTime.Now.ToString("yyyyMMdd")));
        await insert.ExecuteNonQueryAsync();

        return new Client_TripPutGetDTO();//zwraca pusty, bo i tak kod odpowiedzi to 204 No Content
    }
    
    public async Task DeleteClientTrip(int id, int tripId)
    {
        var result = new List<ClientsTripGetDTO>();
        
        var connectionString = configuration.GetConnectionString("Default");
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        
        //----- sprawdzenie istnienia klienta -------
        var sqlklient = @"select 1 from client where IdClient = @id";
        await using (var commandKlient = new SqlCommand(sqlklient, connection))
        {
            commandKlient.Parameters.Add(new SqlParameter("@id", id));
            await using var readerKlient = await commandKlient.ExecuteReaderAsync();
            if (!await readerKlient.ReadAsync())
            {
                throw new NotFoundException($"klient o id {id} nie istnieje");
            }
        }
        
        //----- sprawdzenie istnienia wycieczki -------
        var sqltrip = @"select 1 from trip where IdTrip = @tripId";
        await using (var commandtrip = new SqlCommand(sqltrip, connection))
        {
            commandtrip.Parameters.Add(new SqlParameter("@tripid", tripId));
            await using var readertrip = await commandtrip.ExecuteReaderAsync();
            if (!await readertrip.ReadAsync())
            {
                throw new NotFoundException($"wycieczka o id {id} nie istnieje");
            }
        }
        
        // -------- usuniecie powiazan klient-wycieczka -------
        var sqlDelete = "Delete from Client_Trip where IdTrip = @tripId and idClient = @id";
        await using (var commandDelete = new SqlCommand(sqlDelete, connection))
        {
            commandDelete.Parameters.Add(new SqlParameter("@id", id));
            commandDelete.Parameters.Add(new SqlParameter("@tripId", tripId));
            await commandDelete.ExecuteNonQueryAsync();
        }
    }
}