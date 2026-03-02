using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatMessageList_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_GetChatMessageList]
(
    @ChatId INT = 0,
    @PageNo INT = 1,
    @PageSize INT = 10
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

SELECT(
    SELECT [CM].[Id],
           [CM].[ChannelId],
           [CM].[MessageType],
           [CM].[MessageSender],
           [CM].[Source],
           [CM].[MsgReqType],
           [CM].[Attachment],
           [CM].[RequestId],
           [CM].[Url],
           [CM].[Message],
           [CM].[TranslateMessage],
           [CM].[IsActive],
           [CM].[CreatedAt],
		   [CM].[IsRead],
		   [CM].[MessageSenderId],
		   CASE 
			WHEN MessageSender = 1  THEN 'HospitioUser'
			WHEN MessageSender = 2  THEN 'CustomerUser'
			WHEN MessageSender = 3  THEN 'CustomerGuest'
			END AS [UserType]
    FROM [dbo].[ChannelMessages] CM (NOLOCK)
    WHERE [CM].[DeletedAt] IS NULL
          AND [CM].[ChannelId] = @ChatId
    ORDER BY [CM].[CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
	FOR JSON PATH
	)

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
