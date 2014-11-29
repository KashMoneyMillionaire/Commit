namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryDetailAdjustment : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CategoryDetails", name: "PercentDetail_Id", newName: "PartnerDetail_Id");
            RenameIndex(table: "dbo.CategoryDetails", name: "IX_PercentDetail_Id", newName: "IX_PartnerDetail_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.CategoryDetails", name: "IX_PartnerDetail_Id", newName: "IX_PercentDetail_Id");
            RenameColumn(table: "dbo.CategoryDetails", name: "PartnerDetail_Id", newName: "PercentDetail_Id");
        }
    }
}
