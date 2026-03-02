using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetDisplayOrder_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetDisplayOrder SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetDisplayOrder]    Script Date: 12-06-2023 14:42:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetDisplayOrder]
(
@ReferenceId int = 0,
@ScreenName int = 0
)
as
select * from ScreenDisplayOrderAndStatuses 
where DeletedAt is null 
ANd RefrenceId = @ReferenceId
AND ScreenName = @ScreenName");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
