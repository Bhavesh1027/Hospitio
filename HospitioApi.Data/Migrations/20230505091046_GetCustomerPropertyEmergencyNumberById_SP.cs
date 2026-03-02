using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPropertyEmergencyNumberById_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyEmergencyNumberById]    Script Date: 05-05-2023 14:41:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create OR ALTER Procedure [dbo].[GetCustomerPropertyEmergencyNumberById]
@Id int=0
as
select * from CustomerPropertyEmergencyNumbers 
where DeletedAt is null 
and Id = @Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
