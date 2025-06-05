using Cw_10_s30338.DTOs;
using Cw_10_s30338.Models;

public class GetTripDTO
{
       public int pageNum {get;set;}
       public int pageSize {get;set;}
       public int allPages {get;set;}
       public IEnumerable<TripDTO> trips {get;set;}
}