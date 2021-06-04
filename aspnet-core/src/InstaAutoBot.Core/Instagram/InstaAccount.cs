using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities.Auditing;

namespace InstaAutoBot.Instagram
{
    [Table("InstaAccounts")]
    [Audited]
    public class InstaAccount : FullAuditedEntity<long>
    {
        public virtual ICollection<InstaPost> InstaPosts { get; set; }
        public virtual ICollection<InstaStory> InstaStories { get; set; }


        [Required]
        [StringLength(256)]
        public virtual string FirstName { get; set; }

        [StringLength(256)]
        public virtual string UserName { get; set; }

        [StringLength(256)]
        public virtual string Password { get; set; }

        [StringLength(256)]
        public virtual string PhoneNumber { get; set; }

         
        public string ProfilePicture { get; set; }

        [StringLength(256)]
        public string ProfilePictureId { get; set; } = "unknown";

        [StringLength(256)]
        public string FullName { get; set; }

        public bool IsVerified { get; set; }

        public bool IsPrivate { get; set; }

        public long Pk { get; set; }
    }
}