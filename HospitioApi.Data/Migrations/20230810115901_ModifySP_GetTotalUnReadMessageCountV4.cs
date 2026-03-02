using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifySP_GetTotalUnReadMessageCountV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetTotalUnReadMessageCount]
(
	 @UserId INT = 0,
	 @UserType INT = 0,
	 @ChatUserType VARCHAR(20) = ''
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON
	
	IF(@UserType = 3)
	BEGIN
		SELECT COUNT(DISTINCT([CM].[Id])) AS [TotalUnreadCount]
		FROM [dbo].[Channels] C (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU 
				ON [C].[Id] = [CU].[ChannelId] 
					AND [CU].[UserId] = @UserId
					AND [CU].[UserType] = @ChatUserType
			INNER JOIN [dbo].[ChannelMessages] CM  
				ON  [CM].[ChannelId] = [C].[Id] 
					AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0)
					AND (
						(
								CM.MessageSender = @UserType
								AND CM.MessageSenderId <> @UserId
						)
						OR (
								CM.MessageSender <> @UserType
						)
					)
	END
	ELSE 
	BEGIN
		SELECT COUNT(DISTINCT([C].[Id])) AS [TotalUnreadCount]
		FROM [dbo].[Channels] C (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU 
				ON [C].[Id] = [CU].[ChannelId] 
					AND [CU].[UserId] = @UserId
					AND [CU].[UserType] = @ChatUserType
			INNER JOIN [dbo].[ChannelMessages] CM  
				ON  [CM].[ChannelId] = [C].[Id] 
					AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0)
					AND (
						(
								CM.MessageSender = @UserType
								AND CM.MessageSenderId <> @UserId
						)
						OR (
								CM.MessageSender <> @UserType
						)
					)
	END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
