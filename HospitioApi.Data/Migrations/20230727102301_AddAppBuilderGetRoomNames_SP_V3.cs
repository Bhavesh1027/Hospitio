using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddAppBuilderGetRoomNames_SP_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomNames]    Script Date: 27-07-2023 15:36:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerRoomNames] -- 1
(
	@CustomerId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SELECT [R].[Id],[R].[Name],[G].[IsWork],[BT].BizType 
    FROM [dbo].[CustomerRoomNames] [R] (NOLOCK)
	INNER JOIN [dbo].[CustomerGuestAppBuilders] [G](NOLOCK)
	ON [G].CustomerRoomNameId = [R].Id
	INNER JOIN [dbo].[Customers] [C]
	ON [G].CustomerId = [C].Id
	INNER JOIN [dbo].[BusinessTypes] [BT]
	ON [C].BusinessTypeId = [BT].Id
    WHERE [R].[DeletedAt] IS NULL
          AND [G].[CustomerId] = @CustomerId
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
