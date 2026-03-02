using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetPermissions_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetPermissions]    Script Date: 15-06-2023 10:24:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  Procedure [dbo].[GetPermissions]

AS

Select [Id]
      ,[Name]
	  From dbo.Permissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
