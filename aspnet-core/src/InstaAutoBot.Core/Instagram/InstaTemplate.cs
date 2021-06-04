using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities.Auditing;

namespace InstaAutoBot.Instagram
{
    [Table("InstaTemplates")]
    [Audited]
    public class InstaTemplate : FullAuditedEntity<long>
    {
        [ForeignKey("InstaAccountId")]
        public InstaAccount InstaAccount { get; set; }
        public long InstaAccountId { get; set; }

        public virtual ICollection<InstaTemplateTag> InstaTemplateTags { get; set; }

        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(256)]
        public virtual string ZipFileName { get; set; }

        [Required]
        [StringLength(100)]
        public virtual int PostsIntervalInHours { get; set; }

        [Required]
        [StringLength(100)]
        public virtual int StoriesIntervalInHours { get; set; }
    }
}