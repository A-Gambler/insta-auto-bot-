using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Castle.Core.Internal;
using InstaAutoBot.HangfireJobs;
using InstaAutoBot.Instagram;
using InstaAutoBot.Instagram.Dto;
using InstaAutoBot.Instagram.Storage.FileManagement;
using InstaAutoBot.Sessions.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstaAutoBot.Sessions
{
    public class InstagramAccountAppService : InstaAutoBotAppServiceBase, IInstagramAccountAppService
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IRepository<InstaAccount, long> _instaAccountRepository;
        private readonly IRepository<InstaTemplate, long> _instaTemplateRepository;
        private readonly IRepository<InstaTemplateTag, long> _instaTemplateTagRepository;
        private readonly IRepository<InstaMessage, long> _instaMessageRepository;
        private readonly IRepository<InstaMessageRecipient, long> _instaMessageRecipientRepository;
        private readonly IRepository<InstaSetting, long> _instaSettingRepository;
        private readonly InstagramManager _instagramManager;

        public InstagramAccountAppService(
            IBackgroundJobManager backgroundJobManager,
            IRepository<InstaAccount, long> instaAccountRepository,
            IRepository<InstaTemplate, long> instaTemplateRepository,
            IRepository<InstaTemplateTag, long> instaTemplateTagRepository,
            IRepository<InstaMessage, long> instaMessageRepository,
            IRepository<InstaMessageRecipient, long> instaMessageRecipientRepository,
            IRepository<InstaSetting, long> instaSettingRepository,
            InstagramManager instagramManager)
        {
            _backgroundJobManager = backgroundJobManager;
            _instaAccountRepository = instaAccountRepository;
            _instaTemplateRepository = instaTemplateRepository;
            _instaTemplateTagRepository = instaTemplateTagRepository;
            _instaMessageRepository = instaMessageRepository;
            _instaMessageRecipientRepository = instaMessageRecipientRepository;
            _instaSettingRepository = instaSettingRepository;
            _instagramManager = instagramManager;
        }

        public async Task CreateInstagramBulkAccounts(int limit)
        {
            if (limit > 10)
            {
                var maxThreads = limit / AppConsts.NoOfAccountCreatorThreads;
                for (int i = 0; i < AppConsts.NoOfAccountCreatorThreads; i++)
                {
                    await _backgroundJobManager.EnqueueAsync<AccountCreatorBackgroundJob, int>(maxThreads);
                }
            }
            else
            {
                await _backgroundJobManager.EnqueueAsync<AccountCreatorBackgroundJob, int>(limit);
            }
        }

        public async Task<PagedResultDto<InstaAccountOutputDto>> GetInstaAccounts(InstaAccountInputDto input)
        {
            var query = _instaAccountRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrEmpty(), x => x.UserName.ToLower().Contains(input.Keyword.ToLower()))
                .WhereIf(!input.Keyword.IsNullOrEmpty(), x => x.FullName.ToLower().Contains(input.Keyword.ToLower()))
                .WhereIf(!input.Keyword.IsNullOrEmpty(), x => x.PhoneNumber.ToLower().Contains(input.Keyword.ToLower()))
                .OrderByDescending(x => x.CreationTime)
                .AsQueryable();

            var totalCount = query.Count();

            if (input.Page > 0)
            {
                query = query.Page(input.Page, input.PageSize);
            }

            var instaAccounts = await query.Select(x => new InstaAccountOutputDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                FullName = x.FullName,
                IsPrivate = x.IsPrivate,
                IsVerified = x.IsVerified,
                PhoneNumber = x.PhoneNumber,
                Pk = x.Pk,
                ProfilePicture = x.ProfilePicture,
                ProfilePictureId = x.ProfilePictureId,
                UserName = x.UserName
            }).ToListAsync();

            return new PagedResultDto<InstaAccountOutputDto>()
            {
                Items = instaAccounts,
                TotalCount = totalCount
            };
        }

        public async Task TestInstaDataFileType(InstaDataFileType input)
        {

        }
        
        public async Task<InstaSettingUpdateInputDto> GetInstaSetting()
        {
            var instaSetting = await _instaSettingRepository
                .GetAll()
                .Select(x => new InstaSettingUpdateInputDto
                {
                    Id = x.Id,
                    PostInterval = x.PostInterval,
                    PostIntervalValue = x.PostIntervalValue,
                    StoryInterval = x.StoryInterval,
                    StoryIntervalValue = x.StoryIntervalValue,
                    MessageIntervalValue = x.MessageIntervalValue,
                    MessageInterval = x.MessageInterval,
                    MessageBody = x.MessageBody,
                    MessageNumbers = x.MessageNumbers,
                    PostNumbers = x.PostNumbers,
                    StoryNumbers = x.StoryNumbers
                }).FirstOrDefaultAsync();

            if (instaSetting == null)
            {
                return new InstaSettingUpdateInputDto
                {
                    PostInterval = TimeInterval.PerDay,
                    PostIntervalValue =1,
                    StoryInterval = TimeInterval.PerDay,
                    StoryIntervalValue = 1,
                    MessageIntervalValue =1,
                    MessageInterval = TimeInterval.PerDay,
                    StoryNumbers = 10,
                    PostNumbers = 10,
                    MessageNumbers = 1000,
                };
            }

            instaSetting.RecipientTags = await _instaMessageRecipientRepository
                .GetAll()
                .Where(x => x.InstaSettingId == instaSetting.Id)
                .Select(x => x.Recipient)
                .ToListAsync(); 

            return instaSetting;
        }

        public async Task CreateOrUpdateInstaSettings(InstaSettingUpdateInputDto input)
        {
            var instaSetting = await _instaSettingRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (instaSetting != null)
            {
                instaSetting.MessageBody = input.MessageBody;
                instaSetting.MessageInterval = input.MessageInterval;
                instaSetting.MessageIntervalValue = input.MessageIntervalValue;

                instaSetting.PostIntervalValue = input.PostIntervalValue;
                instaSetting.PostIntervalValue = input.PostIntervalValue;

                instaSetting.StoryInterval = input.StoryInterval;
                instaSetting.StoryIntervalValue = input.StoryIntervalValue;

                instaSetting.StoryNumbers = input.StoryNumbers;
                instaSetting.PostNumbers = input.PostNumbers;
                instaSetting.MessageNumbers = input.MessageNumbers;

                await _instaSettingRepository.UpdateAsync(instaSetting);
                await CurrentUnitOfWork.SaveChangesAsync();

                await CreateInstaMessageTags(instaSetting.Id, input.RecipientTags);
            }
            else
            {
                instaSetting = new InstaSetting
                {
                    MessageInterval = input.MessageInterval,
                    MessageIntervalValue = input.MessageIntervalValue,
                    PostIntervalValue = input.PostIntervalValue,
                    PostInterval = input.PostInterval,
                    StoryIntervalValue = input.StoryIntervalValue,
                    StoryInterval = input.StoryInterval,
                    PostNumbers = input.PostNumbers,
                    StoryNumbers = input.StoryNumbers,
                    MessageNumbers = input.MessageNumbers
                };

                await _instaSettingRepository.InsertAsync(instaSetting);
                await CurrentUnitOfWork.SaveChangesAsync();

                await CreateInstaMessageTags(instaSetting.Id, input.RecipientTags);
            }

        }

        public async Task CreateInstaTemplate(InstaTemplateInputDto input)
        {
            var instaTemplate = new InstaTemplate()
            {
                InstaAccountId = input.InstaAccountId,
                Name = input.Name,
                ZipFileName = input.ZipFileName,
                PostsIntervalInHours = input.PostsIntervalInHours,
                StoriesIntervalInHours = input.StoriesIntervalInHours
            };

            await _instaTemplateRepository.InsertAsync(instaTemplate);
            await CurrentUnitOfWork.SaveChangesAsync();

            await CreateInstaTemplateTags(instaTemplate.Id, input.TemplateTags);
        }

        public async Task UpdateInstaTemplate(InstaTemplateUpdateInputDto input)
        {
            var instaTemplate = await _instaTemplateRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (instaTemplate != null)
            {
                instaTemplate.Name = input.Name;
                instaTemplate.ZipFileName = input.ZipFileName;
                instaTemplate.PostsIntervalInHours = input.PostsIntervalInHours;
                instaTemplate.StoriesIntervalInHours = input.StoriesIntervalInHours;
                await _instaTemplateRepository.UpdateAsync(instaTemplate);
                await CurrentUnitOfWork.SaveChangesAsync();

                await DeleteInstaTemplateTags(input.Id);
                await CreateInstaTemplateTags(input.Id, input.TemplateTags);
            }
        }

        public async Task<InstaTemplateUpdateInputDto> GetInstaTemplateByInstaAccountId(long instaAccountId)
        {
            var instaTemplate = await _instaTemplateRepository
                .GetAll()
                .Where(x => x.InstaAccountId == instaAccountId)
                .Select(x => new InstaTemplateUpdateInputDto
                {
                    Id = x.Id,
                    InstaAccountId = x.InstaAccountId,
                    Name = x.Name,
                    PostsIntervalInHours = x.PostsIntervalInHours,
                    StoriesIntervalInHours = x.StoriesIntervalInHours,
                    ZipFileName = x.ZipFileName
                }).FirstOrDefaultAsync();

            if (instaTemplate == null)
            {
                return null;
            }

            instaTemplate.TemplateTags = await GetInstTemplateTags(instaTemplate.Id);
            return instaTemplate;
        }

        public async Task<InstaSummaryDto> GetInstaDashboardSummary()
        {
            var instaTemplateCount = await _instaMessageRepository.CountAsync();
            var instaAccountCount = await _instaAccountRepository.CountAsync();

            var output = new InstaSummaryDto()
            {
                TotalInstaAccounts = instaAccountCount,
                TotalMessages = instaTemplateCount
            };

            return output;
        }

        private async Task<List<string>> GetInstTemplateTags(long instaTemplateId)
        {
            var templateTags = await _instaTemplateTagRepository
                .GetAll()
                .Where(x => x.InstaTemplateId == instaTemplateId)
                .Select(x => x.TagUserName)
                .ToListAsync();

            return templateTags;
        }

        public async Task SendInstaMessage(InstaMessageInputDto input)
        {
            var instaMessage = new InstaMessage()
            {
                InstaAccountId = input.InstaAccountId,
                Message = input.TextMessage
            };

            await _instaMessageRepository.InsertAsync(instaMessage);
            await CurrentUnitOfWork.SaveChangesAsync();

            await CreateInstaMessageTags(instaMessage.Id, input.MessageTags);
        }

        private async Task CreateInstaMessageTags(long instaMessageId, List<string> inputMessageTags)
        {
            await _instaMessageRecipientRepository.HardDeleteAsync(x => x.InstaSettingId == instaMessageId);
            var messageTags = inputMessageTags.Select(x => new InstaMessageRecipient()
            {
                InstaSettingId = instaMessageId,
                Recipient = x
            }).ToList();

            await _instaMessageRecipientRepository.GetDbContext().AddRangeAsync(messageTags);
            await CurrentUnitOfWork.SaveChangesAsync();
        }


        private async Task CreateInstaTemplateTags(long instaTemplateId, List<string> intsaTemplateTags)
        {
            var templateTags = intsaTemplateTags.Select(x => new InstaTemplateTag()
            {
                InstaTemplateId = instaTemplateId,
                TagUserName = x
            }).ToList();

            await _instaTemplateTagRepository.GetDbContext().AddRangeAsync(templateTags);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteInstaTemplate(long id)
        {
            await _instaTemplateRepository.DeleteAsync(x => x.Id == id);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        private async Task DeleteInstaTemplateTags(long instaTemplateId)
        {
            await _instaTemplateTagRepository.HardDeleteAsync(x => x.InstaTemplateId == instaTemplateId);
            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
