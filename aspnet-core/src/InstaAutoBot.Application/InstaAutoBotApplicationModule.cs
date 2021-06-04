using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InstaAutoBot.Authorization;

namespace InstaAutoBot
{
    [DependsOn(
        typeof(InstaAutoBotCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class InstaAutoBotApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<InstaAutoBotAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(InstaAutoBotApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
