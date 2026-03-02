using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetTaxiTransferDataByGuestId_SP_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetTaxiTransferDataByGuestId
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetTaxiTransferDataByGuestId]    Script Date: 14/03/2024 3:16:55 PM ******/
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

    WITH LatestRecords AS (
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
			[ExtraDetailsJson],
            ROW_NUMBER() OVER (PARTITION BY [TransferId] ORDER BY Id DESC) AS RowNum
        FROM 
            [dbo].[TaxiTransferGuestRequests] (NOLOCK)
        WHERE 
            [CustomerId] = @CustomerId
            AND [GuestId] = @GuestId
    )
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
        LatestRecords
    WHERE 
        RowNum = 1;
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
