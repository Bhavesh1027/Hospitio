using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerForHospitioAdminRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerForHospitioAdmin]    Script Date: 24-08-2023 11:43:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerForHospitioAdmin] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [M].[Id],
           [M].[BusinessName],
           [M].[BusinessTypeId],
           [M].[NoOfRooms],
           [M].[TimeZone],
           [M].[WhatsappCountry],
           [M].[WhatsappNumber],
           [M].[Cname],
           [M].[ClientDoamin],
           [CU].[Email],
           [M].[Messenger],
           [M].[ViberCountry],
           [M].[ViberNumber],
           [M].[TelegramCounty],
           [M].[TelegramNumber],
           [M].[PhoneCountry],
           [M].[PhoneNumber],
           [M].[BusinessStartTime],
           [M].[BusinessCloseTime],
           [M].[DoNotDisturbGuestStartTime],
           [M].[DoNotDisturbGuestEndTime],
           [M].[StaffAlertsOffduty],
           [M].[NoMessageToGuestWhileQuiteTime],
           [M].[IncomingTranslationLangage],
           [M].[NoTranslateWords],
           [M].[ProductId],
           [M].[IsActive],
		   [M].[Country],
           [CU].[FirstName],
           [CU].[LastName],
           [CU].[Title],
           [CU].[ProfilePicture],
           [CU].[UserName]
    FROM [dbo].[Customers] M (NOLOCK)
        INNER JOIN [dbo].[CustomerUsers] CU (NOLOCK)
            ON [M].[Id] = [CU].[CustomerId]
               AND [CU].[DeletedAt] IS NULL
    WHERE [M].[DeletedAt] IS NULL
          AND [M].[Id] = @Id
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
