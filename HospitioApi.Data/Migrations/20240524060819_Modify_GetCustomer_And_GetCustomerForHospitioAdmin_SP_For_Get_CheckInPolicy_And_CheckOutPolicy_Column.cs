using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetCustomer_And_GetCustomerForHospitioAdmin_SP_For_Get_CheckInPolicy_And_CheckOutPolicy_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomer
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomer]    Script Date: 24/05/2024 11:35:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomer]
@Id int=1
AS

 
SELECT 
( 
SELECT [Id]
      ,[BusinessName]
      ,[BusinessTypeId]
      ,[NoOfRooms]
      ,[TimeZone]
      ,[WhatsappCountry]
      ,[WhatsappNumber]
      ,[Cname]
      ,[ClientDoamin]
      ,[Email]
      ,[Messenger]
      ,[ViberCountry]
      ,[ViberNumber]
      ,[TelegramCounty]
      ,[TelegramNumber]
      ,[PhoneCountry]
      ,[PhoneNumber]
      ,[BusinessStartTime]
      ,[BusinessCloseTime]
      ,[DoNotDisturbGuestStartTime]
      ,[DoNotDisturbGuestEndTime]
      ,[StaffAlertsOffduty]
      ,[NoMessageToGuestWhileQuiteTime]
      ,[IncomingTranslationLangage]
      ,[NoTranslateWords]
      ,[ProductId]
      ,[SmsTitle]
      ,[IsActive]
	  ,[CurrencyCode]
	  ,[IsTwoWayComunication]
	  ,[Latitude]
	  ,[Longitude]
	  ,[PMSId]
	  ,ISNULL([GuidType], 2) AS UserType
      ,[CheckInPolicy]
      ,[CheckOutPolicy]
,JSON_QUERY(( 
SELECT [Id]
	  ,[CustomerId]
      ,[Name]
      ,[CreatedFrom]
      ,[IsActive]
	  ,CONVERT(NVARCHAR(36), [Guid]) AS CenturionLocationCode
	  ,ISNULL([GuidType], 2) as LocationType
FROM [dbo].[CustomerRoomNames] where CustomerId=m.Id
FOR JSON PATH
)) as [UpdateCustomerRoomNamesOuts]
FROM  [dbo].[Customers] m 
where Id = @Id
FOR JSON PATH )");
            #endregion

            #region GetCustomerForHospitioAdmin
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerForHospitioAdmin]    Script Date: 24/05/2024 11:36:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomerForHospitioAdmin] 
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
		   [M].[CheckOutPolicy]
    FROM [dbo].[Customers] M (NOLOCK)
        INNER JOIN [dbo].[CustomerUsers] CU (NOLOCK)
            ON [M].[Id] = [CU].[CustomerId]
               AND [CU].[DeletedAt] IS NULL
		LEFT JOIN [dbo].[Products] P (NOLOCK)
			ON [M].[ProductId] = [P].[Id]
    WHERE [M].[DeletedAt] IS NULL
          AND [M].[Id] = @Id
END");
            #endregion

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
