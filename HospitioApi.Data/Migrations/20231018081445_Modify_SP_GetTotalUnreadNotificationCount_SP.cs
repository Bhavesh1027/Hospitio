using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SP_GetTotalUnreadNotificationCount_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetTotalUnreadNotificationCount
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetTotalUnreadNotificationCount]    Script Date: 18/10/2023 12:15:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_GetTotalUnreadNotificationCount]
(
	@UserId INT = 0,
	@UserType INT =0
)
AS
BEGIN

    SET NOCOUNT ON;

	SELECT COUNT([NH].[Id]) AS TotalUnreadCount
      	   FROM [dbo].[NotificationHistorys] NH (NOLOCK)
      	        INNER JOIN [dbo].[Notifications] N (NOLOCK)
      			ON [N].[Id] = [NH].[NotificationId]
      			   AND [N].[DeletedAt] IS NULL
                  WHERE [NH].[DeletedAt] IS NULL
      			       AND [NH].[UserId] = @UserId
					   AND [NH].[UserType] = @UserType
      				   AND ISNULL([NH].[IsRead] , 0) = 0
						
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
