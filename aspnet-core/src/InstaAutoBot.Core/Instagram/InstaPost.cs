using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities.Auditing;

namespace InstaAutoBot.Instagram
{
    [Table("InstaPosts")]
    [Audited]
    public class InstaPost : FullAuditedEntity<long>
    {
        [ForeignKey("InstaAccountId")]
        public InstaAccount InstaAccount { get; set; }
        public long InstaAccountId { get; set; }


        [Required]
        [StringLength(256)]
        public virtual string FileName { get; set; }


        public virtual ICollection<InstaPostTag> InstaPostTags { get; set; }
    }



}