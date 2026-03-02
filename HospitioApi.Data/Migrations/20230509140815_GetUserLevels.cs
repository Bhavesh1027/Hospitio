using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetUserLevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetUserLevels]    Script Date: 09-05-2023 19:38:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create OR ALTER Procedure [dbo].[GetUserLevels]
@IsHospitioUserType bit = 'true'
as
select * from UserLevels 
where DeletedAt is null 
and IsHospitioUserType = @IsHospitioUserType
and LevelName != 'Super Admin' ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
