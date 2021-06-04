using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities.Auditing;

namespace InstaAutoBot.Instagram
{
    [Table("InstaMessageRecipients")]
    [Audited]
    public class InstaMessageRecipient : FullAuditedEntity<long>
    {
        [ForeignKey("InstaSettingId")]
        public InstaSetting InstaSetting { get; set; }
        public long InstaSettingId { get; set; }


        [Required]
        [StringLength(256)]
        public string Recipient { get; set; }
    }
}