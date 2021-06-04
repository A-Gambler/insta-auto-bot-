using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InstaAutoBot.Configuration;
using InstaAutoBot.EntityFrameworkCore;
using InstaAutoBot.Migrator.DependencyInjection;

namespace InstaAutoBot.Migrator
{
    [DependsOn(typeof(InstaAutoBotEntityFrameworkModule))]
    public class InstaAutoBotMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public InstaAutoBotMigratorModule(InstaAutoBotEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(InstaAutoBotMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                InstaAutoBotConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InstaAutoBotMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
