using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_Notification_Sps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetNotifications
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetNotifications]    Script Date: 13/10/2023 11:04:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetNotifications]-- 1 , 10 , 73 , 2
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
                    COUNT(*) OVER () as [FilteredCount]
             FROM [dbo].[Notifications] N WITH (NOLOCK)   
             WHERE [N].[DeletedAt] IS NULL
			       AND [N].[Id]  NOT IN (SELECT [NH].[NotificationId] FROM NotificationHistorys NH where [NH].[UserType] = 3 )
            )
        SELECT [Id],
               [Title],
               [Message],
               [CreatedAt],
               [FilteredCount]
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
                    COUNT(*) OVER () as [FilteredCount]
             FROM [dbo].[Notifications] N WITH (NOLOCK)
                 INNER JOIN [dbo].[NotificationHistorys] NH (NOLOCK)
                       ON [N].[Id] = [NH].[NotificationId]
                 INNER JOIN [dbo].[CustomerGuests] CG
				       ON [CG].[Id] = [NH].[UserId]
                 INNER JOIN [dbo].[CustomerReservations] CR
				       ON [CR].[Id] = [CG].[CustomerReservationId]
             WHERE [N].[DeletedAt] IS NULL
				   AND [NH].[UserType] = 3
				   AND [CR].[CustomerId] = @UserId
            )
        SELECT [Id],
               [Title],
               [Message],
               [CreatedAt],
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
               [FilteredCount]
        FROM FilteredNotifications
        ORDER BY [CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
	END
END");
            #endregion

            #region SP_UpdateNotificationRead
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateNotificationRead]    Script Date: 12/10/2023 8:03:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_UpdateNotificationRead] 
(
  @UserId INT = 0,
  @UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

	 DECLARE @TotalUnreadNotificationCount INT = 0

	IF EXISTS(
      SELECT * 
      FROM [dbo].[NotificationHistorys] NH (NOLOCK)
           INNER JOIN [dbo].[Notifications] N (NOLOCK)
	            ON [NH].[NotificationId] = [N].[Id] 
	               AND [N].[DeletedAt] IS NUll
	           WHERE [NH].[UserId] = @UserId
			         AND [NH].[UserType]= @UserType
			         AND [NH].[DeletedAt] IS NULL
	 )
	 BEGIN
	   UPDATE [dbo].[NotificationHistorys] 
	   SET [IsRead] = 1 
	   WHERE [UserId] = @UserId
	         AND UserType = @UserType
	         AND [DeletedAt] IS NULL
	   
	   SELECT  @TotalUnreadNotificationCount = COUNT([NH].[Id])
	   FROM [dbo].[NotificationHistorys] NH (NOLOCK)
	        INNER JOIN [dbo].[Notifications] N (NOLOCK)
			ON [N].[Id] = [NH].[NotificationId]
			   AND [N].[DeletedAt] IS NULL
            WHERE [NH].[DeletedAt] IS NULL
			       AND [NH].[UserId] = @UserId
				   AND [NH].[UserType]= @UserType
				   AND ISNULL([NH].[IsRead] , 0) = 0

      SELECT @TotalUnreadNotificationCount AS [TotalUnreadCount]

     END
  ELSE
	 BEGIN
	   SELECT 0 AS [TotalUnreadCount]
	 END
 
END");
            #endregion

            #region SP_GetCustomerForNotification
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetCustomerForNotification]    Script Date: 12/10/2023 8:04:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_GetCustomerForNotification] 
(
    @businessTypeId INT,
    @productId INT,
    @country nvarchar(max),
    @city nvarchar(max),
    @postalcode nvarchar(max)
)
AS 
BEGIN
    SELECT [dbo].[Customers].Id AS UserId
    FROM [dbo].[Customers]
    WHERE ([dbo].[Customers].BusinessTypeId = @businessTypeId OR ISNULL(@businessTypeId, 0) = 0)
        AND ([dbo].[Customers].ProductId = @productId OR ISNULL(@productId, 0) = 0)
        AND ([dbo].[Customers].Country = @country OR ISNULL(@country, '') = '')
        AND ([dbo].[Customers].City = @city OR ISNULL(@city, '') = '')
        AND ([dbo].[Customers].Postalcode = @postalcode OR ISNULL(@postalcode, '') = '')
		AND DeletedAt Is null
END");
            #endregion

            #region SP_GetGuestForNotification
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetGuestForNotification]    Script Date: 13/10/2023 10:02:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetGuestForNotification]
(
    @country nvarchar(max),
    @city nvarchar(max),
    @postalcode nvarchar(max),
	@customerId INT = 0
)
AS
BEGIN
 SELECT [CG].[Id] AS UserId
    FROM [dbo].[CustomerGuests] CG
	     INNER JOIN [dbo].[CustomerReservations] CR
		       ON [CR].[Id] = [CG].[CustomerReservationId]
    WHERE ([CG].[Country] = @country OR ISNULL(@country, '') = '')
          AND ([CG].[City] = @city OR ISNULL(@city, '') = '')
          AND ([CG].[Postalcode] = @postalcode OR ISNULL(@postalcode, '') = '')
		  AND [CR].[CustomerId] = @customerId
		  AND [CG].[DeletedAt] IS NULL
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
