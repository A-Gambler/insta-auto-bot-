using Abp.Dependency;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using InstaAutoBot.EntityFrameworkCore.Seed;

namespace InstaAutoBot.EntityFrameworkCore
{
    [DependsOn(
        typeof(InstaAutoBotCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class InstaAutoBotEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<InstaAutoBotDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        InstaAutoBotDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        InstaAutoBotDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InstaAutoBotEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            EnsureMigrated();

            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }


        private void EnsureMigrated()
        {
            using (var migrateExecuter = IocManager.ResolveAsDisposable<MultiTenantMigrateExecuter>())
            {
                migrateExecuter.Object.Run();
            }
        }
    }
}
