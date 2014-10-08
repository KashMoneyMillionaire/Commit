using System.Data.Entity.Migrations;

namespace Infrastructure.EntityMapping.Migrations
{
    public partial class SubCatField : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubCatFields",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Subject = c.Long(nullable: false),
                        Category = c.Long(nullable: false),
                        Field = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.StaarStats", "SubCatField_Id", c => c.Long(nullable: false));
            CreateIndex("dbo.StaarStats", "SubCatField_Id");
            AddForeignKey("dbo.StaarStats", "SubCatField_Id", "dbo.SubCatFields", "Id", cascadeDelete: true);
            DropColumn("dbo.StaarStats", "Subject");
            DropColumn("dbo.StaarStats", "Field");
            DropColumn("dbo.StaarStats", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StaarStats", "Category", c => c.Long(nullable: false));
            AddColumn("dbo.StaarStats", "Field", c => c.Long(nullable: false));
            AddColumn("dbo.StaarStats", "Subject", c => c.Long(nullable: false));
            DropForeignKey("dbo.StaarStats", "SubCatField_Id", "dbo.SubCatFields");
            DropIndex("dbo.StaarStats", new[] { "SubCatField_Id" });
            DropColumn("dbo.StaarStats", "SubCatField_Id");
            DropTable("dbo.SubCatFields");
        }
    }
}
