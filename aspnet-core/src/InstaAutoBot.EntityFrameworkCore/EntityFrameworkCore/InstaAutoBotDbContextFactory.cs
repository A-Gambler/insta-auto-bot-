using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using InstaAutoBot.Configuration;
using InstaAutoBot.Web;

namespace InstaAutoBot.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class InstaAutoBotDbContextFactory : IDesignTimeDbContextFactory<InstaAutoBotDbContext>
    {
        public InstaAutoBotDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<InstaAutoBotDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            InstaAutoBotDbContextConfigurer.Configure(builder, configuration.GetConnectionString(InstaAutoBotConsts.ConnectionStringName));

            return new InstaAutoBotDbContext(builder.Options);
        }
    }
}
