using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modified_GetTicketsSP_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetTickets]
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
    @ApplyPagination BIT = 0
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

    DECLARE @query NVARCHAR(MAX) = ''
	
	SET @query += ' SELECT	[dbo].[Tickets].[Id], [dbo].[Tickets].[CustomerId], [dbo].[Customers].[BusinessName], [dbo].[Customers].[Cname] as [CustomerName], [dbo].[Customers].[Email], [dbo].[Tickets].[Title], [dbo].[Tickets].[Details], [dbo].[Tickets].[Priority], [dbo].[Tickets].[Duedate], [dbo].[Tickets].[Status], [dbo].[Tickets].[CloseDate], [dbo].[Tickets].[CreatedFrom],
							CASE WHEN [dbo].[Tickets].[CreatedFrom] = 2 AND [dbo].[Tickets].[CSAgentId] IS NULL THEN NULL ELSE CONCAT(ISNULL([dbo].[Users].[FirstName],''''),SPACE(1),ISNULL([dbo].[Users].[LastName],'''')) END AS [CSAgentName],
							[dbo].[Tickets].[IsActive], [dbo].[Tickets].[CreatedAt], [dbo].[CustomerGuestsCheckInFormBuilders].[Logo] AS [ProfilePicture], COUNT(*) OVER () AS [FilteredCount],
							(SELECT COUNT([TR].[Id]) AS [TotalUnReadCount] FROM [dbo].[TicketReplies] TR (NOLOCK)
									WHERE [dbo].[Tickets].[Id] = [TR].[TicketId]
										AND [TR].[DeletedAt] IS NULL
										AND [TR].[CreatedFrom] <> '+CAST(@CreatedFrom AS VARCHAR(50))+'
										AND ISNULL([TR].[IsRead], 0) = 0
							) AS [TotalUnReadCount]
					FROM [dbo].[Tickets] WITH (NOLOCK)
							INNER JOIN [dbo].[Customers] WITH (NOLOCK)
								ON [dbo].[Tickets].[CustomerId] = [dbo].[Customers].[Id] AND [dbo].[Customers].[DeletedAt] IS NULL
							LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] (NOLOCK)
								ON [dbo].[Customers].[Id] = [dbo].[CustomerGuestsCheckInFormBuilders].[CustomerId] AND [dbo].[CustomerGuestsCheckInFormBuilders].[DeletedAt] IS NULL
							LEFT JOIN [dbo].[Users] WITH (NOLOCK)
								ON [dbo].[Tickets].[CSAgentId] = [dbo].[Users].[Id] 
									AND [dbo].[Users].[DeletedAt] IS NULL AND [dbo].[Tickets].[CreatedFrom] <> 2
					WHERE ( [dbo].[Tickets].[DeletedAt] IS NULL )
						AND ( [dbo].[Tickets].[Status] = '''+ CAST(@Status AS VARCHAR(50)) +''' OR '''+ CAST(@Status AS VARCHAR(50)) +''' = 0 )
						AND ( [dbo].[Tickets].[Priority] = '''+ CAST(@Priority AS VARCHAR(50)) +''' OR '''+ CAST(@Priority AS VARCHAR(50)) +''' = 0 )
						AND ( [dbo].[Tickets].[CustomerId] = '''+ CAST(@CustomerId AS VARCHAR(50)) +''' OR '''+ CAST(@CustomerId AS VARCHAR(50)) +''' = 0 )
						AND ( [dbo].[Tickets].[CSAgentId] = '''+ CAST(@CSAgentId AS VARCHAR(50)) +''' OR '''+ CAST(@CSAgentId AS VARCHAR(50)) +''' = 0 )
						AND ( [dbo].[Tickets].[CreatedFrom] = '''+ CAST(@CreatedFrom AS VARCHAR(50)) +''' OR '''+ CAST(@CreatedFrom AS VARCHAR(50)) +''' = 0 )
					'
					IF @FromCreate IS NOT NULL
						SET @query += ' AND  CAST([dbo].[Tickets].[CreatedAt] AS DATE) >= '''+ CAST(CAST(@FromCreate AS DATE) AS NVARCHAR(30))+''''
	
					IF @ToCreate IS NOT NULL
						SET @query += ' AND  CAST([dbo].[Tickets].[CreatedAt] AS DATE) <= '''+ CAST(CAST(@ToCreate AS DATE) AS NVARCHAR(30))+''''
	
					IF @FromClose IS NOT NULL
						SET @query += ' AND  CAST([dbo].[Tickets].[CloseDate] AS DATE) >= '''+ CAST(CAST(@FromClose AS DATE) AS NVARCHAR(30))+''''
	
					IF @ToClose IS NOT NULL
						SET @query += ' AND  CAST([dbo].[Tickets].[CloseDate] AS DATE) <= '''+ CAST(CAST(@ToClose AS DATE) AS NVARCHAR(30))+''''
	
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
								SET @query += ' [dbo].[Tickets].[CreatedAt] '    
							ELSE IF(CAST(@ShortBy AS VARCHAR(50)) = '2')    
								SET @query += ' MONTH([dbo].[Tickets].[CreatedAt]) ' 
							ELSE IF(cast(@ShortBy AS VARCHAR(50)) = '3')    
								SET @query += ' YEAR([dbo].[Tickets].[CreatedAt]) '
							ELSE
								SET @query += '[dbo].[Tickets].[Id]'
	
							SET @query += ' '+@orderASC+' OFFSET '+ CAST(@pagingNo AS NVARCHAR(10)) +' ROWS FETCH NEXT '+ CAST(@PageSize AS NVARCHAR(10)) +' ROWS ONLY '

						END
	EXEC(@query)   

END
");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
