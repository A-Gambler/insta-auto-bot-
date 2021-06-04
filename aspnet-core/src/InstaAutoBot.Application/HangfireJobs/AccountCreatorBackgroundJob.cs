using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using Hangfire;
using InstaAutoBot.Instagram;

namespace InstaAutoBot.HangfireJobs
{
    public class AccountCreatorBackgroundJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly InstagramManager _instagramManager;

        public AccountCreatorBackgroundJob(InstagramManager instagramManager)
        {
            _instagramManager = instagramManager;
        }

        [AutomaticRetry(Attempts = 0)]
        [UnitOfWork]
        public override void Execute(int maxLimit)
        {
            for (var i = 0; i < maxLimit; i++)
            {
                 AsyncHelper.RunSync(()=> _instagramManager.CreateInstaAccount());
            }
        }
    }
}