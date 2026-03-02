using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetTicketById_SP_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetTicketById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [t].[Id],
                   [t].[Title],
                   [t].[Details],
                   [t].[CloseDate],
                   [t].[CreatedFrom],
                   [t].[CSAgentId],
                   CASE
                       WHEN [t].[CSAgentId] IS NULL THEN
                           ''
                       ELSE
                           ISNULL([U].[FirstName], '') + SPACE(1) + ISNULL([U].[LastName], '')
                   END AS [CSAgentName],
                   [t].[CustomerId],
                   ISNULL([C].[BusinessName], '') as [CustomerName],
                   [t].[Duedate],
                   [t].[Priority],
                   [t].[Status],
                   [t].[CreatedAt],
                   JSON_QUERY(
                   (
                       SELECT [TR].[Id],
                              [TR].[Reply],
                              [TR].[TicketId],
                              [TR].[CreatedAt],
                              [TR].[CreatedBy],
                              [TR].[CreatedFrom],
                              CONCAT_WS(
                                           ' ',
                                           COALESCE([U].[FirstName], [CU].[FirstName]),
                                           COALESCE([U].[LastName], [CU].[LastName])
                                       ) AS [UserName]
                       FROM [dbo].[TicketReplies] TR (NOLOCK)
                           LEFT JOIN [dbo].[Users] U (NOLOCK)
                               ON [TR].[CreatedBy] = 1
                                  AND [U].[Id] = [TR].[CreatedFrom]
                           LEFT JOIN [dbo].[CustomerUsers] CU (NOLOCK)
                               ON [TR].[CreatedBy] = 2
                                  AND [CU].[Id] = [TR].[CreatedFrom]
                       WHERE [TR].[TicketId] = [t].[Id]
                             AND [TR].[DeletedAt] IS NULL
						ORDER BY [TR].[CreatedAt] DESC
                       FOR JSON PATH
                   )
                             ) as [Replies]
            FROM [dbo].[Tickets] t (NOLOCK)
                LEFT JOIN [dbo].[Users] U (NOLOCK)
                    ON [U].[Id] = [t].[CSAgentId]
                INNER JOIN [dbo].[Customers] C (NOLOCK)
                    ON [C].[Id] = [t].[CustomerId]
            WHERE [t].[DeletedAt] IS NULL
                  AND [t].[Id] = @Id 
				  ORDER BY [t].[CreatedAt] DESC
            FOR JSON PATH
        );

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
