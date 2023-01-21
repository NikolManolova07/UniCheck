namespace UniCheckManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Specialty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Specialties", "Length", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Specialties", "Length", c => c.String(nullable: false));
        }
    }
}
