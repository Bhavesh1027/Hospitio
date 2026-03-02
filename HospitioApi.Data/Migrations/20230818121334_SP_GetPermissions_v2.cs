using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetPermissions_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetPermissions]
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
		   [NormalizedName],
		   [IsEdit],
		   [IsView],
		   [IsReply],
		   [IsUpload],
		   [IsSend],
		   [IsACtive]
    FROM [dbo].[Permissions] (NOLOCK)
	WHERE [DeletedAt] IS NULL
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
