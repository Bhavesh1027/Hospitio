using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE SP_GetChatId
(
	@UserId INT = 0,
	@UserType VARCHAR(20),
	@ClaimUserId INT = 0,
	@ClaimUserType VARCHAR(20)
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON

	DECLARE @ChatId INT = 0

	IF EXISTS (SELECT * FROM [dbo].[ChannelUsers] (NOLOCK) WHERE [UserId] = @UserId AND [UserType] = @UserType AND [DeletedAt] IS NULL)
	BEGIN
		IF EXISTS(
			SELECT * FROM [dbo].[ChannelUsers] (NOLOCK) WHERE [UserId] = @ClaimUserId AND [UserType] = @ClaimUserType AND [DeletedAt] IS NULL
		)
		BEGIN
				SELECT @ChatId = [CU2].[ChannelId] FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
					INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId] AND [CU2].[DeletedAt] IS NULL 
						AND [CU2].[UserId] = @ClaimUserId 
						AND [CU2].[UserType] = @ClaimUserType
				WHERE [CU1].[DeletedAt] IS NULL
						AND [CU1].[UserId] = @UserId 
						AND [CU1].[UserType] = @UserType
		END
	END
	
	SELECT @ChatId AS [ChatId]

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
