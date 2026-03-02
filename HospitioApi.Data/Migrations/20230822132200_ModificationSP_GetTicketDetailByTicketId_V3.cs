using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModificationSP_GetTicketDetailByTicketId_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER     PROCEDURE [dbo].[SP_GetTicketDetailByTicketId]
(
    @UserId INT = 0,
    @UserType INT = 0,
    @TicketId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;

	SELECT [T].[Id],([C].[Cname]) AS [CustomerName],[C].[Email],[CFB].[Logo] AS ProfilePicture,[T].[Title],[T].[Details],[T].[Priority],[T].[CloseDate],[T].[CreatedAt],
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
		INNER JOIN Customers  C
			ON C.Id = T.CustomerId
		INNER JOIN CustomerGuestsCheckInFormBuilders CFB
			ON CFB.CustomerId = T.CustomerId
	WHERE [T].[Id] = @TicketId
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
