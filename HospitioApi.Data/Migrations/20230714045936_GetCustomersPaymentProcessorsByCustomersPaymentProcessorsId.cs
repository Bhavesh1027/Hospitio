using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomersPaymentProcessorsByCustomersPaymentProcessorsId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
CREATE OR ALTER PROCEDURE GetCustomersPaymentProcessorsById 
    @ID INT = 0
AS
BEGIN
    SELECT
        cpp.[Id],
        cpp.[CustomerId],
        cpp.[PaymentProcessorId],
        cpp.[GRPaymentServiceId],
        cpp.[GRWebhookURL],
        cpp.[IsActive],
        cpp.[GR3DSecureEnabled],
        cpp.[GRAcceptedCountries],
        cpp.[GRAcceptedCurrencies],
        cpp.[GRFields],
        cpp.[GRIsDeleted],
        cpp.[GRMerchantProfile],
        (
            SELECT
                PP.[Id],
                PP.[IsActive],
                PP.[GRCategory],
                PP.[GRGroup],
                PP.[GRID],
                PP.[GRIcon],
                PP.[GRName],
                (
                    SELECT
                        PPD.[Id],
                        PPD.[PaymentProcessorId],
                        PPD.[GRFields],
                        PPD.[GRSupportedCountries],
                        PPD.[GRSupportedCurrencies],
                        PPD.[GRSupportedFeatures],
                     
                        PPD.[IsActive]
                    FROM
                        [dbo].[PaymentProcessorsDefinations] AS PPD
                    WHERE
                        PPD.PaymentProcessorId = PP.Id
                    FOR JSON AUTO
                ) AS PaymentProcessorsDefinations
            FROM
                [dbo].[PaymentProcessors] AS PP
            WHERE
                cpp.PaymentProcessorId = PP.Id
            FOR JSON AUTO
        ) AS PaymentProcessors
    FROM
        [dbo].[CustomerPaymentProcessors] AS cpp
    WHERE
        cpp.Id = @ID
    FOR JSON AUTO;
END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
