namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rearangement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaarTests", "Grade", c => c.String());
            AddColumn("dbo.StaarTests", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.StaarTests", "Count");
            DropColumn("dbo.StaarTests", "Percentage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StaarTests", "Percentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.StaarTests", "Count", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.StaarTests", "Value");
            DropColumn("dbo.StaarTests", "Grade");
        }
    }
}
