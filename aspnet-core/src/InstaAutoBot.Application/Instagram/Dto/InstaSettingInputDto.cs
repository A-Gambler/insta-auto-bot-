using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using InstaAutoBot.Users.Dto;

namespace InstaAutoBot.Instagram.Dto
{
    public class InstaSettingUpdateInputDto
    {
        public long Id { get; set; }
        public virtual TimeInterval PostInterval { get; set; }
        public virtual int PostIntervalValue { get; set; }
        public virtual TimeInterval StoryInterval { get; set; }
        public virtual int StoryIntervalValue { get; set; }
        public virtual TimeInterval MessageInterval { get; set; }
        public virtual int MessageIntervalValue { get; set; }
        public virtual string MessageBody { get; set; }
        public List<string> RecipientTags { get; set; }

        public  int PostNumbers { get; set; }
        public  int StoryNumbers { get; set; }
        public  int MessageNumbers { get; set; }
    }
}
