using System.Threading.Tasks;
using InstaAutoBot.Models.TokenAuth;
using InstaAutoBot.Web.Controllers;
using Shouldly;
using Xunit;

namespace InstaAutoBot.Web.Tests.Controllers
{
    public class HomeController_Tests: InstaAutoBotWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}