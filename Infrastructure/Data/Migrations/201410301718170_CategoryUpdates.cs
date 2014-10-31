namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Districts", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.CategoryDetails", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.DemographicDetails", "Demographic_Id", "dbo.Demographics");
            DropIndex("dbo.Districts", new[] { "Region_Id" });
            DropIndex("dbo.CategoryDetails", new[] { "Category_Id" });
            DropIndex("dbo.DemographicDetails", new[] { "Demographic_Id" });
            AddColumn("dbo.StaarTests", "Count", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.StaarTests", "Percentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CategoryDetails", "CategoryType", c => c.Int(nullable: false));
            AlterColumn("dbo.Districts", "Region_Id", c => c.Long(nullable: false));
            AlterColumn("dbo.CategoryDetails", "Category_Id", c => c.Long(nullable: false));
            AlterColumn("dbo.DemographicDetails", "Demographic_Id", c => c.Long(nullable: false));
            CreateIndex("dbo.Districts", "Region_Id");
            CreateIndex("dbo.CategoryDetails", "Category_Id");
            CreateIndex("dbo.DemographicDetails", "Demographic_Id");
            AddForeignKey("dbo.Districts", "Region_Id", "dbo.Regions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CategoryDetails", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DemographicDetails", "Demographic_Id", "dbo.Demographics", "Id", cascadeDelete: true);
            DropColumn("dbo.StaarTests", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StaarTests", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.DemographicDetails", "Demographic_Id", "dbo.Demographics");
            DropForeignKey("dbo.CategoryDetails", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Districts", "Region_Id", "dbo.Regions");
            DropIndex("dbo.DemographicDetails", new[] { "Demographic_Id" });
            DropIndex("dbo.CategoryDetails", new[] { "Category_Id" });
            DropIndex("dbo.Districts", new[] { "Region_Id" });
            AlterColumn("dbo.DemographicDetails", "Demographic_Id", c => c.Long());
            AlterColumn("dbo.CategoryDetails", "Category_Id", c => c.Long());
            AlterColumn("dbo.Districts", "Region_Id", c => c.Long());
            DropColumn("dbo.CategoryDetails", "CategoryType");
            DropColumn("dbo.StaarTests", "Percentage");
            DropColumn("dbo.StaarTests", "Count");
            CreateIndex("dbo.DemographicDetails", "Demographic_Id");
            CreateIndex("dbo.CategoryDetails", "Category_Id");
            CreateIndex("dbo.Districts", "Region_Id");
            AddForeignKey("dbo.DemographicDetails", "Demographic_Id", "dbo.Demographics", "Id");
            AddForeignKey("dbo.CategoryDetails", "Category_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Districts", "Region_Id", "dbo.Regions", "Id");
        }
    }
}
