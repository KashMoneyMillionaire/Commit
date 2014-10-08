using System.Data.Entity.Migrations;

namespace Infrastructure.EntityMapping.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CampusNumber = c.Long(nullable: false),
                        Name = c.String(),
                        IsCharterSchool = c.Boolean(nullable: false),
                        CountyName = c.String(),
                        CountyId = c.Long(nullable: false),
                        AccountabilityRating = c.Long(nullable: false),
                        DistrictName = c.String(),
                        DistrictNumber = c.Long(nullable: false),
                        StartGrade = c.Long(nullable: false),
                        EndGrade = c.Long(nullable: false),
                        GradeType = c.Long(nullable: false),
                        RegionNumber = c.Long(nullable: false),
                        CackDtl = c.Long(nullable: false),
                        CadMath = c.Boolean(nullable: false),
                        CadRead = c.Boolean(nullable: false),
                        CadProgress = c.Boolean(nullable: false),
                        IsRatedUnderAEAProcedures = c.Boolean(nullable: false),
                        PairedCampus_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campus", t => t.PairedCampus_Id)
                .Index(t => t.PairedCampus_Id);
            
            CreateTable(
                "dbo.StaarStats",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.Long(nullable: false),
                        Grade = c.Long(nullable: false),
                        Language = c.Long(nullable: false),
                        Subject = c.Long(nullable: false),
                        Field = c.Long(nullable: false),
                        Category = c.Long(nullable: false),
                        Value = c.String(),
                        Campus_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campus", t => t.Campus_Id, cascadeDelete: true)
                .Index(t => t.Campus_Id);
            
            CreateTable(
                "dbo.CompletedFiles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileName = c.String(),
                        TimeCompleted = c.DateTime(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StaarStats", "Campus_Id", "dbo.Campus");
            DropForeignKey("dbo.Campus", "PairedCampus_Id", "dbo.Campus");
            DropIndex("dbo.StaarStats", new[] { "Campus_Id" });
            DropIndex("dbo.Campus", new[] { "PairedCampus_Id" });
            DropTable("dbo.CompletedFiles");
            DropTable("dbo.StaarStats");
            DropTable("dbo.Campus");
        }
    }
}
