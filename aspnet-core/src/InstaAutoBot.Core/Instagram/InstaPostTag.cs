using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities.Auditing;

namespace InstaAutoBot.Instagram
{
    [Table("InstaPostTags")]
    [Audited]
    public class InstaPostTag : FullAuditedEntity<long>
    {
        [ForeignKey("InstaPostId")]
        public InstaPost InstaPost { get; set; }
        public long InstaPostId { get; set; }


        [Required]
        [StringLength(256)]
        public virtual string TagUserName { get; set; }
    }
}