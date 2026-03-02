using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SP_GetTotalUnReadMessageCount_AND_SP_GetChatMessageList_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetChatMessageList
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatMessageList]    Script Date: 3/11/2023 5:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetChatMessageList] -- 1078 , 1 , 20 , 'HospitioUser'
(
    @ChatId INT = 0,
    @PageNo INT = 1,
    @PageSize INT = 10,
	@UserType NVARCHAR(50) 
)
AS
BEGIN

    DECLARE @query NVARCHAR(MAX) = ''
    
	SET @query = '
	SELECT (
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
		   [CM].[RequestType],
		   CASE 
			WHEN MessageSender = 1  THEN '''+'HospitioUser'+'''
			WHEN MessageSender = 2  THEN '''+'CustomerUser'+'''
			WHEN MessageSender = 3  THEN '''+'CustomerGuest'+'''
			WHEN MessageSender = 4  THEN '''+'AnonymousUser'+'''
			END AS [UserType],
		   CASE 
			WHEN [CM].[RequestType] = 1 THEN [GR].[Status] 
			WHEN [CM].[RequestType] = 2 THEN [ER].[Status]
			END AS [RequestStatus]
	FROM [dbo].[ChannelMessages] CM (NOLOCK)
		LEFT JOIN GuestRequests GR
	    ON [CM].[RequestId] = [GR].[Id]
		LEFT JOIN EnhanceStayItemExtraGuestRequests ER
			ON [CM].[RequestId] = [ER].[Id]
	 WHERE [CM].[DeletedAt] IS NULL
          AND [CM].[ChannelId] = CAST(''' + CAST(@ChatId AS  NVARCHAR(MAX))  + ''' AS INT )'
	IF(@UserType = 'CustomerGuest')
	BEGIN
	 SET @query += ' AND [CM].[Source] =  1'
	END

	SET @query += ' 	ORDER BY [CM].[CreatedAt] DESC OFFSET CAST(''' + CAST(@PageSize AS NVARCHAR(MAX))  + ''' AS INT ) * (  CAST(''' + CAST(@PageNo AS NVARCHAR(MAX)) + ''' AS INT)  - 1) ROWS FETCH NEXT CAST(''' + CAST(@PageSize AS NVARCHAR(MAX))  + ''' AS INT ) ROWS ONLY
	FOR JSON PATH 
	)'

    EXEC sp_executesql @query

END");
            #endregion

            #region SP_GetTotalUnReadMessageCount
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetTotalUnReadMessageCount]    Script Date: 3/11/2023 6:55:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetTotalUnReadMessageCount]   
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
	ELSE IF ( @UserType = 4 )
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
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
