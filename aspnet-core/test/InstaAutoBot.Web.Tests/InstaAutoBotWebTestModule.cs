using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InstaAutoBot.EntityFrameworkCore;
using InstaAutoBot.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace InstaAutoBot.Web.Tests
{
    [DependsOn(
        typeof(InstaAutoBotWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class InstaAutoBotWebTestModule : AbpModule
    {
        public InstaAutoBotWebTestModule(InstaAutoBotEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InstaAutoBotWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(InstaAutoBotWebMvcModule).Assembly);
        }
    }
}