using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetDisplayOrder_SP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetDisplayOrder]    Script Date: 21-06-2023 10:24:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetDisplayOrder]
(
@ReferenceId int = 0,
@ScreenName int = 0
)
as
select * from ScreenDisplayOrderAndStatuses 
where DeletedAt is null 
ANd RefrenceId = @ReferenceId
AND ScreenName = @ScreenName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
