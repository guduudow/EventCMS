namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reception : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventAttendees", "Event_EventID", "dbo.Events");
            DropForeignKey("dbo.EventAttendees", "Attendee_AttendeeID", "dbo.Attendees");
            DropIndex("dbo.EventAttendees", new[] { "Event_EventID" });
            DropIndex("dbo.EventAttendees", new[] { "Attendee_AttendeeID" });
            CreateTable(
                "dbo.Receptions",
                c => new
                    {
                        ReceptionID = c.Int(nullable: false, identity: true),
                        ReceptionName = c.String(),
                        ReceptionLocation = c.String(),
                        ReceptionDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ReceptionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceptionDescription = c.String(),
                    })
                .PrimaryKey(t => t.ReceptionID);
            
            CreateTable(
                "dbo.ReceptionAttendees",
                c => new
                    {
                        Reception_ReceptionID = c.Int(nullable: false),
                        Attendee_AttendeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reception_ReceptionID, t.Attendee_AttendeeID })
                .ForeignKey("dbo.Receptions", t => t.Reception_ReceptionID, cascadeDelete: true)
                .ForeignKey("dbo.Attendees", t => t.Attendee_AttendeeID, cascadeDelete: true)
                .Index(t => t.Reception_ReceptionID)
                .Index(t => t.Attendee_AttendeeID);
            
            DropTable("dbo.Events");
            DropTable("dbo.EventAttendees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EventAttendees",
                c => new
                    {
                        Event_EventID = c.Int(nullable: false),
                        Attendee_AttendeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_EventID, t.Attendee_AttendeeID });
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        EventLocation = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        EventPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EventDescription = c.String(),
                    })
                .PrimaryKey(t => t.EventID);
            
            DropForeignKey("dbo.ReceptionAttendees", "Attendee_AttendeeID", "dbo.Attendees");
            DropForeignKey("dbo.ReceptionAttendees", "Reception_ReceptionID", "dbo.Receptions");
            DropIndex("dbo.ReceptionAttendees", new[] { "Attendee_AttendeeID" });
            DropIndex("dbo.ReceptionAttendees", new[] { "Reception_ReceptionID" });
            DropTable("dbo.ReceptionAttendees");
            DropTable("dbo.Receptions");
            CreateIndex("dbo.EventAttendees", "Attendee_AttendeeID");
            CreateIndex("dbo.EventAttendees", "Event_EventID");
            AddForeignKey("dbo.EventAttendees", "Attendee_AttendeeID", "dbo.Attendees", "AttendeeID", cascadeDelete: true);
            AddForeignKey("dbo.EventAttendees", "Event_EventID", "dbo.Events", "EventID", cascadeDelete: true);
        }
    }
}
