using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatListByChatId_Modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE SP_GetChatListByChatId
(
	@ChatId INT = 0,
	@Id INT = 0
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

	DECLARE @UserId INT
	DECLARE @UserType VARCHAR(50)

	SELECT @UserType = [CU].[UserType],@UserId = [CU].[UserId] FROM [dbo].[ChannelUsers] (NOLOCK) CU
	WHERE [CU].[ChannelId] = @ChatId
	AND CU.[UserId] <> @Id

	IF(@UserType = 'CustomerUser')
	BEGIN
		SELECT DISTINCT [C].[Id] AS [UserId],[CT].[ChannelId] AS [ChatId],[CM].[Message] AS LastMessage,CT.LastMessageReadTime AS LastMessageTime,'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
			[CU].[ProfilePicture],[CT].[IsActive],
			(SELECT COUNT(*) FROM [dbo].[ChannelMessages] CMM (NOLOCK) WHERE CMM.IsRead = 0 AND CMM.[ChannelId] = [CT].[ChannelId]) AS [TotalUnReadCount],[CT].[UserType]
		FROM [dbo].[Customers](NOLOCK) C 
		LEFT JOIN [dbo].[CustomerUsers] CU ON [CU].[CustomerId] = [C].[Id]
		LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [C].[Id]
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId]
			LEFT JOIN [dbo].[BusinessTypes] BT (NOLOCK) 
				ON [BT].[Id] = [C].[BusinessTypeId] 
					AND [BT].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[Products] P (NOLOCK) 
				ON [P].[Id] = [C].[ProductId] 
					AND  [P].[DeletedAt] IS NULL
		WHERE [C].[Id] = @UserId
			AND [C].[DeletedAt] IS NULL
			AND [CT].[ChannelId] = @ChatId
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT	DISTINCT [U].[Id] AS [UserId],[CT].[ChannelId] AS [ChatId],CM.[Message] AS [LastMessage],[CT].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],'' AS [BusinessName],[ProfilePicture],
			[CT].[IsActive],(SELECT COUNT(*) FROM [dbo].[ChannelMessages] CMM (NOLOCK) WHERE CMM.IsRead = 0 AND CMM.[ChannelId] = [CT].[ChannelId]) AS [TotalUnReadCount],[CT].[UserType]
			FROM [dbo].[Users] U (NOLOCK)
			LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [U].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId]
			WHERE [U].[DeletedAt] IS NULL
				AND [U].[Id] = @UserId
				AND [CT].[ChannelId] = @ChatId
	END

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
