using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetCustomerPermission_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerPermission
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPermissions]    Script Date: 20/10/2023 2:24:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[GetCustomerPermissions]
AS
BEGIN
 
 	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
		   [NormalizedName] AS [Value],
		   [IsEdit],
		   [IsView],
		   [IsReply],
		   [IsUpload],
		   [IsDownload],
		   [IsACtive]
    FROM [dbo].[CustomerPermissions] (NOLOCK)
	WHERE [DeletedAt] IS NULL

END
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
