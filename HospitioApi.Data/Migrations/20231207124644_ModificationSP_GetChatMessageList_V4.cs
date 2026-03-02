using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModificationSP_GetChatMessageList_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
CREATE OR ALTER   PROCEDURE [dbo].[SP_GetChatMessageList]
(
    @ChatId INT = 0,
    @PageNo INT = 1,
    @PageSize INT = 10,
	@UserType NVARCHAR(50) ,
	@UserId INT = 0
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
			WHEN MessageSender = 5  THEN '''+'ChatWidgetUser'+'''
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

	IF(@UserType = 'HospitioUser' AND ( SELECT [U].[UserLevelId] FROM Users U WHERE U.Id = @UserId ) NOT IN (1) )
	BEGIN
	  SET @query += ' AND [CM].[Source] NOT IN (3) '
	END

	IF(@UserType = 'CustomerUser' AND ( SELECT [CU].[CustomerLevelId] FROM CustomerUsers CU WHERE CU.Id = @UserId ) NOT IN (1))
	BEGIN
	  SET @query += ' AND [CM].[Source] NOT IN (3) ' 
	END

	IF(@UserType = 'ChatWidgetUser')
	BEGIN
	 SET @query += ' AND [CM].[Source] =  1'
	END

	SET @query += ' 	ORDER BY [CM].[CreatedAt] DESC OFFSET CAST(''' + CAST(@PageSize AS NVARCHAR(MAX))  + ''' AS INT ) * (  CAST(''' + CAST(@PageNo AS NVARCHAR(MAX)) + ''' AS INT)  - 1) ROWS FETCH NEXT CAST(''' + CAST(@PageSize AS NVARCHAR(MAX))  + ''' AS INT ) ROWS ONLY
	FOR JSON PATH 
	)'

    EXEC sp_executesql @query

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
