namespace Commuter.FeedJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHashToSubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscription", "Hash", c => c.String(nullable: false, maxLength: 45));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscription", "Hash");
        }
    }
}
