using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GetCustomerPermissions_Sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerPermissions
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPermissions]    Script Date: 16/10/2023 3:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPermissions]
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
		   [IsDownload],
		   [IsACtive]
    FROM [dbo].[CustomerPermissions] (NOLOCK)
	WHERE [DeletedAt] IS NULL

END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
