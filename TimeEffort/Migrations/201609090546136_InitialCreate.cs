namespace TimeEffort.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Access",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                        DateFrom = c.DateTime(),
                        DateTo = c.DateTime(),
                        UserInfo_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.Project", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("public.UserInfo", t => t.UserInfo_ID)
                .ForeignKey("public.Role", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.RoleID)
                .Index(t => t.UserInfo_ID);
            
            CreateTable(
                "public.Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        ContractUSD = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ContractUZS = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ManagerID = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        CType = c.String(),
                        CustomerId = c.Int(),
                        UserInfo_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.Customer", t => t.CustomerId)
                .ForeignKey("public.UserInfo", t => t.UserInfo_ID)
                .Index(t => t.CustomerId)
                .Index(t => t.UserInfo_ID);
            
            CreateTable(
                "public.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TIN = c.String(),
                        Address = c.String(),
                        ContactPhone = c.String(),
                        GenDirector = c.String(),
                        ContactPerson = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.Notification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FROMID = c.Int(),
                        TOID = c.Int(),
                        Date = c.DateTime(nullable: false),
                        MESSAGE = c.String(),
                        ISREAD = c.Boolean(nullable: false),
                        TYPEID = c.Int(),
                        ProjectId = c.Int(nullable: false),
                        NotificationType_ID = c.Int(),
                        UserInfo_ID = c.Int(),
                        UserInfo_ID1 = c.Int(),
                        UserInfo_ID2 = c.Int(),
                        UserInfo1_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.NotificationType", t => t.NotificationType_ID)
                .ForeignKey("public.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("public.UserInfo", t => t.UserInfo_ID)
                .ForeignKey("public.UserInfo", t => t.UserInfo_ID1)
                .ForeignKey("public.UserInfo", t => t.UserInfo_ID2)
                .ForeignKey("public.UserInfo", t => t.UserInfo1_ID)
                .Index(t => t.ProjectId)
                .Index(t => t.NotificationType_ID)
                .Index(t => t.UserInfo_ID)
                .Index(t => t.UserInfo_ID1)
                .Index(t => t.UserInfo_ID2)
                .Index(t => t.UserInfo1_ID);
            
            CreateTable(
                "public.NotificationType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NAME = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.UserInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        PositionID = c.Int(nullable: false),
                        Username = c.String(),
                        Password = c.String(),
                        Address = c.String(),
                        Major = c.String(),
                        DirectHead = c.Int(),
                        UserInfo2_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.Position", t => t.PositionID, cascadeDelete: true)
                .ForeignKey("public.UserInfo", t => t.UserInfo2_ID)
                .Index(t => t.PositionID)
                .Index(t => t.UserInfo2_ID);
            
            CreateTable(
                "public.Position",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.Workload",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        UserID = c.Int(nullable: false),
                        ProjectID = c.Int(),
                        Duration = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Note = c.String(),
                        WorkloadTypeID = c.Int(nullable: false),
                        UserInfo_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.Project", t => t.ProjectID)
                .ForeignKey("public.UserInfo", t => t.UserInfo_ID)
                .ForeignKey("public.WorkloadType", t => t.WorkloadTypeID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.WorkloadTypeID)
                .Index(t => t.UserInfo_ID);
            
            CreateTable(
                "public.WorkloadType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Access", "RoleID", "public.Role");
            DropForeignKey("public.Notification", "UserInfo1_ID", "public.UserInfo");
            DropForeignKey("public.Notification", "UserInfo_ID2", "public.UserInfo");
            DropForeignKey("public.Workload", "WorkloadTypeID", "public.WorkloadType");
            DropForeignKey("public.Workload", "UserInfo_ID", "public.UserInfo");
            DropForeignKey("public.Workload", "ProjectID", "public.Project");
            DropForeignKey("public.UserInfo", "UserInfo2_ID", "public.UserInfo");
            DropForeignKey("public.Project", "UserInfo_ID", "public.UserInfo");
            DropForeignKey("public.UserInfo", "PositionID", "public.Position");
            DropForeignKey("public.Notification", "UserInfo_ID1", "public.UserInfo");
            DropForeignKey("public.Notification", "UserInfo_ID", "public.UserInfo");
            DropForeignKey("public.Access", "UserInfo_ID", "public.UserInfo");
            DropForeignKey("public.Notification", "ProjectId", "public.Project");
            DropForeignKey("public.Notification", "NotificationType_ID", "public.NotificationType");
            DropForeignKey("public.Project", "CustomerId", "public.Customer");
            DropForeignKey("public.Access", "ProjectID", "public.Project");
            DropIndex("public.Workload", new[] { "UserInfo_ID" });
            DropIndex("public.Workload", new[] { "WorkloadTypeID" });
            DropIndex("public.Workload", new[] { "ProjectID" });
            DropIndex("public.UserInfo", new[] { "UserInfo2_ID" });
            DropIndex("public.UserInfo", new[] { "PositionID" });
            DropIndex("public.Notification", new[] { "UserInfo1_ID" });
            DropIndex("public.Notification", new[] { "UserInfo_ID2" });
            DropIndex("public.Notification", new[] { "UserInfo_ID1" });
            DropIndex("public.Notification", new[] { "UserInfo_ID" });
            DropIndex("public.Notification", new[] { "NotificationType_ID" });
            DropIndex("public.Notification", new[] { "ProjectId" });
            DropIndex("public.Project", new[] { "UserInfo_ID" });
            DropIndex("public.Project", new[] { "CustomerId" });
            DropIndex("public.Access", new[] { "UserInfo_ID" });
            DropIndex("public.Access", new[] { "RoleID" });
            DropIndex("public.Access", new[] { "ProjectID" });
            DropTable("public.Role");
            DropTable("public.WorkloadType");
            DropTable("public.Workload");
            DropTable("public.Position");
            DropTable("public.UserInfo");
            DropTable("public.NotificationType");
            DropTable("public.Notification");
            DropTable("public.Customer");
            DropTable("public.Project");
            DropTable("public.Access");
        }
    }
}
