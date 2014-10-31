namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.Long(nullable: false),
                        Name = c.String(),
                        District_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.District_Id, cascadeDelete: true)
                .Index(t => t.District_Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.Long(nullable: false),
                        Name = c.String(),
                        Region_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.Long(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StaarTests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Campus_Id = c.Long(nullable: false),
                        CategoryDetail_Id = c.Long(nullable: false),
                        DemographicDetail_Id = c.Long(nullable: false),
                        Language_Id = c.Long(nullable: false),
                        Subject_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campus", t => t.Campus_Id, cascadeDelete: true)
                .ForeignKey("dbo.CategoryDetails", t => t.CategoryDetail_Id, cascadeDelete: true)
                .ForeignKey("dbo.DemographicDetails", t => t.DemographicDetail_Id, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.Language_Id, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .Index(t => t.Campus_Id)
                .Index(t => t.CategoryDetail_Id)
                .Index(t => t.DemographicDetail_Id)
                .Index(t => t.Language_Id)
                .Index(t => t.Subject_Id);
            
            CreateTable(
                "dbo.CategoryDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Detail = c.String(),
                        Description = c.String(),
                        YearStarted = c.Int(nullable: false),
                        Category_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DemographicDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Detail = c.String(),
                        Description = c.String(),
                        YearStarted = c.Int(nullable: false),
                        Demographic_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Demographics", t => t.Demographic_Id)
                .Index(t => t.Demographic_Id);
            
            CreateTable(
                "dbo.Demographics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        YearStarted = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        YearStarted = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StaarTests", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.StaarTests", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.StaarTests", "DemographicDetail_Id", "dbo.DemographicDetails");
            DropForeignKey("dbo.DemographicDetails", "Demographic_Id", "dbo.Demographics");
            DropForeignKey("dbo.StaarTests", "CategoryDetail_Id", "dbo.CategoryDetails");
            DropForeignKey("dbo.CategoryDetails", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.StaarTests", "Campus_Id", "dbo.Campus");
            DropForeignKey("dbo.Campus", "District_Id", "dbo.Districts");
            DropForeignKey("dbo.Districts", "Region_Id", "dbo.Regions");
            DropIndex("dbo.DemographicDetails", new[] { "Demographic_Id" });
            DropIndex("dbo.CategoryDetails", new[] { "Category_Id" });
            DropIndex("dbo.StaarTests", new[] { "Subject_Id" });
            DropIndex("dbo.StaarTests", new[] { "Language_Id" });
            DropIndex("dbo.StaarTests", new[] { "DemographicDetail_Id" });
            DropIndex("dbo.StaarTests", new[] { "CategoryDetail_Id" });
            DropIndex("dbo.StaarTests", new[] { "Campus_Id" });
            DropIndex("dbo.Districts", new[] { "Region_Id" });
            DropIndex("dbo.Campus", new[] { "District_Id" });
            DropTable("dbo.Subjects");
            DropTable("dbo.Languages");
            DropTable("dbo.Demographics");
            DropTable("dbo.DemographicDetails");
            DropTable("dbo.Categories");
            DropTable("dbo.CategoryDetails");
            DropTable("dbo.StaarTests");
            DropTable("dbo.Regions");
            DropTable("dbo.Districts");
            DropTable("dbo.Campus");
        }
    }
}
