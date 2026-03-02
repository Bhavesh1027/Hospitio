using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerRoomNamesByGuIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomNamesByGuId]    Script Date: 30-06-2023 18:41:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[GetCustomerRoomNamesByGuId] --  '54E03B65-C6BF-4BE3-9630-323026F97C97'
(
	@GuId UNIQUEIDENTIFIER = NULL
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [M].[Guid] AS LocationCode,
           [M].[Name]
    FROM [dbo].[CustomerRoomNames] M (NOLOCK)
    WHERE [M].[DeletedAt] IS NULL
          AND [M].[Guid] = @GuId
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
