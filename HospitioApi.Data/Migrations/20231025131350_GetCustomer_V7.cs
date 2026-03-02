using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomer_V7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomer]    Script Date: 25-10-2023 18:12:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER       Procedure [dbo].[GetCustomer] 
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
	  ,[Latitude]
	  ,[Longitude]
	  ,ISNULL([GuidType], 2) AS UserType
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
