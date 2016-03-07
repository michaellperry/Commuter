using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Commuter.FeedJob.Entities
{
    public class CommuterDbContext : DbContext
    {
        public CommuterDbContext()
            : base("Commuter")
        {

        }

        public virtual DbSet<Podcast> Podcasts { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Subscription>()
                .Property(x => x.Hash)
                .IsRequired()
                .HasMaxLength(45);

            modelBuilder.Entity<Podcast>()
                .Property(x => x.FeedUrl)
                .IsRequired()
                .HasMaxLength(512);

            modelBuilder.Entity<Episode>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Episode>()
                .Property(x => x.Summary)
                .IsRequired()
                .IsMaxLength();
            modelBuilder.Entity<Episode>()
                .Property(x => x.MediaUrl)
                .IsRequired()
                .HasMaxLength(512);
        }
    }
}
