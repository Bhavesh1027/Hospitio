using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_ReadChatMessage_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[SP_ReadChatMessage]    Script Date: 12-12-2023 18:15:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[SP_ReadChatMessage]
(
	 @ChatId INT = 0,
	 @UserId INT = 0,
	 @UserType INT = 0
)
AS
BEGIN
	
SET NOCOUNT ON;
SET XACT_ABORT ON;

	DECLARE @LastMsgReadId INT = 0
	DECLARE @TotalUnreadCount INT = 0

	DECLARE @ChatUserType VARCHAR(50)

   IF(@UserType = 1 )
   BEGIN
	SET @ChatUserType = 'HospitioUser'
   END
   ELSE IF(@UserType = 2)
   BEGIN
	SET @ChatUserType = 'CustomerUser'
   END
   ELSE IF(@UserType=3)
   BEGIN
	SET @ChatUserType = 'CustomerGuest'
   END
   ELSE IF(@UserType=4)
   BEGIN
    SET @ChatUserType = 'AnonymousUser'
   END
   ELSE IF(@UserType=5)
   BEGIN
    SET @ChatUserType = 'ChatWidgetUser'
   END


	IF EXISTS (SELECT * FROM [dbo].[ChannelMessages](NOLOCK) CM 
				INNER JOIN [dbo].[ChannelUsers](NOLOCK) CU 
				ON [CU].[ChannelId] = [CM].[ChannelId] 
				   AND [CU].[DeletedAt] IS NULL
				WHERE [CM].[ChannelId] = @ChatId AND [CU].[DeletedAt] IS NULL
				AND (
						(
							[CM].[MessageSender] = @UserType AND [CM].[MessageSenderId] <> @UserId
						)
					OR (
							[CM].[MessageSender] <> @UserType
						)
					)
		)
		BEGIN
			SELECT TOP(1) @LastMsgReadId = [CM].[Id] 
			FROM [dbo].[ChannelMessages](NOLOCK) CM 
				INNER JOIN [dbo].[ChannelUsers](NOLOCK) CU 
					ON [CU].[ChannelId] = [CM].[ChannelId] 
					AND [CU].[DeletedAt] IS NULL
			WHERE [CM].[ChannelId] = @ChatId 
				AND [CU].[DeletedAt] IS NULL
				AND (
						(
							[CM].[MessageSender] = @UserType AND [CM].[MessageSenderId] <> @UserId
						)
					OR (
							[CM].[MessageSender] <> @UserType
						)
					)
				ORDER BY [CM].[Id] DESC

			IF(ISNULL(@LastMsgReadId,0) > 0)
			BEGIN
				UPDATE [dbo].[ChannelUsers] SET 
					[LastMessageReadId] = @LastMsgReadId, 
					[LastMessageReadTime] = GETUTCDATE()
				WHERE [ChannelId] = @ChatId
					AND [DeletedAt] IS NULL
					AND UserType = @ChatUserType 
					AND UserId = @UserId

				SELECT @TotalUnreadCount = COUNT([CM].[Id])  
				FROM [dbo].[ChannelMessages] CM 
					INNER JOIN [dbo].[ChannelUsers] CU ON [CU].[ChannelId] = [CM].[ChannelId]
					AND (
							(
								[CU].[UserType] = @ChatUserType AND [CU].[UserId] <> @UserId
							)
						OR (
								[CU].[UserType] <> @ChatUserType
							)
						)
				WHERE [CM].[ChannelId] = @ChatId
				   -- AND [CU].[UserId] = @UserId
					AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
					AND (
							(
								[CM].[MessageSender] = @UserType AND [CM].[MessageSenderId] <> @UserId
							)
						OR (
								[CM].[MessageSender] <> @UserType
							)
						)

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
