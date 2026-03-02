using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GETUserDetailByChatId_newcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE SP_GETUserDetailByChatId
(
	@ChatId INT = 0,
	@Id INT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
    SET XACT_ABORT ON

	DECLARE @UserType NVARCHAR(100) 
	DECLARE @UserId INT

	SELECT @UserType = CU.[UserType],@UserId = [CU].[UserId] FROM [dbo].[ChannelUsers] (NOLOCK) CU
	WHERE [CU].[ChannelId] = @ChatId
	AND [CU].[UserId] <> @Id

	IF(@UserType = 'CustomerUser')
	BEGIN
		SELECT [C].[BusinessName],NULL AS[FirstName],NULL AS [LastName],[C].[Email],NULL AS [ProfilePicture],[C].[PhoneCountry],[C].[PhoneNumber],
			   [C].[IncomingTranslationLangage],[C].[NoOfRooms],[BT].[BizType],[P].[Name] AS [ServicePackageName],[C].[CreatedAt],
			   @UserType AS [UserType] ,@UserId AS [UserId], @ChatId AS [ChatId],[C].[IsActive],[C].[DeActivated]
		FROM [dbo].[Customers](NOLOCK) C 
			LEFT JOIN [dbo].[BusinessTypes] BT (NOLOCK) 
				ON [BT].[Id] = [C].[BusinessTypeId] 
					AND [BT].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[Products] P (NOLOCK) 
				ON [P].[Id] = [C].[ProductId] 
					AND  [P].[DeletedAt] IS NULL
		WHERE [C].[Id] = @UserId
			AND [C].[DeletedAt] IS NULL
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT	NULL AS [BusinessName],[U].[FirstName],[U].[LastName],[U].[Email],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],
				NULL AS [IncomingTranslationLangage],NULL AS [NoOfRooms],NULL AS [BizType],NULL AS [ServicePackageName],[U].[CreatedAt],
				@UserType AS [UserType] ,@UserId AS [UserId], @ChatId AS [ChatId],[U].[IsActive],[U].[DeActivated]
			FROM [dbo].[Users] U (NOLOCK)
			WHERE [U].[DeletedAt] IS NULL
				AND [U].[Id] = @UserId
	END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
