using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CreateSP_GetTotalUnReadMessageCountPerChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_GetTotalUnReadMessageCountPerChat]
(
	 @UserId INT = 0,
	 @UserType INT = 0,
	 @ChatId INT = 0
)
AS
BEGIN

	SELECT COUNT(CM.Id) As [TotalUnreadMessageCount] from Channels C
	INNER JOIN ChannelMessages CM on CM.ChannelId = C.Id
	AND (
		(
			CM.MessageSender = @UserType AND (CM.MessageSenderId <> @UserId OR CM.MessageSenderId IS NULL)
		)
		OR
		(
			CM.MessageSender <> @UserType
		)
	)
	where C.Id = @ChatId
	
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
