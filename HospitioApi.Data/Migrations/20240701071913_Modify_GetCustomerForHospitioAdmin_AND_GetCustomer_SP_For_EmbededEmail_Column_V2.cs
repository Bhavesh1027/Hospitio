using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
	public partial class Modify_GetCustomerForHospitioAdmin_AND_GetCustomer_SP_For_EmbededEmail_Column_V2 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			#region GetCustomerForHospitioAdmin
			migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerForHospitioAdmin]    Script Date: 01-07-2024 11:08:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER    PROCEDURE [dbo].[GetCustomerForHospitioAdmin] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [M].[Id],
		   CONVERT(NVARCHAR(36), [M].[Guid]) AS UserUniqueID,
		   [M].[PMSId],
           [M].[PMSAPIAuthUsername],
	       [M].[PMSAPIAuthPassword],
           [M].[BusinessName],
           [M].[BusinessTypeId],
           [M].[NoOfRooms],
           [M].[TimeZone],
           [M].[WhatsappCountry],
           [M].[WhatsappNumber],
           [M].[Cname],
           [M].[ClientDoamin],
           [M].[Email],
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
		   [M].[Country],
           [M].[ProductId],
           [M].[IsActive],
           [CU].[FirstName],
           [CU].[LastName],
           [CU].[Title],
           [CU].[ProfilePicture],
           [CU].[UserName],
		   [M].[SmsTitle],
		   [M].[CurrencyCode],
		   [M].[Longitude],
		   [M].[Latitude],
		   [M].[IsTwoWayComunication],
		   [P].[Name] AS ProductName,
		   [CU].[Email] AS CustomerUserEmail,
		   [M].[CheckInPolicy],
		   [M].[CheckOutPolicy],
		   [M].[EmbededEmail],
		   [M].[SubscriptionExpirationDate]
    FROM [dbo].[Customers] M (NOLOCK)
        INNER JOIN [dbo].[CustomerUsers] CU (NOLOCK)
            ON [M].[Id] = [CU].[CustomerId]
               AND [CU].[DeletedAt] IS NULL
		LEFT JOIN [dbo].[Products] P (NOLOCK)
			ON [M].[ProductId] = [P].[Id]
    WHERE [M].[DeletedAt] IS NULL
          AND [M].[Id] = @Id
END
");
			#endregion
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
