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
using Microsoft.EntityFrameworkCore;

namespace InstaAutoBot.HangfireJobs
{
    public class InstaMessagesBackgroundJob : BackgroundJob<InstaMessageInputDto>, ITransientDependency
    {
        private readonly IRepository<InstaAccount, long> _instaAccountRepository;
        private readonly IRepository<InstaSetting, long> _instaSettingRepository;
        private readonly IRepository<InstaMessageRecipient, long> _instaMessageRecipientRepository;
        private readonly InstagramManager _instagramManager;

        public InstaMessagesBackgroundJob(
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

            var targetedUserNames = await _instaMessageRecipientRepository
                .GetAll()
                .Where(x => x.InstaSettingId == instaSetting.Id)
                .Select(x => x.Recipient)
                .ToListAsync();

            var instaAccounts = await _instaAccountRepository
                .GetAll()
                .ToListAsync();


            //Send 1000 Messages
            var totalMessages = instaSetting.MessageNumbers;

            var allRecipientList = new List<string>();

            foreach (var instaAccount in instaAccounts)
            {
                var instaApi = await _instagramManager.GetInstaApi(new CurrentSession()
                {
                    UserName = instaAccount.UserName,
                    Password = instaAccount.Password
                });

                var currentAccountRecipientList = new List<string>();

                //1000/100 = 10
                var totalMessagePerTargetedUser = totalMessages / targetedUserNames.Count;

                foreach (var targetedUserName in targetedUserNames)
                {
                    var posts = await instaApi.UserProcessor.GetUserMediaAsync(targetedUserName,
                        PaginationParameters.MaxPagesToLoad(1));

                    foreach (var post in posts.Value)
                    {
                        var postId = post.Pk;
                        var postLikes = await instaApi.MediaProcessor.GetMediaLikersAsync(postId);
                        foreach (var postLike in postLikes.Value)
                        {
                            if (currentAccountRecipientList.Any(x => x == postLike.UserName) ||
                                allRecipientList.Any(x => x == postLike.UserName))
                                continue;//this user already exist try antoher like user

                            currentAccountRecipientList.Add(postLike.UserName);
                            allRecipientList.Add(postLike.UserName);

                            if (currentAccountRecipientList.Count == totalMessagePerTargetedUser)
                                goto exitAndStartSendingMessage; // it reach to required recipient list exit

                        }
                    }
                }

                exitAndStartSendingMessage:

                var directText = await instaApi.MessagingProcessor
                    .SendDirectTextAsync(
                        recipients: string.Join(",", currentAccountRecipientList),
                        threadIds: null,
                        text: instaSetting.MessageBody);
            }


        }

    }
}