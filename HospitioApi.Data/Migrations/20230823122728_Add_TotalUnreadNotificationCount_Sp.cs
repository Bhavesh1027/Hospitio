using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_TotalUnreadNotificationCount_Sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateNotificationRead]    Script Date: 23/08/2023 5:52:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [dbo].[SP_UpdateNotificationRead] 
(
  @CustomerId INT = 0
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
	           WHERE [NH].[CustomerId] = @CustomerId
			          AND [NH].[DeletedAt] IS NULL
	 )
	 BEGIN
	   UPDATE [dbo].[NotificationHistorys] 
	   SET [IsRead] = 1 
	   WHERE [CustomerId] = @CustomerId
	         AND [DeletedAt] IS NULL
	   
	   SELECT  @TotalUnreadNotificationCount = COUNT([NH].[Id])
	   FROM [dbo].[NotificationHistorys] NH (NOLOCK)
	        INNER JOIN [dbo].[Notifications] N (NOLOCK)
			ON [N].[Id] = [NH].[NotificationId]
			   AND [N].[DeletedAt] IS NULL
            WHERE [NH].[DeletedAt] IS NULL
			       AND [NH].[CustomerId] = @CustomerId
				   AND ISNULL([NH].[IsRead] , 0) = 0

      SELECT @TotalUnreadNotificationCount AS [TotalUnreadCount]

     END
  ELSE
	 BEGIN
	   SELECT 0 AS [TotalUnreadCount]
	 END
 
END 
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
