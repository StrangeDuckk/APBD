using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cw_9_s30338.Data;

public class AppDbContextFactory :IDesignTimeDbContextFactory<AppDbContext>
{
    /*
     * dla wyeliminowania bledu:
     * Unable to create a 'DbContext' of type 'RuntimeType'. The exception 'Unable to resolve service for type
     * 'Microsoft.EntityFrameworkCore.DbContextOptions`1[Cw_9_s30338.Data.AppDbContext]' while attempting to activate
     * 'Cw_9_s30338.Data.AppDbContext'. was thrown while attempting to create an instance.
     * For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728   
     */

    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=cw_9_s30338;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        return new AppDbContext(optionsBuilder.Options);
    }
}