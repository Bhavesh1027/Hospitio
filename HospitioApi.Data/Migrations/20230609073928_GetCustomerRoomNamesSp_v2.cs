using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerRoomNamesSp_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomNames]    Script Date: 09-06-2023 12:52:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetCustomerRoomNames]
(
@CustomerId int = 1
)
AS
SELECT
	[Id],
	[Name],
	CASE WHEN EXISTS (
        SELECT *
        FROM [CustomerGuestAppBuilders]
        WHERE [CustomerRoomNameId] = rooms.Id AND  [CustomerId]= @CustomerId
    ) THEN 1 ELSE 0 END AS IsCompleted
FROM dbo.CustomerRoomNames rooms
WHERE DeletedAt is null 
AND CustomerId = @CustomerId
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
