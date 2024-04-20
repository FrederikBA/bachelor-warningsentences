using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Shared.Integration.Configuration;

namespace WS.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WarningSentenceContext>
{
    public WarningSentenceContext CreateDbContext(string[] args)
    {
        const string connectionString = Config.ConnectionStrings.ShwWarningSentences; 
        
        var optionsBuilder = new DbContextOptionsBuilder<WarningSentenceContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new WarningSentenceContext(optionsBuilder.Options);
    }
}