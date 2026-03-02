using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CreateSP_GetTotalUnreadQACountAndSP_GetTotalUnreadNotificationCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER     PROCEDURE [dbo].[SP_GetTotalUnreadQACount]
(
    @UserId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;

	SELECT COUNT(Id) AS [TotalUnReadCount]
	FROM QuestionAnswers 
		WHERE Id NOT IN (
			SELECT QuestionAnswerId
			FROM QuestionAnswersRead
			WHERE UserId = @UserId
		);
						
END
");

            migrationBuilder.Sql(@"CREATE OR ALTER       PROCEDURE [dbo].[SP_GetTotalUnreadNotificationCount]
(
	@CustomerId INT = 0
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
      			       AND [NH].[CustomerId] = 1
      				   AND ISNULL([NH].[IsRead] , 0) = 0
						
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
