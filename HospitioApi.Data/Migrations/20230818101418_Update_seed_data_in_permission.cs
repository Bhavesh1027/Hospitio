using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Update_seed_data_in_permission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 0, IsView = 1 Where Name = 'Customer'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 0, IsView = 1 Where Name = 'Customer Profile'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 0, IsView = 1 Where Name = 'Account Settings'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=1, IsSend = 1, IsUpload = 1, IsView = 1 Where Name = 'e-Keys'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=1, IsSend = 1, IsUpload = 1, IsView = 1 Where Name = 'Event Logs'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=1, IsSend = 1, IsUpload = 1, IsView = 1 Where Name = 'Upsell'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 0, IsReply=1, IsSend = 0, IsUpload = 0, IsView = 1 Where Name = 'Tickets'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 0, IsView = 1 Where Name = 'Offers'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 0, IsView = 1 Where Name = 'Q&A'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 0, IsReply=0, IsSend = 0, IsUpload = 1, IsView = 1 Where Name = 'Notifications'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 1, IsView = 1 Where Name = 'Alert Settings'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 1, IsView = 1 Where Name = 'Journal Settings'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 1, IsReply=0, IsSend = 0, IsUpload = 0, IsView = 1 Where Name = 'KPIs'");
            migrationBuilder.Sql(@"Update Permissions Set IsEdit = 0, IsReply=1, IsSend = 1, IsUpload = 1, IsView = 1 Where Name = 'Live Chat'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
