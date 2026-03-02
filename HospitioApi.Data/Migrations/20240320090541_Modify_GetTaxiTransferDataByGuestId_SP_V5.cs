using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetTaxiTransferDataByGuestId_SP_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetTaxiTransferDataByGuestId
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetTaxiTransferDataByGuestId]    Script Date: 20/03/2024 1:55:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetTaxiTransferDataByGuestId]
(
	@CustomerId INT = 0,
	@GuestId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

		SELECT
		    [Id], 
            [GuestId],
            [TransferId],
            [TransferStatus],
            [RefundAmount],
            [GRPaymentId],
            [GRPaymentDetails],
            [TransferJson],
            [RefundId],
            [RefundStatus],
			[FareAmount],
			[HospitioFareAmount],
			[ExtraDetailsJson]
        FROM 
            [dbo].[TaxiTransferGuestRequests] (NOLOCK)
        WHERE 
            [CustomerId] = @CustomerId
            AND [GuestId] = @GuestId
			ORDER BY CreatedAt DESC
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
