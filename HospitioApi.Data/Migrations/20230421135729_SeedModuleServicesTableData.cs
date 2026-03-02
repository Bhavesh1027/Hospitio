using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SeedModuleServicesTableData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Admin Module Services
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(1,'Settings',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(1,'Digital Assistant',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(1,'Guest Alerts',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(1,'Payments',1,GETUTCDATE())");


            //Communication Hub Module Services
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'WhatsApp',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'WhatsApp Branded',0,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'Telegram',0,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'Telegram Branded',0,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'Messenger',0,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'SMS',0,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'Email',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'Web Chat',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'ChatBot',0,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(2,'ChatGPT',0,GETUTCDATE())");


            //Guest Portal Module Services
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(5,'Menu',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(5,'Property Info',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(5,'Enhance your Stay',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(5,'Reception',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(5,'Housekeeping',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(5,'Room Service',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(5,'Concierge',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[ModuleServices]([ModuleId],[Name],[IsActive],[CreatedAt]) VALUES(5,'Local Experiences',1,GETUTCDATE())");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
