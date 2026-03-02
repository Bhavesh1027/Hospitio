using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SeedPermissionsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Add Default Permission 
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Customer','Customer',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Customer Profile','CustomerProfile',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Account Settings','AccountSettings',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('e-Keys','eKeys',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Tickets','Tickets',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Offers','Offers',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Upsell','Upsell',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Q&A','QA',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Notifications','Notifications',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Alert Settings','AlertSettings',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Journal Settings','JournalSettings',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Event Logs','EventLogs',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('KPIs','KPIs',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt]) VALUES('Live Chat','LiveChat',1,GETUTCDATE())");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
