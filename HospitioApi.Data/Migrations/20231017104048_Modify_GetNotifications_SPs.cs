using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetNotifications_SPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetNotifications
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetNotifications]    Script Date: 17/10/2023 3:40:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetNotifications]
(
    @PageNo INT = 1,
    @PageSize INT = 10,
    @UserId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

	IF @UserType = 1
	BEGIN
	 ;WITH AllNotifications
         AS (SELECT [N].[Id],
                    [N].[Title],
                    [N].[Message],
                    [N].[CreatedAt],
					'HospitioUser' AS [MessageType],
                    COUNT(*) OVER () as [FilteredCount]
             FROM [dbo].[Notifications] N WITH (NOLOCK)   
             WHERE [N].[DeletedAt] IS NULL
			       AND [N].[Id]  IN (SELECT [NH].[NotificationId] FROM NotificationHistorys NH where [NH].[UserType] = 2 )
            )
        SELECT [Id],
               [Title],
               [Message],
               [CreatedAt],
               [FilteredCount],
			   [MessageType]
        FROM AllNotifications
        ORDER BY [CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
	END
	ELSE IF @UserType = 2
	BEGIN
	;WITH FilteredNotifications
         AS (SELECT [N].[Id],
                    [N].[Title],
                    [N].[Message],
                    [N].[CreatedAt],
					'HospitioUser' AS [MessageType],
                    COUNT(*) OVER () as [FilteredCount]
             FROM [dbo].[Notifications] N WITH (NOLOCK)
                 INNER JOIN [dbo].[NotificationHistorys] NH(NOLOCK)
                     ON [N].[Id] = [NH].[NotificationId]
             WHERE [N].[DeletedAt] IS NULL
                   AND [NH].[UserId] = @UserId
				   AND [NH].[UserType] = @UserType

				   UNION ALL

			 SELECT [N].[Id],
                    [N].[Title],
                    [N].[Message],
                    [N].[CreatedAt],
					'CustomerUser' AS [MessageType],
                    COUNT(*) OVER () as [FilteredCount]
             FROM [dbo].[Notifications] N WITH (NOLOCK)
			 WHERE [N].[Id] IN
			    ( SELECT [NN].[Id] FROM  [dbo].[Notifications] NN
                 INNER JOIN [dbo].[NotificationHistorys] NH (NOLOCK)
                       ON [NN].[Id] = [NH].[NotificationId]
                 INNER JOIN [dbo].[CustomerGuests] CG
				       ON [CG].[Id] = [NH].[UserId]
                 INNER JOIN [dbo].[CustomerReservations] CR
				       ON [CR].[Id] = [CG].[CustomerReservationId]
                 WHERE [NN].[DeletedAt] IS NULL
				   AND [NH].[UserType] = 3
				   AND [CR].[CustomerId] = @UserId
				   )
            )
        SELECT [Id],
               [Title],
               [Message],
               [CreatedAt],
			   [MessageType],
               COUNT(*) OVER () as [FilteredCount]
        FROM FilteredNotifications
        ORDER BY [CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
	END
	ELSE
	BEGIN
	;WITH FilteredNotifications
         AS (SELECT [Notifications].[Id],
                    [Title],
                    [Message],
                    [Notifications].[CreatedAt],
					'CustomerUser' AS [MessageType],
                    COUNT(*) OVER () as [FilteredCount]
             FROM [dbo].[Notifications] WITH (NOLOCK)
                 INNER JOIN [dbo].[NotificationHistorys] (NOLOCK)
                     ON [dbo].[Notifications].[Id] = [NotificationHistorys].[NotificationId]
             WHERE [Notifications].[DeletedAt] IS NULL
                   AND [dbo].[NotificationHistorys].[UserId] = @UserId
				   AND [dbo].[NotificationHistorys].[UserType] = @UserType
            )
        SELECT [Id],
               [Title],
               [Message],
               [CreatedAt],
               [FilteredCount],
			   [MessageType]
        FROM FilteredNotifications
        ORDER BY [CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
	END
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
