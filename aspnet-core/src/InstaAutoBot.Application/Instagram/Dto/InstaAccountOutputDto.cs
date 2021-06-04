using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace InstaAutoBot.Instagram.Dto
{
    public class InstaAccountOutputDto: EntityDto<long>
    {
        public string FirstName { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }

        public string ProfilePictureId { get; set; }

        public string FullName { get; set; }

        public bool IsVerified { get; set; }

        public bool IsPrivate { get; set; }

        public long Pk { get; set; }
    }
}
