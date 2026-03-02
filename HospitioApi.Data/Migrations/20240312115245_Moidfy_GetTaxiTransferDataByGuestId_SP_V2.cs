using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Moidfy_GetTaxiTransferDataByGuestId_SP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetTaxiTransferDataByGuestId
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetTaxiTransferDataByGuestId]    Script Date: 12/03/2024 5:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[GetTaxiTransferDataByGuestId]
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
        [RefundStatus]
    FROM 
        LatestRecords
    WHERE 
        RowNum = 1;
END
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
