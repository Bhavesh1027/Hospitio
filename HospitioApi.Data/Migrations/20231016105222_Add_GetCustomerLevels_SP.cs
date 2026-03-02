using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GetCustomerLevels_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerLevels]    Script Date: 16/10/2023 4:09:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomerLevels]
(
   @IsCustomerUserType BIT = 'true'
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [LevelName],
           [NormalizedLevelName],
           [IsCustomerUserType],
           [IsActive]
    FROM [dbo].[CustomerLevels] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [IsCustomerUserType] = @IsCustomerUserType 
          AND [LevelName] != 'Super Admin'
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
