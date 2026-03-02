using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetProductNamesRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetProductNames]    Script Date: 19-05-2023 13:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetProductNames]

as
SET NOCOUNT ON 
SET XACT_ABORT ON 

select dbo.Products.Id,dbo.Products.Name from dbo.Products where DeletedAt is null and IsActive = 1
                "
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
