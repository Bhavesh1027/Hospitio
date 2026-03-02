using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPropertyEmergencyNumbers_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerPropertyEmergencyNumbers SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyEmergencyNumbers]    Script Date: 29-05-2023 20:24:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetCustomerPropertyEmergencyNumbers]
@PropertyId int =0
as
select * from CustomerPropertyEmergencyNumbers 
where DeletedAt is null 
AND CustomerPropertyInformationId = @PropertyId");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
