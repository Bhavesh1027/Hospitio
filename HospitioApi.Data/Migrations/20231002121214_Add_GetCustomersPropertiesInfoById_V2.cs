using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GetCustomersPropertiesInfoById_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomersPropertiesInfoById]    Script Date: 2/10/2023 11:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomersPropertiesInfoById] 
(
	@Id INT = 0,
	@UserType INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON
 
	SELECT
    CI.Id,
    CI.CustomerId,
    MAX(CASE WHEN [key] = 'CustomerGuestAppBuilderId' THEN [value] END) AS [CustomerGuestAppBuilderId],
    MAX(CASE WHEN [key] = 'WifiUsername' THEN [value] END) AS [WifiUsername],
    MAX(CASE WHEN [key] = 'WifiPassword' THEN [value] END) AS [WifiPassword],
    MAX(CASE WHEN [key] = 'Overview' THEN [value] END) AS [Overview],
    MAX(CASE WHEN [key] = 'CheckInPolicy' THEN [value] END) AS [CheckInPolicy],
    MAX(CASE WHEN [key] = 'TermsAndConditions' THEN [value] END) AS [TermsAndConditions],
    MAX(CASE WHEN [key] = 'Street' THEN [value] END) AS [Street],
    MAX(CASE WHEN [key] = 'StreetNumber' THEN [value] END) AS [StreetNumber],
    MAX(CASE WHEN [key] = 'City' THEN [value] END) AS [City],
    MAX(CASE WHEN [key] = 'Postalcode' THEN [value] END) AS [Postalcode],
    MAX(CASE WHEN [key] = 'Country' THEN [value] END) AS [Country],
    MAX(CASE WHEN [key] = 'IsActive' THEN [value] END) AS [IsActive]
    FROM CustomerPropertyInformations CI
    CROSS APPLY OPENJSON(JsonData)
         WHERE ISJSON(JsonData) = 1
               AND [DeletedAt] IS NULL
               AND (@UserType = 2)
               AND Id = @Id
        GROUP BY CI.Id, CI.CustomerId

		UNION ALL
		
		SELECT
			CI.Id,
			CI.CustomerId,
			CI.[CustomerGuestAppBuilderId],
			CI.[WifiUsername],
			CI.[WifiPassword],
			CI.[Overview],
			CI.[CheckInPolicy],
			CI.[TermsAndConditions],
			CI.[Street],
			CI.[StreetNumber],
			CI.[City],
			CI.[Postalcode],
			CI.[Country],
			CI.[IsActive]
		FROM CustomerPropertyInformations CI
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
			AND Id = @Id
END  ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
