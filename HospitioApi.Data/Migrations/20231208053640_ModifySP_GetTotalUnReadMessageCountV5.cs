using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifySP_GetTotalUnReadMessageCountV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
CREATE OR ALTER    PROCEDURE [dbo].[SP_GetTotalUnReadMessageCount]   
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
					AND [CM].[Source] = 1
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
	ELSE IF ( @UserType = 4 OR @UserType = 5)
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
	ELSE IF ( @UserType = 2)
	BEGIN
		SELECT COUNT(DISTINCT([C].[Id])) AS [TotalUnreadCount]
		FROM [dbo].[Channels] C (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU 
				ON [C].[Id] = [CU].[ChannelId] 
					AND [CU].[UserId] = @UserId
					AND [CU].[UserType] = @ChatUserType
			INNER JOIN [dbo].[ChannelMessages] CM  
				ON  [CM].[ChannelId] = [C].[Id] 
            INNER JOIN [dbo].[CustomerUsers] CUS
			    ON [CUS].[Id] = [CU].[UserId]
				AND [CUS].[DeletedAt] IS NULL
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
		  WHERE ( [CUS].[CustomerLevelId] <> 1 AND [CM].[Source] <> 3)
                   OR ([CUS].[CustomerLevelId] = 1)
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
                INNER JOIN [dbo].[Users] U
				    ON [U].[Id] = [CU].[UserId]
					   AND [U].[DeletedAt] IS NULL
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
                WHERE ( [U].[UserLevelId] <> 1 AND [CM].[Source] <> 3)
                   OR ( [U].[UserLevelId] = 1)
	END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
