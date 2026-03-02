using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerRoomNames_sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerRoomNames SP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomNames]    Script Date: 27-05-2023 13:58:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerRoomNames]
(
@CustomerId int = 1
)
as
select [Id],[Name] from dbo.CustomerRoomNames 
where DeletedAt is null 
AND CustomerId = @CustomerId
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
