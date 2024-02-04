namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class registrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventAttendees",
                c => new
                    {
                        Event_EventID = c.Int(nullable: false),
                        Attendee_AttendeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_EventID, t.Attendee_AttendeeID })
                .ForeignKey("dbo.Events", t => t.Event_EventID, cascadeDelete: true)
                .ForeignKey("dbo.Attendees", t => t.Attendee_AttendeeID, cascadeDelete: true)
                .Index(t => t.Event_EventID)
                .Index(t => t.Attendee_AttendeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventAttendees", "Attendee_AttendeeID", "dbo.Attendees");
            DropForeignKey("dbo.EventAttendees", "Event_EventID", "dbo.Events");
            DropIndex("dbo.EventAttendees", new[] { "Attendee_AttendeeID" });
            DropIndex("dbo.EventAttendees", new[] { "Event_EventID" });
            DropTable("dbo.EventAttendees");
        }
    }
}
