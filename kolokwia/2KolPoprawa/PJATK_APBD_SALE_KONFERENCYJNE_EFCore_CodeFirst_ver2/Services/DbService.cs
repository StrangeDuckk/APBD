using Microsoft.EntityFrameworkCore;
using PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Data;
using PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.DTOs;
using PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Models;

namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Services;

public interface IDbService
{
    Task<GetEmployeeByIdDto?> GetEmployeeByIdAsync(int id);
    Task<GetEmployeeWithAccesibleRoomsDto?> PutEmployeeWithAccessAsync(int id, PutEmployeeWithAccessDto employee);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<GetEmployeeByIdDto?> GetEmployeeByIdAsync(int id)
    {
        // --------- sprawdzenie czy pracownik istnieje ---------- 
        var employee = await data.Employees
            .Include(e => e.RoomAccesses)//dla usuniecia pustych pokojow
            .ThenInclude(r => r.Room)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

        if (employee == null)
            return null;
        
        // -------- znalezienie dostepnych pokojow dla pracownika ---------
        var rooms = employee.RoomAccesses.Select(r => new GetRoomAccessDto()
        {
            Id = r.Room.RoomId,
            Name = r.Room.Name,
            Capacity = r.Room.Capacity,
        }).ToList();
        // foreach (var room in employee.RoomAccesses)
        // {
        //     rooms.Add(new GetRoomAccessDto
        //     {
        //         Id = room.Room.RoomId,
        //         Name = room.Room.Name,
        //         Capacity = room.Room.Capacity,
        //     });
        // }

        return new GetEmployeeByIdDto()
        {
            Id = employee.EmployeeId,
            Name = employee.Name,
            Email = employee.Email,
            AccesibleRooms = rooms
        };
    }

    public async Task<GetEmployeeWithAccesibleRoomsDto?> PutEmployeeWithAccessAsync(int id, PutEmployeeWithAccessDto body)
    {
        var transaction = await data.Database.BeginTransactionAsync();
        try
        {
            // ---------- sprawdzenie czy employee istnieje ------------
            var employee = await data.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null)
                return null;
            // -------- aktualizacja danych pracownika -----------
            employee.Name = body.Name;
            employee.Email = body.Email;
            employee.RoomAccesses.Clear();
            await data.SaveChangesAsync();
            
            // ------- aktualizacja danych dla room access -> usuniecie starych dostepow ---
            var oldAccesses = await data.RoomAccesses.Where(e => e.EmployeeId == id).ToListAsync();
            data.RoomAccesses.RemoveRange(oldAccesses);
            await data.SaveChangesAsync();
            
            
            var rooms = new List<GetRoomAccessDto>();
            foreach (var roomId in body.AccessibleRoomsIds)
            {
                // --------- sprawdzanie czy pokoj istnieje ----------
                var room = await data.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
                if (room == null)
                    continue;
                
                // ------ jesli istnieje przypisanie go do pracownika --------
                var r = new RoomAccess()
                {
                    RoomId = roomId,
                    Room = room,
                    EmployeeId = employee.EmployeeId,
                    Employee = employee,
                };
                await data.RoomAccesses.AddAsync(r);
                
                rooms.Add(new GetRoomAccessDto()
                {
                    Id = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                });
            }
            await data.SaveChangesAsync();

            
            await transaction.CommitAsync();
            
            return new GetEmployeeWithAccesibleRoomsDto()
            {
                Id = employee.EmployeeId,
                Name = employee.Name,
                Email = employee.Email,
                Rooms = rooms,
                Message = "Employee data and access list updated successfully"
            };
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}