using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_Get_AdminUserCustomersDetail_Modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_Get_AdminUserCustomersDetail]
(
	@CustomerId INT = 0
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON

	SELECT	[C].[BusinessName],[C].[PhoneCountry],[C].[PhoneNumber],[C].[Email],[C].[IncomingTranslationLangage],[C].[NoOfRooms],[BT].[BizType],
			[C].[ProductId],[P].[Name],[C].[CreatedAt] 
	FROM [dbo].[Customers] C (NOLOCK)
		INNER JOIN [dbo].[BusinessTypes] BT (NOLOCK) 
			ON [C].[BusinessTypeId] = [BT].[Id] 
				AND [BT].[DeletedAt] IS NULL
		LEFT JOIN [dbo].[Products] P (NOLOCK) 
			ON [P].[Id] = [C].[ProductId] 
				AND  [P].[DeletedAt] IS NULL
	WHERE [C].[DeletedAt] IS NULL
		AND [C].[Id] = @CustomerId 

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
