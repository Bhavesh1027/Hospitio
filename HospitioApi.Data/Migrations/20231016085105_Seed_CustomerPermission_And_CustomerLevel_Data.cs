using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Seed_CustomerPermission_And_CustomerLevel_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Add Default CustomerPermission
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('Guests','Guests',1,1,1,0,1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('Communication','Communication',0,0,0,1,1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('Guest Journey','GuestJourney',1,1,1,0,1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('e-Keys','eKeys',1,1,0,0,0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('Analytics','Analytics',1,0,0,0,0,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('Help Center','HelpCenter',1,1,1,1,1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('Admin','Admin',1,1,1,0,1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('Online Check in','OnlineCheckin',1,1,1,0,1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerPermissions]([Name],[NormalizedName],[IsView],[IsEdit],[IsUpload],[IsReply],[IsDownload],[IsActive],[CreatedAt]) Values ('Guest Portal','GuestPortal',1,1,1,0,1,1,GETUTCDATE())");

            //Add Default CustomerLevel
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerLevels]([LevelName],[NormalizedLevelName],[IsCustomerUserType],[IsActive],[CreatedAt]) VALUES('Super Admin','SuperAdmin',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerLevels]([LevelName],[NormalizedLevelName],[IsCustomerUserType],[IsActive],[CreatedAt]) VALUES('CEO','CEO',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerLevels]([LevelName],[NormalizedLevelName],[IsCustomerUserType],[IsActive],[CreatedAt]) VALUES('Manager','Manager',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerLevels]([LevelName],[NormalizedLevelName],[IsCustomerUserType],[IsActive],[CreatedAt]) VALUES('Group Leader','GroupLeader',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomerLevels]([LevelName],[NormalizedLevelName],[IsCustomerUserType],[IsActive],[CreatedAt]) VALUES('Staff','Staff',1,1,GETUTCDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
