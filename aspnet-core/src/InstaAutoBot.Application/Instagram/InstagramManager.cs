using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using InstaAutoBot.Instagram.Dto;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shortid;
using shortid.Configuration;

namespace InstaAutoBot.Instagram
{
    public class InstagramManager : InstaAutoBotDomainServiceBase
    {
        private readonly IRepository<InstaAccount, long> _instaAccountRepository;
        private readonly PhoneNumberManager _phoneNumberManager;
        private readonly ProxyManager _proxyManager;


        public InstagramManager(
            IRepository<InstaAccount, long> instaAccountRepository,
            PhoneNumberManager phoneNumberManager,
            ProxyManager proxyManager)
        {
            _instaAccountRepository = instaAccountRepository;
            _phoneNumberManager = phoneNumberManager;
            _proxyManager = proxyManager; 
        }

        public async Task SendInstaMessage(
            string textMessage,
            long instaAccountId,
            List<string> messageTags)
        {
            var recipients = string.Join(",", messageTags);
            var instaApi = await GetInstaApi(instaAccountId);
            var directText = await instaApi.MessagingProcessor
                .SendDirectTextAsync(recipients, null, textMessage);
        }

        public async Task<InstaAccount> CreateInstaAccount()
        {
            var userName = ShortId.Generate(new GenerationOptions
            {
                UseNumbers = false,
                UseSpecialCharacters = false,
                Length = 15
            });

            var password = ShortId.Generate(new GenerationOptions
            {
                UseNumbers = true,
                UseSpecialCharacters = true,
                Length = 15
            });

            return await CreateInstaAccount(
                userName: userName,
                password: password,
                firstName: userName);
        }

        public async Task<InstaAccount> CreateInstaAccount(
            string userName,
            string password,
            string firstName)
        {

            //return await CreateFakeInstaAccount(userName, password, firstName);
            var rentNewPhoneNumberResponse = await _phoneNumberManager.RentNewPhoneNumber();
            var phoneNumber = $"{rentNewPhoneNumberResponse.CountryCode}{rentNewPhoneNumberResponse.Number}".Replace("+", string.Empty);
            var phoneNumberId = rentNewPhoneNumberResponse.Id;

            var webProxy = await _proxyManager.GetWebProxy();

            var delay = RequestDelay.FromSeconds(1, 2);

            // create new InstaApi instance using Builder
            var instaApi = InstaApiBuilder.CreateBuilder()
                 .UseLogger(new DebugLogger(InstagramApiSharp.Logger.LogLevel.All)) // use logger for requests and debug messages
                 .UseHttpClientHandler(webProxy)
                 .SetRequestDelay(delay)
                 .Build();

            var checkPhone = await instaApi.CheckPhoneNumberAsync(phoneNumber);
            var singUpSmsCode = await instaApi.SendSignUpSmsCodeAsync(phoneNumber);

            var verificationCode = await _phoneNumberManager.WaitForInstaVerificationCode(phoneNumberId);

            if (string.IsNullOrEmpty(verificationCode))
            {
                return null;
            }

            var verify = await instaApi.VerifySignUpSmsCodeAsync(phoneNumber, verificationCode);
            var create = await instaApi.ValidateNewAccountWithPhoneNumberAsync(phoneNumber, verificationCode, userName, password, firstName);

            var result = new InstaAccount()
            {
                FirstName = firstName,
                Password = password,
                PhoneNumber = phoneNumber
            };

            if (create.Value?.CreatedUser != null)
            {
                var createdUser = create.Value?.CreatedUser;

                result.FullName = createdUser.FullName;
                result.IsPrivate = createdUser.IsPrivate;
                result.IsVerified = createdUser.IsVerified;
                result.Pk = createdUser.Pk;
                result.ProfilePicture = createdUser.ProfilePicture;
                result.ProfilePictureId = createdUser.ProfilePictureId;
                result.UserName = createdUser.UserName;
            }

            var instaAccountCreated = await _instaAccountRepository.InsertAsync(result);
            return instaAccountCreated;
        }

        public async Task<InstaAccount> CreateFakeInstaAccount(string userName,
            string password,
            string firstName)
        {

            var result = new InstaAccount
            {
                FirstName = firstName,
                Password = password,
                PhoneNumber = userName,
                FullName = firstName,
                UserName = userName
            };



            var instaAccountCreated = await _instaAccountRepository.InsertAsync(result);
            return instaAccountCreated;
        }

        private async Task PostStory()
        {

        }

        private async Task<IInstaApi> GetInstaApi(long instaAccountId)
        {
            var instaAccount = await _instaAccountRepository.GetAll()
                .Where(x => x.Id == instaAccountId)
                .Select(x => new CurrentSession
                {
                    Password = x.Password,
                    UserName = x.UserName
                })
                .FirstOrDefaultAsync();

            if (instaAccount != null)
            {
                return await GetInstaApi(instaAccount);
            }

            return null;
        }



        public async Task<InstaStoryMedia> UploadInstaUserStory(string userName, string password)
        {
            var instaApi = await GetInstaApi(new CurrentSession()
            {
                UserName = userName,
                Password = password
            });

            var image = new InstaImage { Uri = @"c:\AutoBot\posts\acoount1\Naran.jpeg" };

            var result = await instaApi.StoryProcessor.UploadStoryPhotoAsync(image, "someawesomepicture");

            return result.Value;
        }  
        
        public async Task<InstaDirectInboxThreadList> SendMessageToUser(string userName, string password)
        {
            var instaApi = await GetInstaApi(new CurrentSession()
            {
                UserName = userName,
                Password = password
            });
            var user = await instaApi.UserProcessor.GetUserAsync("imrafay");
            var userId = user.Value.Pk.ToString();
            var currentAccountRecipientList = new List<string>()
            {
                userId
            };

            var result= await instaApi.MessagingProcessor
                .SendDirectTextAsync(
                    recipients: string.Join(",", currentAccountRecipientList),
                    threadIds: null,
                    text: "Hello");

            return result.Value;
        }

        public async Task<InstaMedia> UploadInstaUserPost(string userName,string password)
        {
            var instaApi = await GetInstaApi(new CurrentSession()
            {
                UserName = userName,
                Password = password
            });

            var mediaImage = new InstaImageUpload
            {
                // leave zero, if you don't know how height and width is it.
                Height = 0,
                Width = 0,
                Uri = @"c:\AutoBot\posts\acoount1\Naran.jpeg"
            };

            // Add user tag (tag people)
            mediaImage.UserTags.Add(new InstaUserTagUpload
            {
                //Username = "rmt4006",
                X = 0.5,
                Y = 0.5
            });

            var result = await instaApi.MediaProcessor.UploadPhotoAsync(mediaImage, "Naran");

            return result.Value;
        }
        
        
        public async Task<IInstaApi> GetInstaApi(CurrentSession currentSession)
        {
            // create user session data and provide login details
            var userSession = new UserSessionData
            {
                UserName = currentSession.UserName,
                Password = currentSession.Password
            };

            var webProxy = await _proxyManager.GetWebProxy();

            var delay = RequestDelay.FromSeconds(2, 2);

            // create new InstaApi instance using Builder
            var IInstaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(InstagramApiSharp.Logger.LogLevel.All)) // use logger for requests and debug messages
                .UseHttpClientHandler(webProxy)
                .SetRequestDelay(delay)
                .Build();

            await IInstaApi.LoginAsync();

            return IInstaApi;
        }

        public async Task ProxyTest()
        {
            var webProxy = await _proxyManager.GetWebProxy();
            var delay = RequestDelay.FromSeconds(2, 2);

            var instaApi = InstaApiBuilder.CreateBuilder()
                .UseLogger(new DebugLogger(InstagramApiSharp.Logger.LogLevel.All)) // use logger for requests and debug messages
                .UseHttpClientHandler(webProxy)
                .SetRequestDelay(delay)
                .Build();

            var checkPhone = await instaApi.CheckPhoneNumberAsync("923219173647");
        }
    }


}
