using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetNotificationsSP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetNotifications]    Script Date: 01-06-2023 10:41:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetNotifications]
(
@PageNo Int=1,
@PageSize Int=10,
@CustomerId int=0
)

AS 
BEGIN
SET NOCOUNT ON;
SET XACT_ABORT ON

IF @CustomerId != 0
BEGIN
        ;WITH FilteredNotifications AS
        (
            SELECT Notifications.[Id],
                [Title],
                [Message],
                Notifications.[CreatedAt],
                COUNT(*) OVER () as FilteredCount
            FROM Notifications WITH (NOLOCK)
            INNER JOIN [NotificationHistorys] ON dbo.Notifications.Id = NotificationHistorys.NotificationId
            WHERE Notifications.DeletedAt IS NULL
            AND (dbo.NotificationHistorys.CustomerId = @CustomerId)
        )

        SELECT [Id],
            [Title],
            [Message],
            [CreatedAt],
            FilteredCount
        FROM FilteredNotifications
        ORDER BY CreatedAt DESC
        OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY;
    END
    ELSE
    BEGIN
        ;WITH AllNotifications AS
        (
            SELECT [Id],
                [Title],
                [Message],
                [CreatedAt],
                COUNT(*) OVER () as FilteredCount
            FROM Notifications WITH (NOLOCK)
            WHERE Notifications.DeletedAt IS NULL
        )

        SELECT [Id],
            [Title],
            [Message],
            [CreatedAt],
            FilteredCount
        FROM AllNotifications
        ORDER BY CreatedAt DESC
        OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY;
    END
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
