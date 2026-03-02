using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetTickets_modification_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[GetTickets]
(
    @Status INT = 0,
    @Priority INT = 0,
    @CustomerId INT = 0,
    @CSAgentId INT = 0,
    @FromCreate DATETIME = NULL,
    @ToCreate DATETIME = NULL,
    @FromClose DATETIME = NULL,
    @ToClose DATETIME = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @ShortBy TINYINT = 0,     -- on CreateAt 1=Short by Date,2=Short By Month,3=Short By Year (modify in feture if need)
    @CreatedFrom TINYINT = 0, -- 1 Fro Radfy ,2 for Customer,
    @ApplyPagination BIT = 1,
	@UserId INT = 1,
	@UserType VARCHAR(50) = 1
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

    DECLARE @query NVARCHAR(MAX) = '';
	DECLARE @GroupId INT = (SELECT ISNULL(GroupId,0) FROM Users WHERE Id = @UserId);
	DECLARE @UserLevelId INT = (SELECT ISNULL(UserLevelId,0) FROM Users WHERE Id = @UserId);

	SET @query += ' WITH CTE AS (SELECT [Ticket].[Id],[Ticket].[CustomerId],[dbo].[Customers].[BusinessName],[dbo].[Customers].[Cname] AS [CustomerName],[dbo].[Customers].[Email],[Ticket].[Title],[Ticket].[Details],[Ticket].[Priority],[Ticket].[Duedate],[Ticket].[Status],[Ticket].[CloseDate], [Ticket].[CreatedFrom],CASE WHEN [Ticket].[CreatedFrom] = 2 AND [Ticket].[CSAgentId] IS NULL THEN NULL ELSE CONCAT(ISNULL([dbo].[Users].[FirstName],''''),SPACE(1),ISNULL([dbo].[Users].[LastName],'''')) END AS [CSAgentName],[Ticket].[IsActive],[Ticket].[CreatedAt],[dbo].[CustomerGuestsCheckInFormBuilders].[Logo] AS [ProfilePicture],COUNT(*) OVER () AS [FilteredCount],[Ticket].[MaxCreatedDate],
							(SELECT COUNT([TR].[Id]) AS [TotalUnReadCount] FROM [dbo].[TicketReplies] TR (NOLOCK)
									WHERE [Ticket].[Id] = [TR].[TicketId]
										AND [TR].[DeletedAt] IS NULL
										AND ISNULL([TR].[IsRead], 0) = 0
										AND (
											(
													[TR].[CreatedFrom] = '+CAST(@UserType AS VARCHAR(50))+'
													AND [TR].[CreatedBy] <> '+CAST(@UserId AS VARCHAR(50))+'
											)
											OR (
													[TR].[CreatedFrom] <> '+CAST(@UserType AS VARCHAR(50))+'
											)
										)
							) AS [TotalUnReadCount],
							ROW_NUMBER() OVER (PARTITION BY [Ticket].Id ORDER BY [Ticket].[CreatedAt] DESC) AS RowNum
					FROM (
							SELECT	[dbo].[Tickets].[Id],[dbo].[Tickets].[CustomerId],MAX(ISNULL(TR.CreatedAt, [dbo].[Tickets].[CreatedAt])) AS MaxCreatedDate,[dbo].[Tickets].[Title],[dbo].[Tickets].[Details],
									[dbo].[Tickets].[CSAgentId],[dbo].[Tickets].[GroupId],[dbo].[Tickets].CreatedFrom,[dbo].[Tickets].[DeletedAt],[dbo].[Tickets].[Status],[dbo].[Tickets].[Priority],[dbo].[Tickets].[Duedate],[dbo].[Tickets].[CloseDate],
									[dbo].[Tickets].[IsActive],[dbo].[Tickets].[CreatedAt]
							FROM Tickets
								LEFT JOIN [TicketReplies] TR ON [dbo].[Tickets].[Id] = TR.TicketId AND [TR].[DeletedAt] IS NULL AND [Tickets].[DeletedAt] IS NULL
							GROUP BY [dbo].[Tickets].[Id],[dbo].[Tickets].[CustomerId],[dbo].[Tickets].[Title],[dbo].[Tickets].[Details],[dbo].[Tickets].[CSAgentId],[dbo].[Tickets].[GroupId],[dbo].[Tickets].CreatedFrom,[dbo].[Tickets].[DeletedAt],[dbo].[Tickets].[Status],[dbo].[Tickets].[Priority],
									[dbo].[Tickets].[Duedate],[dbo].[Tickets].[CloseDate],[dbo].[Tickets].[IsActive],[dbo].[Tickets].[CreatedAt]
						) Ticket
							INNER JOIN [dbo].[Customers] WITH (NOLOCK)
								ON [Ticket].[CustomerId] = [dbo].[Customers].[Id] AND [dbo].[Customers].[DeletedAt] IS NULL
							LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] (NOLOCK)
								ON [dbo].[Customers].[Id] = [dbo].[CustomerGuestsCheckInFormBuilders].[CustomerId] AND [dbo].[CustomerGuestsCheckInFormBuilders].[DeletedAt] IS NULL
							LEFT JOIN [dbo].[Users] WITH (NOLOCK)
								ON [Ticket].[CSAgentId] = [dbo].[Users].[Id] 
									AND [dbo].[Users].[DeletedAt] IS NULL AND [Ticket].[CreatedFrom] <> 2'

					IF @FromCreate IS NOT NULL
						SET @query += ' LEFT JOIN [dbo].[TicketReplies] TR ON [TR].[TicketId] = [Ticket].[Id] AND [TR].[DeletedAt] IS NULL AND NOT EXISTS (SELECT 1 FROM Tickets T2 LEFT JOIN TicketReplies TR2 ON TR2.TicketId = T2.Id  WHERE Ticket.Id = T2.Id AND TR2.CreatedAt > TR.CreatedAt)'

					SET @query += ' WHERE ( [Ticket].[DeletedAt] IS NULL )
						AND (
							[Ticket].[CSAgentId] IS NULL AND [Ticket].[GroupId] IS NULL AND '+CAST(@UserLevelId AS VARCHAR(50))+' = 1  
							OR
							[Ticket].[CSAgentId] = '+CAST(@UserId AS VARCHAR(50))+'  
							OR
							([Ticket].[GroupId] = '+CAST(@GroupId AS VARCHAR(50))+' AND [Ticket].[CSAgentId] IS NULL)  
						)
						AND ( [Ticket].[Status] = '''+ CAST(@Status AS VARCHAR(50)) +''' OR '''+ CAST(@Status AS VARCHAR(50)) +''' = 0 )
						AND ( [Ticket].[Priority] = '''+ CAST(@Priority AS VARCHAR(50)) +''' OR '''+ CAST(@Priority AS VARCHAR(50)) +''' = 0 )
						AND ( [Ticket].[CustomerId] = '''+ CAST(@CustomerId AS VARCHAR(50)) +''' OR '''+ CAST(@CustomerId AS VARCHAR(50)) +''' = 0 )
						AND ( [Ticket].[CSAgentId] = '''+ CAST(@CSAgentId AS VARCHAR(50)) +''' OR '''+ CAST(@CSAgentId AS VARCHAR(50)) +''' = 0 )
						AND ( [Ticket].[CreatedFrom] = '''+ CAST(@CreatedFrom AS VARCHAR(50)) +''' OR '''+ CAST(@CreatedFrom AS VARCHAR(50)) +''' = 0 )'

					IF @FromCreate IS NOT NULL
						SET @query += ' AND (CAST([Ticket].[CreatedAt] AS DATE) >= '''+ CAST(CAST(@FromCreate AS DATE) AS NVARCHAR(30))+''' OR CAST([TR].[CreatedAt] AS DATE) >= '''+ CAST(CAST(@FromCreate AS DATE) AS NVARCHAR(30))+''')'
	
					IF @ToCreate IS NOT NULL
						SET @query += ' AND (CAST([Ticket].[CreatedAt] AS DATE) <= '''+ CAST(CAST(@ToCreate AS DATE) AS NVARCHAR(30))+''' OR CAST([TR].[CreatedAt] AS DATE) <= '''+ CAST(CAST(@ToCreate AS DATE) AS NVARCHAR(30))+''')'

					IF @FromCreate IS NOT NULL
						SET @query += ' OR (CAST([TR].[CreatedAt] AS DATE) >= '''+ CAST(CAST(@FromCreate AS DATE) AS NVARCHAR(30))+''' AND CAST([TR].[CreatedAt] AS DATE) <= '''+ CAST(CAST(@ToCreate AS DATE) AS NVARCHAR(30))+''')'
	
					IF @FromClose IS NOT NULL
						SET @query += ' AND  CAST([Ticket].[CloseDate] AS DATE) >= '''+ CAST(CAST(@FromClose AS DATE) AS NVARCHAR(30))+''''
	
					IF @ToClose IS NOT NULL
						SET @query += ' AND (CAST([Ticket].[CloseDate] AS DATE) <= '''+ CAST(CAST(@ToClose AS DATE) AS NVARCHAR(30))+''')'
	
					IF (@ApplyPagination = 1)
						BEGIN
							DECLARE @orderASC VARCHAR(5) = ''
							DECLARE @pagingNo INT = 0
							SET @pagingNo  = @PageSize * (@PageNo - 1)

							IF (@ShortBy = 0)
								SET @orderASC = 'ASC'
							ELSE
								SET @orderASC = 'DESC'
	
							SET @query +=' ORDER BY '
							
							IF(CAST(@ShortBy AS VARCHAR(50)) = '1')    
								SET @query += ' [Ticket].[MaxCreatedDate] '    
							ELSE IF(CAST(@ShortBy AS VARCHAR(50)) = '2')    
								SET @query += ' MONTH([Ticket].[MaxCreatedDate]) ' 
							ELSE IF(cast(@ShortBy AS VARCHAR(50)) = '3')    
								SET @query += ' YEAR([Ticket].[MaxCreatedDate]) '
							ELSE
								SET @query += '[Ticket].[MaxCreatedDate]'
	
							SET @query += ' '+@orderASC+' OFFSET '+ CAST(@pagingNo AS NVARCHAR(10)) +' ROWS FETCH NEXT '+ CAST(@PageSize AS NVARCHAR(10)) +' ROWS ONLY '

							SET @query += ') select * from CTE where RowNum = 1'

						END
						print @query
	EXEC(@query)   
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
