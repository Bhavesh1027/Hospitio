using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetTotalUnreadTicketCount_modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER     PROCEDURE [dbo].[SP_GetTotalUnreadTicketCount]
(
    @UserId INT = 0,
    @UserType INT = 0,
	@CustomerId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;

	DECLARE @GroupId INT = (SELECT ISNULL(GroupId,0) FROM Users WHERE Id = @UserId);
	DECLARE @UserLevelId INT = (SELECT ISNULL(UserLevelId,0) FROM Users WHERE Id = @UserId);

	SELECT  COUNT(DISTINCT([T].[Id])) AS [TotalUnReadCount]
	FROM [dbo].[Tickets] T (NOLOCK)
		INNER JOIN [dbo].[TicketReplies] TR (NOLOCK)
			ON [T].[Id] = [TR].[TicketId]
               AND [T].[DeletedAt] IS NULL
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
		WHERE  ( [T].[CustomerId] = @CustomerId OR @CustomerId = 0 )
			AND ([T].[CSAgentId] IS NULL AND [T].[GroupId] IS NULL AND @UserLevelId = 1
				OR
				[T].[CSAgentId] = @UserId
				OR
				([T].[GroupId] = @GroupId AND [T].[CSAgentId] IS NULL)
			) 
						
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
