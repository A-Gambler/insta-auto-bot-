using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using InstaAutoBot.Authorization.Roles;
using InstaAutoBot.Authorization.Users;
using InstaAutoBot.Instagram;
using InstaAutoBot.MultiTenancy;

namespace InstaAutoBot.EntityFrameworkCore
{
    public class InstaAutoBotDbContext : AbpZeroDbContext<Tenant, Role, User, InstaAutoBotDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<InstaAccount> InstaAccounts { get; set; }
        public virtual DbSet<InstaPost> InstaPosts { get; set; }
        public virtual DbSet<InstaStory> InstaStories { get; set; }
        public virtual DbSet<InstaTemplate> InstaTemplates { get; set; }
        public virtual DbSet<InstaTemplateTag> InstaTemplateTags { get; set; }
        public virtual DbSet<InstaMessageRecipient> InstaMessageRecipients { get; set; }
        public virtual DbSet<InstaMessage> InstaMessages { get; set; }

        public virtual DbSet<InstaSetting> InstaSettings { get; set; }

        public InstaAutoBotDbContext(DbContextOptions<InstaAutoBotDbContext> options)
            : base(options)
        {
        }
    }
}
