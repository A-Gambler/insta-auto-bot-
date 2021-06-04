using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace InstaAutoBot.EntityFrameworkCore
{
    public static class InstaAutoBotDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<InstaAutoBotDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<InstaAutoBotDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
