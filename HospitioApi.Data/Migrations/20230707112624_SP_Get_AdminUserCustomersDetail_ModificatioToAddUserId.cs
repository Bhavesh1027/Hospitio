using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_Get_AdminUserCustomersDetail_ModificatioToAddUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_Get_AdminUserCustomersDetail]
(
	@CustomerId INT = 0,
	@UserType NVARCHAr(50) = NULL
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON

	IF(UPPER(LTRIM(RTRIM(@UserType))) = 'CUSTOMER')
	BEGIN
		SELECT	[U].[Id] AS UserId,[C].[BusinessName],NULL AS [FirstName],NULL AS [LastName],[C].[Email],NULL AS [ProfilePicture],[C].[PhoneCountry],[C].[PhoneNumber],[C].[IncomingTranslationLangage],[C].[NoOfRooms],[BT].[BizType],
			[P].[Name] AS [ServicePackageName],[C].[CreatedAt], 'CUSTOMER' AS [UserType] 
		FROM [dbo].[Customers] C (NOLOCK)
			INNER JOIN [dbo].[CustomerUsers] U ON C.Id  = U.CustomerId
			INNER JOIN [dbo].[BusinessTypes] BT (NOLOCK) 
				ON [C].[BusinessTypeId] = [BT].[Id] 
					AND [BT].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[Products] P (NOLOCK) 
				ON [P].[Id] = [C].[ProductId] 
					AND  [P].[DeletedAt] IS NULL
		WHERE [C].[DeletedAt] IS NULL
			AND [C].[Id] = @CustomerId 
	END
	ELSE IF(UPPER(LTRIM(RTRIM(@UserType))) = 'HOSPITIOUSER')
	BEGIN
		SELECT U.Id AS UserId,NULL AS [BusinessName],[U].[FirstName],[U].[LastName],[U].[Email],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],NULL AS [IncomingTranslationLangage],NULL AS [NoOfRooms],NULL AS [BizType],
		NULL AS [ServicePackageName],[U].[CreatedAt],'HOSPITIOUSER' AS [UserType] 
		FROM [dbo].[Users] U (NOLOCK)
		WHERE [U].[DeletedAt] IS NULL
			AND [U].[Id] = @CustomerId
	END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
