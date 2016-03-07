namespace Commuter.FeedJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeDateTimesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Podcast", "LastUpdateDateTime", c => c.DateTime());
            AlterColumn("dbo.Podcast", "LastAttemptDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Podcast", "LastAttemptDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Podcast", "LastUpdateDateTime", c => c.DateTime(nullable: false));
        }
    }
}
