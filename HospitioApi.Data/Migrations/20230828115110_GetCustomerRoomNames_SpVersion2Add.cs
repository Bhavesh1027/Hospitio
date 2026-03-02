using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerRoomNames_SpVersion2Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomNames]    Script Date: 28/08/2023 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER  PROCEDURE [dbo].[GetCustomerRoomNames] -- 1
(
	@CustomerId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

  SELECT [R].[Id], [R].[Name], isnull([G].[IsWork], 3) AS IsWork, [BT].BizType
    FROM [dbo].[CustomerRoomNames] [R] (NOLOCK)
    INNER JOIN [dbo].[Customers] [C]
        ON [R].[CustomerId] = [C].[Id]
    LEFT JOIN [dbo].[CustomerGuestAppBuilders] [G] (NOLOCK)
        ON [G].CustomerRoomNameId = [R].Id
    LEFT JOIN [dbo].[BusinessTypes] [BT]
        ON [C].BusinessTypeId = [BT].Id
    WHERE [R].[DeletedAt] IS NULL
    AND ([G].[CustomerId] = @CustomerId OR [C].[Id] = @CustomerId)
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
