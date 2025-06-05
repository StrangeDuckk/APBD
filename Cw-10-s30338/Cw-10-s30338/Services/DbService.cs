using Cw_10_s30338.Data;
using Cw_10_s30338.DTOs;
using Cw_10_s30338.Exceptions;
using Cw_10_s30338.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NotFound = Cw_10_s30338.Exceptions.NotFound;

namespace Cw_10_s30338.Services;

public interface IDbService
{
    Task<GetTripDTO> GetAllTripsAsync(int page=1,int pageSize=10);
    Task DeleteClientAsync(string idClient);
    Task<GetClientTripDTO> AddClietnTripAsync(CreateClientTripDTO clientTrip);
}

public class DbService(Cw10S30338DbContext data): IDbService
{
    public async Task<GetTripDTO> GetAllTripsAsync(int page = 1, int pageSize = 10)
    {
        var totalTrips = await data.Trips.CountAsync();
        var allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await data.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries
                    .Select(ct => new CountryDTO
                    {
                        Name = ct.Name,
                    }).ToList(),
                Clients = t.ClientTrips
                    .Select(ct => new ClientDTO
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    }).ToList()
            })
            .ToListAsync();
        
        
        return new GetTripDTO
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = allPages,
            trips = trips
        };
    }

    public async Task DeleteClientAsync(string idClient)
    {
        // ------ sprawdzenie czy klient istnieje --------
        var client = await data.Clients.FindAsync(idClient);
        if (client == null)
        {
            throw new NotFound($"client {idClient} not found");
        }
        
        // ---------- srpawdzenie czy klient nie ma wycieczek ----------
        var wycieczkiKlienta = await data.ClientTrips.AnyAsync(ct => ct.IdClient ==  client.IdClient);
        if (wycieczkiKlienta)
        {
            throw new DeletingClientWithTripsException($"Cannot delete client {idClient}, client has trips");
        }
        
        data.Clients.Remove(client);
        await data.SaveChangesAsync();
    }

    public async Task<GetClientTripDTO> AddClietnTripAsync(CreateClientTripDTO clientTrip)
    {
        // ----------------- istnienie klienta ------------------
        var clientExists = await data.Clients.FirstOrDefaultAsync(c => c.Pesel == clientTrip.Pesel);
        if (clientExists != null)
        {
            throw new AlreadyExistingClientException($"Client {clientTrip.Pesel} already exists");
        }
        
        // ---------------- wycieczka istnieje i sie nie zaczela ----------
        var tripExists = await data.Trips.FirstOrDefaultAsync(ct => ct.IdTrip == clientTrip.IdTrip);
        if (tripExists == null)
        {
            throw new NotFound($"trip {clientTrip.IdTrip} not found");
        }

        if (tripExists.DateFrom < DateTime.UtcNow)
        {
            throw new TripAlreadyStartedException($"trip {clientTrip.IdTrip} already started");
        }
        
        // ------------- dodanie klienta ----------------
        var newClient = new Client
        {
            FirstName = clientTrip.FirstName,
            LastName = clientTrip.LastName,
            Email = clientTrip.Email,
            Telephone = clientTrip.Telephone,
            Pesel = clientTrip.Pesel,
        };
        
        await data.Clients.AddAsync(newClient);
        await data.SaveChangesAsync();
        
        // -------------- sprawdzenie czy klient nie jest juz dopisany do wycieczki ---------
        var clientHasTrip = await data.ClientTrips.AnyAsync(ct => ct.IdClient == clientExists.IdClient && ct.IdTrip == clientTrip.IdTrip);
        if (clientHasTrip)
        {
            throw new AlreadyExistingClientException($"Client {clientTrip.Pesel} is already registered for trip {clientTrip.IdTrip}");
        }
        
        // ------------ dopisanie klienta do wycieczki -------------
        var newClientTrip = new ClientTrip
        {
            IdClient = newClient.IdClient,
            IdTrip = clientTrip.IdTrip,
            PaymentDate = clientTrip.PaymentDate,
            RegisteredAt = DateTime.Now,
        };
        
        await data.ClientTrips.AddAsync(newClientTrip);
        await data.SaveChangesAsync();

        return new GetClientTripDTO
        {
            client = new ClientDTO
            {
                FirstName = clientTrip.FirstName,
                LastName = clientTrip.LastName,
            },
            trip = new TripDTO
            {
                IdTrip = newClientTrip.IdTrip,
            }
        };
    }
}