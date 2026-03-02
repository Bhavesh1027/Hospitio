using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_Sp_GetCustomerPropertyEmergencyNumbers_V6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyEmergencyNumbers]    Script Date: 10/16/2023 12:34:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER    PROCEDURE [dbo].[GetCustomerPropertyEmergencyNumbers] --2
(
	@PropertyId INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

	DECLARE @ISDELETE BIT = 0;
	;WITH DispalyOrederPropertyInfo
	AS (
	SELECT 
			[Id],
			JSON_VALUE(JsonData, '$.CustomerPropertyInformationId') AS [CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.PhoneCountry') AS [PhoneCountry],
			JSON_VALUE(JsonData, '$.PhoneNumber') AS [PhoneNumber],
			JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]

		FROM CustomerPropertyEmergencyNumbers CEN
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND CustomerPropertyInformationId = @PropertyId

		UNION ALL

		SELECT 
			CEN.[Id],
			CEN.[CustomerPropertyInformationId],
			CEN.[Name],
			CEN.[PhoneCountry],
			CEN.[PhoneNumber],
			CEN.[DisplayOrder],
			@ISDELETE As [IsDeleted]
		FROM CustomerPropertyEmergencyNumbers CEN
		WHERE [DeletedAt] IS NULL
			AND JsonData IS NULL
			AND CustomerPropertyInformationId = @PropertyId
	)
	    SELECT [Id],
               [CustomerPropertyInformationId],
               [Name],
               [PhoneCountry],
               [PhoneNumber],
			   [DisplayOrder]
         FROM  DispalyOrederPropertyInfo
		 WHERE IsDeleted != 1
		  ORDER BY [DisplayOrder]
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
