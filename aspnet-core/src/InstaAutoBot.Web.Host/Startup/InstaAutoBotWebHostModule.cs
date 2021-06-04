using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Hangfire;
using InstaAutoBot.Configuration;
using InstaAutoBot.HangfireJobs;

namespace InstaAutoBot.Web.Host.Startup
{
    [DependsOn(
       typeof(InstaAutoBotWebCoreModule))]
    public class InstaAutoBotWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public InstaAutoBotWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InstaAutoBotWebHostModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            ConfigureScheduledJobs();
        }

        private void ConfigureScheduledJobs()
        {
            RecurringJob.AddOrUpdate<AccountCreatorJobManager>(x => x.Execute(), Cron.Hourly);
        }
    }
}
