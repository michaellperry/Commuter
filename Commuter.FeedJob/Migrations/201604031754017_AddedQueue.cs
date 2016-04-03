namespace Commuter.FeedJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedQueue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Queue",
                c => new
                    {
                        QueueId = c.Int(nullable: false, identity: true),
                        SubscriptionId = c.Int(nullable: false),
                        EpisodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QueueId)
                .ForeignKey("dbo.Episode", t => t.EpisodeId)
                .ForeignKey("dbo.Subscription", t => t.SubscriptionId, cascadeDelete: true)
                .Index(t => t.SubscriptionId)
                .Index(t => t.EpisodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Queue", "SubscriptionId", "dbo.Subscription");
            DropForeignKey("dbo.Queue", "EpisodeId", "dbo.Episode");
            DropIndex("dbo.Queue", new[] { "EpisodeId" });
            DropIndex("dbo.Queue", new[] { "SubscriptionId" });
            DropTable("dbo.Queue");
        }
    }
}
