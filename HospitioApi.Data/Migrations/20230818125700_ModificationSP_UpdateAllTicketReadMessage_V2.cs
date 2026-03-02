using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModificationSP_UpdateAllTicketReadMessage_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_UpdateAllTicketReadMessage]
(
    @UserId INT = 0,
    @UserType INT = 0,
    @TicketId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    DECLARE @TotalUnreadCount INT = 0

    IF EXISTS
    (
        SELECT *
        FROM [dbo].[TicketReplies] TR (NOLOCK)
            INNER JOIN [dbo].[Tickets] T (NOLOCK)
                ON [T].[Id] = [TR].[TicketId]
                   AND [T].[DeletedAt] IS NULL
        WHERE [TR].[DeletedAt] IS NULL
              AND [TR].[TicketId] = @TicketId
			  AND (
					(
							[TR].[CreatedFrom] = @UserType
							AND [TR].[CreatedBy] <> @UserId
					)
					OR (
							[TR].[CreatedFrom] <> @UserType
					)
				)
    )
    BEGIN

        UPDATE [dbo].[TicketReplies]
        SET [IsRead] = 1
        WHERE [DeletedAt] IS NULL
              AND [TicketId] = @TicketId
              AND (
					(
							[CreatedFrom] = @UserType
							AND [CreatedBy] <> @UserId
					)
					OR (
							[CreatedFrom] <> @UserType
					)
				)

        SELECT @TotalUnreadCount = COUNT([TR].[TicketId])
        FROM [dbo].[TicketReplies] TR (NOLOCK)
            INNER JOIN [dbo].[Tickets] T (NOLOCK)
                ON [T].[Id] = [TR].[TicketId]
                   AND [T].[DeletedAt] IS NULL
        WHERE [TR].[DeletedAt] IS NULL
              AND [TR].[TicketId] = @TicketId
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

        SELECT @TicketId AS [TicketId],
               @TotalUnreadCount AS [TotalUnReadCount]

    END
    ELSE
    BEGIN
        SELECT 0 AS [TicketId],
               0 AS [TotalUnReadCount]
    END
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
