using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CreateSP_GetTotalUnreadTicketCount_V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetTotalUnreadTicketCount]
(
    @UserId INT = 0,
    @UserType INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;

	SELECT  COUNT(DISTINCT([T].[Id])) AS [TotalUnReadCount]
	FROM [dbo].[Tickets] T (NOLOCK)
		INNER JOIN [dbo].[TicketReplies] TR (NOLOCK)
			ON [T].[Id] = [TR].[TicketId]
               AND [T].[DeletedAt] IS NULL
			   AND ISNULL([TR].[IsRead], 0) = 0
			   AND (
					(
							[TR].[CreatedFrom] = @UserType
							AND [TR].[CreatedBy] <> @UserId
					)
					OR (
							[TR].[CreatedFrom] <> @UserType
					)
				)
END
");

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetTicketDetailByTicketId]
(
    @UserId INT = 0,
    @UserType INT = 0,
    @TicketId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;

	SELECT [T].[Id],[T].[Title],[T].[Details],[T].[CreatedAt],
		(SELECT COUNT([TR].[Id]) AS [TotalUnReadCount] FROM [dbo].[TicketReplies] TR (NOLOCK)
				WHERE [T].[Id] = [TR].[TicketId]
					AND [TR].[DeletedAt] IS NULL
					AND ISNULL([TR].[IsRead], 0) = 0
					AND (
					(
							[TR].[CreatedFrom] = @UserType
							AND [TR].[CreatedBy] <> @UserId
					)
					OR (
							[TR].[CreatedFrom] <> @UserType
					)
				)
		) AS [TotalUnReadCount] 
	FROM [dbo].[Tickets] T (NOLOCK)
	WHERE [T].[Id] = @TicketId
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
