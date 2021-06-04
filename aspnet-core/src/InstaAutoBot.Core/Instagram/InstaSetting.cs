using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities.Auditing;

namespace InstaAutoBot.Instagram
{
    [Table("InstaSettings")]
    [Audited]
    public class InstaSetting : FullAuditedEntity<long>
    {
        public virtual TimeInterval PostInterval { get; set; }
        public virtual int PostIntervalValue { get; set; }
        public virtual int PostNumbers { get; set; }

        public virtual TimeInterval StoryInterval { get; set; }
        public virtual int StoryIntervalValue { get; set; }
        public virtual int StoryNumbers { get; set; }

        public virtual TimeInterval MessageInterval { get; set; }
        public virtual int MessageIntervalValue { get; set; }
        public virtual int MessageNumbers { get; set; }

        public virtual ICollection<InstaMessageRecipient> InstaMessageRecipients { get; set; }
        public virtual string MessageBody { get; set; }
    }

    public enum TimeInterval
    {
        PerDay,
        PerWeek,
        PerMonth
    }


}