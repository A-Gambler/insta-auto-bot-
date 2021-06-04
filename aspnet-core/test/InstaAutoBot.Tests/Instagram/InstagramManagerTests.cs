using Xunit;
using InstaAutoBot.Instagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InstaAutoBot.Tests;
using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;

namespace InstaAutoBot.Instagram.Tests
{
    public class InstagramManagerTests : InstaAutoBotTestBase
    {
        private readonly InstagramManager _sut;

        public InstagramManagerTests()
        {
            _sut = Resolve<InstagramManager>();
        }

        [Fact()]
        public async Task CreateInstaAccountTest2()
        {
            await _sut.CreateInstaAccount();
        }

        [Fact()]
        public async Task UploadInstaUserPostTest()
        {
           var result =  await _sut.UploadInstaUserPost("iavvkyrnhmiwisk", "laI_kKLhUhQIuby");
        }
        
        [Fact()]
        public async Task UploadInstaUserStoryTest()
        {
           var result =  await _sut.UploadInstaUserStory("iavvkyrnhmiwisk", "laI_kKLhUhQIuby");
        }
        
        [Fact()]
        public async Task SendMessageToUserTest()
        {
           var result =  await _sut.SendMessageToUser("iavvkyrnhmiwisk", "laI_kKLhUhQIuby");
        }

        [Fact()]
        public async Task CreateInstaAccountTest()
        {
            var InstaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(new UserSessionData()
                {
                    UserName = "iavvkyrnhmiwisk",
                    Password = "laI_kKLhUhQIuby"
                })
                .UseLogger(new DebugLogger(InstagramApiSharp.Logger.LogLevel.All)) // use logger for requests and debug messages
                .Build();
            await InstaApi.LoginAsync();
            //var currentUser = await InstaApi.GetCurrentUserAsync();


            //2469060275564135102

            var posts = await InstaApi.UserProcessor.GetUserMediaAsync("elonmusk",
                PaginationParameters.MaxPagesToLoad(5));

            var postId = posts.Value.FirstOrDefault().Pk;
            var postLikes = await InstaApi.MediaProcessor.GetMediaLikersAsync(postId);

            //var followers = await InstaApi.UserProcessor.GetUserFollowersAsync("elonmusk",
            //    PaginationParameters.MaxPagesToLoad(5));



        }


        [Fact()]
        public async Task ProxyTestTest()
        {
            await _sut.ProxyTest();
        }
    }
}