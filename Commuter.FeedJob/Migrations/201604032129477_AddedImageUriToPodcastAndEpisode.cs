namespace Commuter.FeedJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageUriToPodcastAndEpisode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Podcast", "ImageUri", c => c.String(maxLength: 512));
            AddColumn("dbo.Episode", "ImageUri", c => c.String(maxLength: 512));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Episode", "ImageUri");
            DropColumn("dbo.Podcast", "ImageUri");
        }
    }
}
