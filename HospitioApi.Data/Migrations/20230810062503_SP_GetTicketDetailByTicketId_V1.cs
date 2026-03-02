using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetTicketDetailByTicketId_V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetTicketDetailByTicketId]
(
    @CustomerId INT = 0,
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
					AND [TR].[CreatedFrom] <> @UserType
					AND ISNULL([TR].[IsRead], 0) = 0
		) AS [TotalUnReadCount] 
	FROM [dbo].[Tickets] T (NOLOCK)
	WHERE [T].[Id] = @TicketId
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
