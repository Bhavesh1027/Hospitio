using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetUsersByGroupIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetUsersByGroupId]    Script Date: 29-05-2023 09:41:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetUsersByGroupId]  
(
@GroupId int=0
)
as
SET NOCOUNT ON 
SET XACT_ABORT ON  
select dbo.Users.Id,ISNULL([FirstName],'') + ' ' + ISNULL([LastName],'') AS Name from dbo.Users where dbo.Users.GroupId = @GroupId and dbo.Users.DeletedAt is null and dbo.Users.IsActive = 1
                "
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
