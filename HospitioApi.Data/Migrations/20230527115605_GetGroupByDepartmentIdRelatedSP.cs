using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetGroupByDepartmentIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetGroupsByDepartmentId]    Script Date: 27-05-2023 17:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetGroupsByDepartmentId] 
(
@DepartmentId int=0
)
as
SET NOCOUNT ON 
SET XACT_ABORT ON  
select dbo.Groups.Id,dbo.Groups.Name from dbo.Groups where dbo.Groups.DepartmentId = @DepartmentId and dbo.Groups.DeletedAt is null and dbo.Groups.IsActive = 1
                "
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
