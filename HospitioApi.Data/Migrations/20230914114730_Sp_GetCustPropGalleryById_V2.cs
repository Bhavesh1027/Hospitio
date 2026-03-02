using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Sp_GetCustPropGalleryById_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustPropGalleryById]    Script Date: 14/09/2023 4:24:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustPropGalleryById] 
(
	@CustomerPropertyInformationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	Declare @IsDeleted bit= 0;
	SELECT 
	       [Id] ,
		   JSON_VALUE(JsonData, '$.CustomerPropertyInformationId') AS [CustomerPropertyInformationId],
		   JSON_VALUE(JsonData, '$.PropertyImage') AS [PropertyImage],
		   JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
    FROM [dbo].[CustomerPropertyGalleries]
	     WHERE ISJSON(JsonData) = 1 
		       AND [DeletedAt] IS NULL
			   AND CustomerPropertyInformationId = @CustomerPropertyInformationId
    UNION ALL

    SELECT [Id],
	       [CustomerPropertyInformationId],
           [PropertyImage],
		   @IsDeleted AS [IsDeleted]
    FROM [dbo].[CustomerPropertyGalleries] (NOLOCK)
    WHERE [DeletedAt] IS NULL
	      AND JsonData IS NULL
          AND [CustomerPropertyInformationId] = @CustomerPropertyInformationId
          AND [IsActive] = 1
    ORDER BY [Id]
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
