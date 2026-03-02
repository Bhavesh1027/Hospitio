using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetTicketReplyUnReadMessageCount_SP_UpdateAllTicketReadMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetTicketReplyUnReadMessageCount]
(
    @Id INT = 0,
    @UserType INT = 0,
    @TicketId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT COUNT([TR].[Id]) AS [TotalUnReadCount]
    FROM [dbo].[TicketReplies] TR (NOLOCK)
        INNER JOIN [dbo].[Tickets] T (NOLOCK)
            ON [T].[Id] = [TR].[TicketId]
               AND [T].[DeletedAt] IS NULL
    WHERE [TR].[TicketId] = @TicketId
          AND [TR].[CreatedFrom] <> @UserType
          AND [T].[CustomerId] = @Id
          AND ISNULL([TR].[IsRead], 0) = 0
END");

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_UpdateAllTicketReadMessage]
(
    @Id INT = 0,
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
                   AND [T].[CustomerId] = @Id
                   AND [T].[DeletedAt] IS NULL
        WHERE [TR].[DeletedAt] IS NULL
              AND [TR].[TicketId] = @TicketId
              AND [TR].[CreatedFrom] <> @UserType
    )
    BEGIN

        UPDATE [dbo].[TicketReplies]
        SET [IsRead] = 1
        WHERE [DeletedAt] IS NULL
              AND [TicketId] = @TicketId
              AND [CreatedFrom] <> @UserType

        SELECT @TotalUnreadCount = COUNT([TR].[TicketId])
        FROM [dbo].[TicketReplies] TR (NOLOCK)
            INNER JOIN [dbo].[Tickets] T (NOLOCK)
                ON [T].[Id] = [TR].[TicketId]
                   AND [T].[CustomerId] = @Id
                   AND [T].[DeletedAt] IS NULL
        WHERE [TR].[DeletedAt] IS NULL
              AND [TR].[TicketId] = @TicketId
              AND [TR].[CreatedFrom] <> @UserType
              AND ISNULL([TR].[IsRead], 0) = 0

        SELECT @TicketId AS [TicketId],
               @TotalUnreadCount AS [TotalUnReadCount]

    END
    ELSE
    BEGIN
        SELECT 0 AS [TicketId],
               0 AS [TotalUnReadCount]
    END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
