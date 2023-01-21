namespace UniCheckManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Credits = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        EnrollmentId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Grade = c.String(),
                        ExamDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EnrollmentId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => new { t.CourseId, t.StudentId }, unique: true);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        FacultyNumber = c.String(nullable: false, maxLength: 10),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        EnrollmentDate = c.DateTime(nullable: false),
                        ImagePath = c.String(),
                        SpecialtyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Specialties", t => t.SpecialtyId, cascadeDelete: true)
                .Index(t => t.FacultyNumber, unique: true)
                .Index(t => t.SpecialtyId);
            
            CreateTable(
                "dbo.Specialties",
                c => new
                    {
                        SpecialtyId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ProfessionalStream = c.String(nullable: false),
                        Degree = c.String(nullable: false),
                        Qualification = c.String(nullable: false),
                        ModeOfStudy = c.String(nullable: false),
                        Length = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SpecialtyId);
            
            CreateTable(
                "dbo.Lecturers",
                c => new
                    {
                        LecturerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        HireDate = c.DateTime(nullable: false),
                        Office = c.String(nullable: false),
                        ImagePath = c.String(),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LecturerId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Faculty = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.CourseLecturer",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        LecturerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseId, t.LecturerId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.LecturerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.CourseLecturer", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.CourseLecturer", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Lecturers", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Students", "SpecialtyId", "dbo.Specialties");
            DropForeignKey("dbo.Enrollments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Enrollments", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseLecturer", new[] { "LecturerId" });
            DropIndex("dbo.CourseLecturer", new[] { "CourseId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Lecturers", new[] { "DepartmentId" });
            DropIndex("dbo.Students", new[] { "SpecialtyId" });
            DropIndex("dbo.Students", new[] { "FacultyNumber" });
            DropIndex("dbo.Enrollments", new[] { "CourseId", "StudentId" });
            DropTable("dbo.CourseLecturer");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Departments");
            DropTable("dbo.Lecturers");
            DropTable("dbo.Specialties");
            DropTable("dbo.Students");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Courses");
        }
    }
}
