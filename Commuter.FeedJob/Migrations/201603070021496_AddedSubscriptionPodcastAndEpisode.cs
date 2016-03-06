namespace Commuter.FeedJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSubscriptionPodcastAndEpisode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscription",
                c => new
                    {
                        SubscriptionId = c.Int(nullable: false, identity: true),
                        UserGuid = c.Guid(nullable: false),
                        StartAtEpisodeId = c.Int(nullable: false),
                        PodcastId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubscriptionId)
                .ForeignKey("dbo.Podcast", t => t.PodcastId, cascadeDelete: true)
                .Index(t => t.PodcastId);
            
            CreateTable(
                "dbo.Podcast",
                c => new
                    {
                        PodcastId = c.Int(nullable: false, identity: true),
                        FeedUrl = c.String(nullable: false, maxLength: 512),
                        LastUpdateDateTime = c.DateTime(nullable: false),
                        LastAttemptDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PodcastId);
            
            CreateTable(
                "dbo.Episode",
                c => new
                    {
                        EpisodeId = c.Int(nullable: false, identity: true),
                        PodcastId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Summary = c.String(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        MediaUrl = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.EpisodeId)
                .ForeignKey("dbo.Podcast", t => t.PodcastId, cascadeDelete: true)
                .Index(t => t.PodcastId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscription", "PodcastId", "dbo.Podcast");
            DropForeignKey("dbo.Episode", "PodcastId", "dbo.Podcast");
            DropIndex("dbo.Episode", new[] { "PodcastId" });
            DropIndex("dbo.Subscription", new[] { "PodcastId" });
            DropTable("dbo.Episode");
            DropTable("dbo.Podcast");
            DropTable("dbo.Subscription");
        }
    }
}
