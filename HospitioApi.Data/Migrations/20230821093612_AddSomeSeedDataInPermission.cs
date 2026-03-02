using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddSomeSeedDataInPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Update Permissions Set [Name]= 'Journey Settings',[NormalizedName] = 'JourneySettings', IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 1, IsView = 1 Where Name = 'Journal Settings'");
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt],[IsEdit],[IsReply],[IsSend],[IsUpload],[IsView])       VALUES('BI','BI',1,GETUTCDATE(),1,0,0,0,1)");
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt],[IsEdit],[IsReply],[IsSend],[IsUpload],[IsView])       VALUES('Financials','Financials',1,GETUTCDATE(),1,0,0,0,1)");
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt],[IsEdit],[IsReply],[IsSend],[IsUpload],[IsView])       VALUES('Digital Market Place','DigitalMarketPlace',1,GETUTCDATE(),0,0,0,0,0)");
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt],[IsEdit],[IsReply],[IsSend],[IsUpload],[IsView])       VALUES('Integration','Integration',1,GETUTCDATE(),0,0,0,0,0)");
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Permissions]([Name],[NormalizedName],[IsActive],[CreatedAt],[IsEdit],[IsReply],[IsSend],[IsUpload],[IsView])       VALUES('Payments','Payments',1,GETUTCDATE(),1,0,0,0,1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
