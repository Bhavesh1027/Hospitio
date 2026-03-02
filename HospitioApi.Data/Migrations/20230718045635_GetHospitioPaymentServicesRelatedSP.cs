using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetHospitioPaymentServicesRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetHospitioPaymentServices]    Script Date: 18-07-2023 10:12:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[GetHospitioPaymentServices]
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

    SELECT RP.[Id] AS HospitioPaymentProcessorId,RP.[IsActive], PP.[GRCategory], PP.[GRGroup], PP.[GRIcon], PP.[GRName]
    FROM [HospitioPaymentProcessors] RP
    INNER JOIN [PaymentProcessors] PP ON RP.PaymentProcessorId = PP.Id
    WHERE RP.[DeletedAt] IS NULL
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
