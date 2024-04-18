using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WS.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WarningSentenceContext>
{
    public WarningSentenceContext CreateDbContext(string[] args)
    {
        //TODO: Move connection string to appsettings.json / configuration file
        const string connectionString = "Server=localhost;Database=KemiDB;User Id=sa;Password=thisIsSuperStrong1234;TrustServerCertificate=True";
        
        var optionsBuilder = new DbContextOptionsBuilder<WarningSentenceContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new WarningSentenceContext(optionsBuilder.Options);
    }
}