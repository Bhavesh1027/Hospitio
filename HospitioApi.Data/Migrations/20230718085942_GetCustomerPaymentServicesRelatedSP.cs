using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPaymentServicesRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPaymentServices]    Script Date: 18-07-2023 14:20:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[GetCustomerPaymentServices]
@CustomerId int=1
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

    SELECT CP.[Id] AS CustomerPaymentProcessorId,CP.[IsActive], PP.[GRCategory], PP.[GRGroup], PP.[GRIcon], PP.[GRName]
    FROM [CustomerPaymentProcessors] CP
    INNER JOIN [PaymentProcessors] PP ON CP.PaymentProcessorId = PP.Id
    WHERE CP.[DeletedAt] IS NULL and CP.CustomerId = @CustomerId
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
