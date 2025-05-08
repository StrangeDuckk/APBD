using Cw_7_s30338.Exceptions;
using Cw_7_s30338.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace Cw_7_s30338.Services;

public interface IDbService
{
    public Task<IEnumerable<ZwierzakiGetDTO>> GetAllZwierzakiAsync();
    public Task<ZwierzakiGetDTO> GetZwierzakiByIdAsync(int id);
    public Task<ZwierzakiGetDTO> CreateZwierzakiAsync(ZwierzakiCreateDTO zwierzak);
}

public class DbService(IConfiguration configuration): IDbService
{
    public async Task<IEnumerable<ZwierzakiGetDTO>> GetAllZwierzakiAsync()
    {
        var result = new List<ZwierzakiGetDTO>();
        
        var connectionString = configuration.GetConnectionString("Default");
        await using var connection = new SqlConnection(connectionString);
        var sql = "Select Id_Zwierzaka, NazwaGatunku, Imie from Zwierzaki";
        
        await using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) //sciaganie po kolei kazdego elementu
        {
            result.Add(
                new ZwierzakiGetDTO()
                {
                    //UWAZAC PRZY DECIMALACH
                    Id_Zwierzaka = reader.GetInt32(0), //typ get musi sie zgadzac z bazą danych
                    NazwaGatunku = reader.GetString(1),
                    Imie = reader.GetString(2),
                }
            );
        }
        return result;
    }

    public async Task<ZwierzakiGetDTO> GetZwierzakiByIdAsync(int id)
    {
        var connectionString = configuration.GetConnectionString("Default");
        await using var connection = new SqlConnection(connectionString);
        var sql = "Select Id_Zwierzaka, NazwaGatunku, Imie from Zwierzaki where Id_Zwierzaka = @id";
        
        await using var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@id", id);
        
        await connection.OpenAsync();
        
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            throw new NotFoundException($"nie znaleziono rekordu o id: {id}");
        }
        
        return new ZwierzakiGetDTO()
        {
            Id_Zwierzaka = reader.GetInt32(0),
            NazwaGatunku = reader.GetString(1),
            Imie = reader.GetString(2)
        };
    }

    public async Task<ZwierzakiGetDTO> CreateZwierzakiAsync(ZwierzakiCreateDTO zwierzak)
    {
        var connectionString = configuration.GetConnectionString("Default");
        
        await using var connection = new SqlConnection(connectionString);
        var sql = "Insert into Zwierzaki (NazwaGatunku, Imie) values (@nazwaGatunku, @imie); select scope_identity();";
        
        await using var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@NazwaGatunku", zwierzak.NazwaGatunku);
        command.Parameters.AddWithValue("@Imie", zwierzak.Imie);
        
        await connection.OpenAsync();
        
        var newId = Convert.ToInt32(await command.ExecuteScalarAsync());
        return new ZwierzakiGetDTO()
        {
            Id_Zwierzaka = newId,
            NazwaGatunku = zwierzak.Imie,
            Imie = zwierzak.Imie
        };
    }
}