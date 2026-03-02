using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GetAllTaxiTransferData_SP_V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetAllTaxiTransferData
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetAllTaxiTransferData]    Script Date: 29/03/2024 11:46:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllTaxiTransferData]
(
	@SearchString NVARCHAR(50) = NULL,
	@PageNo INT = 1,
    @PageSize INT = 10
)
AS
BEGIN
	
     SET NOCOUNT ON
     SET XACT_ABORT ON;

     SELECT (SELECT [TT].[Id],
	        [TT].[CustomerId],
			[C].[BusinessName] AS [CustomerName],
			[TT].[GuestId],
			[CG].[Firstname] AS [GuestFirstName],
			[CG].[Lastname] AS [GuestLastName],
			[CR].[ReservationNumber] AS [ReservationId],
			[TT].[TransferId],
			[TT].[TransferStatus],
			[TT].[GRPaymentId],
			[TT].[HospitioFareAmount],
			[TT].[FareAmount],
			[TT].[RefundStatus],
			[TT].[RefundAmount],
			[TT].[RefundId],
			JSON_VALUE([TT].[ExtraDetailsJson], '$.Luggage') AS [Luggage],
            JSON_VALUE([TT].[ExtraDetailsJson], '$.passenger') AS [Passenger],
			JSON_VALUE([TT].[ExtraDetailsJson], '$.FromLocation') AS [FromLocation],
            JSON_VALUE([TT].[ExtraDetailsJson], '$.ToLocation') AS [ToLocation],
		    JSON_VALUE([TT].[TransferJson], '$.data.attributes.from_location.description') AS [FormLocationDescription],
            JSON_VALUE([TT].[TransferJson], '$.data.attributes.to_location.description') AS [ToLocationDescription],
            JSON_VALUE([TT].[TransferJson], '$.data.attributes.pickup_date_time') AS [PickUpDateTime],
            JSON_VALUE([TT].[TransferJson], '$.data.attributes.booked_date_time') AS [BookedDateTime],
			JSON_VALUE([TT].[TransferJson], '$.data.attributes.cancelled_at') AS [CancelledAt],
			JSON_VALUE([TT].[TransferJson], '$.data.attributes.passenger.count') AS [PassengerCount],
			JSON_VALUE([TT].[TransferJson], '$.data.attributes.passenger.name') AS [PassengerName],
			JSON_VALUE([TT].[TransferJson], '$.data.attributes.passenger.email') AS [PassengerEmail],
			JSON_VALUE([TT].[TransferJson], '$.data.attributes.passenger.mobile') AS [PassengerMobile],
		    JSON_VALUE([TT].[GRPaymentDetails], '$.currency') AS [Currency],
		    JSON_VALUE([TT].[GRPaymentDetails], '$.created_at') AS [GRCreateAt],
		    JSON_VALUE([TT].[GRPaymentDetails], '$.payment_service_transaction_id') AS [PaymentServiceTransactionId],
		    JSON_VALUE([TT].[GRPaymentDetails], '$.payment_method.method') AS [PaymentMethod],
		    JSON_VALUE([TT].[GRPaymentDetails], '$.payment_service_refund_id') AS [PaymentServiceRefeunfId]
	 FROM [dbo].[TaxiTransferGuestRequests] TT (NOLOCK)
	 INNER JOIN [dbo].[Customers] C (NOLOCK)
	       ON [C].[Id] = [TT].[CustomerId]
     INNER JOIN [dbo].[CustomerGuests] CG (NOLOCK)
	       ON [CG].[Id] = [TT].[GuestId]
     INNER JOIN [dbo].[CustomerReservations] CR (NOLOCK)
	       ON [CR].[Id] = [CG].[CustomerReservationId]
     WHERE [TT].[DeletedAt] IS NULL
	 AND 
			(
				[C].[BusinessName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[CG].[FirstName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[CG].[LastName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR
				[CG].[Firstname]+ ' ' +[CG].[Lastname] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR
				[TT].[TransferStatus] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR
				JSON_VALUE([TT].[ExtraDetailsJson], '$.FromLocation') + ' ' + JSON_VALUE([TT].[ExtraDetailsJson], '$.ToLocation') LIKE '%' + ISNULL(@SearchString,'') + '%'
			)
			ORDER BY [TT].[CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
	 FOR JSON PATH
	 ) AS [TransferJsonData]
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
