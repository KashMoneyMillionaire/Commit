namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryDetailPercent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoryDetails", "PercentDetail_Id", c => c.Long());
            CreateIndex("dbo.CategoryDetails", "PercentDetail_Id");
            AddForeignKey("dbo.CategoryDetails", "PercentDetail_Id", "dbo.CategoryDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryDetails", "PercentDetail_Id", "dbo.CategoryDetails");
            DropIndex("dbo.CategoryDetails", new[] { "PercentDetail_Id" });
            DropColumn("dbo.CategoryDetails", "PercentDetail_Id");
        }
    }
}
