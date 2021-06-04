using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Hangfire;
using InstaAutoBot.Instagram;
using InstaAutoBot.Instagram.Dto;
using InstagramApiSharp;
using InstagramApiSharp.Classes.Models;
using Microsoft.EntityFrameworkCore;

namespace InstaAutoBot.HangfireJobs
{
    public class InstaPostsBackgroundJob : BackgroundJob<InstaMessageInputDto>, ITransientDependency
    {
        private readonly IRepository<InstaAccount, long> _instaAccountRepository;
        private readonly IRepository<InstaSetting, long> _instaSettingRepository;
        private readonly IRepository<InstaMessageRecipient, long> _instaMessageRecipientRepository;
        private readonly InstagramManager _instagramManager;

        public InstaPostsBackgroundJob(
            IRepository<InstaAccount, long> instaAccountRepository,
            IRepository<InstaSetting, long> instaSettingRepository,
            IRepository<InstaMessageRecipient, long> instaMessageRecipientRepository,
            InstagramManager instagramManager)
        {
            _instaAccountRepository = instaAccountRepository;
            _instaSettingRepository = instaSettingRepository;
            _instaMessageRecipientRepository = instaMessageRecipientRepository;
            _instagramManager = instagramManager;
        }

        [AutomaticRetry(Attempts = 0)]
        [UnitOfWork]
        public override void Execute(InstaMessageInputDto args)
        {
            AsyncHelper.RunSync(() => Main(args));
        }

        private async Task Main(InstaMessageInputDto args)
        {

            var instaSetting = await _instaSettingRepository
                .GetAll()
                .FirstOrDefaultAsync();

            var instaAccounts = await _instaAccountRepository
                .GetAll()
                .ToListAsync();

            //Send 1000 Messages
            var totalMessages = instaSetting.MessageNumbers;

            var allRecipientList = new List<string>();

            foreach (var instaAccount in instaAccounts)
            {
                

            }


        }

    }
}