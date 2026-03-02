using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SeedModulesTableData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Seed Modules Table Data 
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Admin',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Communication Hub',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Guest Journey',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Online Check-in',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Guest Portal',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('e-Keys',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Analytics',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('User & Permissions',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Integrations',0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Price per Room/Appartments',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Minimum Amount',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Modules] ([Name],[ModuleType],[IsActive],[CreatedAt]) VALUES('Pricing Period',2,1,GETUTCDATE())");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
