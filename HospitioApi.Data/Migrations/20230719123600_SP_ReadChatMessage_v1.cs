using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_ReadChatMessage_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_ReadChatMessage]
(
	@ChatId INT = 0,
	@UserId INT = 0
)
AS
 BEGIN
	
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @LastMsgReadId INT = 0
	DECLARE @TotalUnreadCount INT = 0

	IF EXISTS (SELECT * FROM [dbo].[ChannelMessages](NOLOCK) CM 
				INNER JOIN [dbo].[ChannelUsers](NOLOCK) CU 
					ON [CU].[ChannelId] = [CM].[ChannelId] 
					AND [CU].[DeletedAt] IS NULL
				WHERE [CM].[ChannelId] = @ChatId 
				AND [CU].[UserId] = @UserId
				AND [CM].[MessageSenderId] <> @UserId
				AND [CU].[DeletedAt] IS NULL
		)
		BEGIN
			SELECT TOP(1) @LastMsgReadId = [CM].[Id] 
			FROM [dbo].[ChannelMessages](NOLOCK) CM 
				INNER JOIN [dbo].[ChannelUsers](NOLOCK) CU 
					ON [CU].[ChannelId] = [CM].[ChannelId] 
					AND [CU].[DeletedAt] IS NULL
			WHERE [CM].[ChannelId] = @ChatId 
				AND [CU].[UserId] = @UserId
				AND [CM].[MessageSenderId] <> @UserId
				AND [CU].[DeletedAt] IS NULL
				ORDER BY [CM].[Id] DESC

			IF(ISNULL(@LastMsgReadId,0) > 0)
			BEGIN
				UPDATE [dbo].[ChannelUsers] SET 
					[LastMessageReadId] = @LastMsgReadId, 
					[LastMessageReadTime] = GETUTCDATE()
				WHERE [ChannelId] = @ChatId
					AND [UserId] = @UserId
					AND [DeletedAt] IS NULL

				SELECT @TotalUnreadCount = COUNT([C].[Id]) 
				FROM [dbo].[Channels] C (NOLOCK)
					INNER JOIN [dbo].[ChannelUsers] CU 
						ON [C].[Id] = [CU].[ChannelId] 
						AND [CU].[UserId] = @UserId
					INNER JOIN [dbo].[ChannelMessages] CM  
						ON  [CM].[ChannelId] = [C].[Id] 
						AND [CM].[MessageSenderId] <> @UserId 
						AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 

				SELECT @ChatId AS [ChatId], @TotalUnreadCount AS [TotalUnreadCount]
			END
		END
		ELSE
		BEGIN
			SELECT 0 AS [ChatId],0 AS [TotalUnReadCount]
		END
 END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
