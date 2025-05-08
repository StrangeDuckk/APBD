using Cw_7_s30338.Models.DTOs;

namespace Cw_7_s30338.Services;

public interface IDbService
{
    public Task<IEnumerable<ClientGetDTO>> SlupTask();
}

public class DbService(IConfiguration configuration): IDbService
{
    public Task<IEnumerable<ClientGetDTO>> SlupTask()
    {
        //todo wykorzystac i pamietac
        throw new NotImplementedException(); //słup zeby o tym pamietac na poczatku pisania
    }
}