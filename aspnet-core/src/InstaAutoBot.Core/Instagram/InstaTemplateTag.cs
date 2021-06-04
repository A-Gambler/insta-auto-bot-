using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities.Auditing;

namespace InstaAutoBot.Instagram
{
    [Table("InstaTemplateTags")]
    [Audited]
    public class InstaTemplateTag : FullAuditedEntity<long>
    {
        [ForeignKey("InstaTemplateId")]
        public InstaTemplate InstaTemplate { get; set; }
        public long InstaTemplateId { get; set; }


        [Required]
        [StringLength(256)]
        public virtual string TagUserName { get; set; }
    }
}