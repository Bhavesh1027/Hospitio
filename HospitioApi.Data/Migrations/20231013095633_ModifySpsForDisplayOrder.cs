using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifySpsForDisplayOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerPropertyEmergencyNumbers

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyEmergencyNumbers]    Script Date: 10/13/2023 2:53:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER       PROCEDURE [dbo].[GetCustomerPropertyEmergencyNumbers] 
(
	@PropertyId INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

	;WITH DispalyOrederPropertyInfo
	AS (
	SELECT 
			[Id],
			JSON_VALUE(JsonData, '$.CustomerPropertyInformationId') AS [CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.PhoneCountry') AS [PhoneCountry],
			JSON_VALUE(JsonData, '$.PhoneNumber') AS [PhoneNumber],
			JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder]

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
			CEN.[DisplayOrder]
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
		  ORDER BY [DisplayOrder]
END");

            #endregion

            #region GetCustomerPropertyExtras

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyExtras]    Script Date: 10/13/2023 3:28:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyExtras]  --1
(
	@CustomerPropertyInformationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @ISDELETE BIT = 0;

	WITH Property_Extra AS (
		SELECT [Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.ExtraType') AS [ExtraType],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted],
			JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder]

		FROM CustomerPropertyExtras CE 
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			

		UNION ALL

		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,@ISDELETE As [IsDeleted]
		  ,CE.[DisplayOrder]
		FROM CustomerPropertyExtras CE 
		WHERE [DeletedAt] IS NULL
		AND JsonData IS NULL
		
	),
	Property_Extra_Detail AS
    (
		SELECT 
			[Id],
            [CustomerPropertyExtraId],
            JSON_VALUE(JsonData, '$.Description') AS [Description],
            JSON_VALUE(JsonData, '$.Link') AS [Link],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			

		UNION ALL

		SELECT  
			CED.[Id],
			CED.[CustomerPropertyExtraId],
			CED.[Description],
			CED.[Link],
			@ISDELETE As [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE [DeletedAt] IS NULL
		AND JsonData IS NULL
			
	)

	SELECT 
	(
		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,CE.[IsDeleted]
		  ,CE.[DisplayOrder]
		   ,JSON_QUERY((
			  Select  CED.[Id]
				  ,CED.[CustomerPropertyExtraId]
				  ,CED.[Description]
				  ,CED.[Link]
				  ,CED.[IsDeleted]
			  From Property_Extra_Detail CED With (NOLOCK)
			  WHERE CED.CustomerPropertyExtraId = CE.Id
			  AND CED.IsDeleted != 1
			  FOR JSON PATH
			  )) as [customerPropertyExtraDetailsOuts]
		FROM Property_Extra CE With (NOLOCK) where CE.CustomerPropertyInformationId = @CustomerPropertyInformationId
		AND CE.IsDeleted != 1
		ORDER BY [CE].[DisplayOrder]
		FOR JSON PATH
	) AS [CustomerPropertyInfoExtrasOuts]
    
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
