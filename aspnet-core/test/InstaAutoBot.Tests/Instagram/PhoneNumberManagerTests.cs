using Xunit;
using InstaAutoBot.Instagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaAutoBot.Tests;

namespace InstaAutoBot.Instagram.Tests
{
    public class PhoneNumberManagerTests : InstaAutoBotTestBase
    {
        private readonly PhoneNumberManager _sut;

        public PhoneNumberManagerTests()
        {
            _sut = Resolve<PhoneNumberManager>();
        }

        [Fact()]
        public async Task RentNewPhoneNumberTest()
        {
            var rentNewPhoneNumberResponse = await _sut.RentNewPhoneNumber();
            var getSmsResponse = await _sut.GetSms(rentNewPhoneNumberResponse.Id);
        }
    }
}