using Cw_10_s30338.Data;
using Cw_10_s30338.DTOs;
using Cw_10_s30338.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw_10_s30338.Services;

public interface IDbService
{
    Task<GetTripDTO> GetAllTripsAsync(int page=1,int pageSize=10);
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
            
        // var allTrips = await data.Trips.CountAsync();
        // var allPages =(int)Math.Ceiling(allTrips / (double) pageSize);
        //
        // var trips = await data.Trips
        //     .Include(t => t.ClientTrips)
        //     .ThenInclude(ct => ct.IdClientNavigation)
        //     .OrderByDescending(t => t.DateFrom)
        //     .Skip((page - 1) * pageSize)
        //     .Take(pageSize)
        //     .Select(t => new TripDTO
        //     {
        //         Name = t.Name,
        //         Description = t.Description,
        //         DateFrom = t.DateFrom,
        //         DateTo = t.DateTo,
        //         MaxPeople = t.MaxPeople,
        //         Countries = t.CountryTrips
        //             .Select(ct => new CountryDTO{ Name = ct.IdCountryNavigation.Name}).ToList(),
        //         Clients = t.ClientTrips
        //             .Select(ct => new ClientDTO()
        //             {
        //                 FirstName = ct.IdClientNavigation.FirstName,
        //                 LastName = ct.IdClientNavigation.LastName,
        //             }).ToList(),
        //     })
        //     .ToListAsync();
        //
        // return new GetTripDTO
        // {
        //     pageNum = page,
        //     pageSize = pageSize,
        //     allPages = allPages,
        //     trips = trips
        // };
    }
}