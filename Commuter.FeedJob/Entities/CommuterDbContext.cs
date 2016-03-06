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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Subscription>();

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
