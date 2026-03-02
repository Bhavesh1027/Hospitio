using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetAdminUsersRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetAdminUsers]    Script Date: 26-09-2023 10:59:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[GetAdminUsers]
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

     SELECT [Id],
           ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
		   [PhoneCountry],
		   [PhoneNumber]
    FROM [dbo].[Users] (NOLOCK)
    WHERE [DeletedAt] IS NULL AND [IsActive] = 1
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
