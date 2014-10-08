using System.Data.Entity.Migrations;

namespace Infrastructure.EntityMapping.Migrations
{
    public partial class Change : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YearGradeLangs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.Long(nullable: false),
                        Grade = c.Long(nullable: false),
                        Language = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.StaarStats", "YearGradeLang_Id", c => c.Long(nullable: false));
            CreateIndex("dbo.StaarStats", "YearGradeLang_Id");
            AddForeignKey("dbo.StaarStats", "YearGradeLang_Id", "dbo.YearGradeLangs", "Id", cascadeDelete: true);
            DropColumn("dbo.StaarStats", "Year");
            DropColumn("dbo.StaarStats", "Grade");
            DropColumn("dbo.StaarStats", "Language");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StaarStats", "Language", c => c.Long(nullable: false));
            AddColumn("dbo.StaarStats", "Grade", c => c.Long(nullable: false));
            AddColumn("dbo.StaarStats", "Year", c => c.Long(nullable: false));
            DropForeignKey("dbo.StaarStats", "YearGradeLang_Id", "dbo.YearGradeLangs");
            DropIndex("dbo.StaarStats", new[] { "YearGradeLang_Id" });
            DropColumn("dbo.StaarStats", "YearGradeLang_Id");
            DropTable("dbo.YearGradeLangs");
        }
    }
}
