using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities.Auditing;

namespace InstaAutoBot.Instagram
{
    [Table("InstaStories")]
    [Audited]
    public class InstaStory : FullAuditedEntity<long>
    {
        [ForeignKey("InstaAccountId")]
        public InstaAccount InstaAccount { get; set; }
        public long InstaAccountId { get; set; }


        [Required]
        [StringLength(256)]
        public virtual string FileName { get; set; }
    }
}