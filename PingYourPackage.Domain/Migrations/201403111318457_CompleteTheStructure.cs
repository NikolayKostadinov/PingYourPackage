namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteTheStructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Affiliates",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 50),
                        TelephoneNumber = c.String(maxLength: 50),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Users", t => t.Key)
                .Index(t => t.Key);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        AffiliateKey = c.Guid(nullable: false),
                        ShipmentTypeKey = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiverSurname = c.String(nullable: false, maxLength: 50),
                        ReceiverAddress = c.String(nullable: false, maxLength: 50),
                        ReceiverZipCode = c.String(nullable: false, maxLength: 50),
                        ReceiverCity = c.String(nullable: false, maxLength: 50),
                        ReceiverCountry = c.String(nullable: false, maxLength: 50),
                        ReceiverTelephone = c.String(nullable: false, maxLength: 50),
                        ReceiverEmail = c.String(nullable: false, maxLength: 320),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Affiliates", t => t.AffiliateKey, cascadeDelete: true)
                .ForeignKey("dbo.ShipmentTypes", t => t.ShipmentTypeKey, cascadeDelete: true)
                .Index(t => t.AffiliateKey)
                .Index(t => t.ShipmentTypeKey);
            
            CreateTable(
                "dbo.ShipmentStates",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShipmentKey = c.Guid(nullable: false),
                        ShipmentStatus = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Shipments", t => t.ShipmentKey, cascadeDelete: true)
                .Index(t => t.ShipmentKey);
            
            CreateTable(
                "dbo.ShipmentTypes",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Affiliates", "Key", "dbo.Users");
            DropForeignKey("dbo.Shipments", "ShipmentTypeKey", "dbo.ShipmentTypes");
            DropForeignKey("dbo.ShipmentStates", "ShipmentKey", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "AffiliateKey", "dbo.Affiliates");
            DropIndex("dbo.Affiliates", new[] { "Key" });
            DropIndex("dbo.Shipments", new[] { "ShipmentTypeKey" });
            DropIndex("dbo.ShipmentStates", new[] { "ShipmentKey" });
            DropIndex("dbo.Shipments", new[] { "AffiliateKey" });
            DropTable("dbo.ShipmentTypes");
            DropTable("dbo.ShipmentStates");
            DropTable("dbo.Shipments");
            DropTable("dbo.Affiliates");
        }
    }
}
