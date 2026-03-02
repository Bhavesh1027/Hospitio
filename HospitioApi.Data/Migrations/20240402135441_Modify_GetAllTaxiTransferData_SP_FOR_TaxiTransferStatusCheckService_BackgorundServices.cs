using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetAllTaxiTransferData_SP_FOR_TaxiTransferStatusCheckService_BackgorundServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetAllTaxiTransferData
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetAllTaxiTransferData]    Script Date: 2/04/2024 3:59:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetAllTaxiTransferData]
(
	@SearchString NVARCHAR(100) = null,
	@PageNo INT = 1,
    @PageSize INT = 10,
	@CustomerId INT = 0,
	@GuestId INT =0,
	@ToCreate DATETIME = NULL,
    @FromCreate DATETIME = NULL,
	@StatusCheck BIT= 0
)
AS
BEGIN
	
     SET NOCOUNT ON
     SET XACT_ABORT ON;
	 
	DECLARE @query NVARCHAR(MAX) = '';

	SET @query +=   ' SELECT(SELECT [TT].[Id],
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
									([TT].[HospitioFareAmount]-[TT].[FareAmount]) AS [MarkUpAmount],
									JSON_VALUE([TT].[ExtraDetailsJson], ''$.Luggage'') AS [Luggage],
									JSON_VALUE([TT].[ExtraDetailsJson], ''$.passenger'') AS [Passenger],
									JSON_VALUE([TT].[ExtraDetailsJson], ''$.FromLocation'') AS [FromLocation],
									JSON_VALUE([TT].[ExtraDetailsJson], ''$.ToLocation'') AS [ToLocation],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.from_location.description'') AS [FormLocationDescription],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.to_location.description'') AS [ToLocationDescription],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.pickup_date_time'') AS [PickUpDateTime],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.booked_date_time'') AS [BookedDateTime],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.cancelled_at'') AS [CancelledAt],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.passenger.count'') AS [PassengerCount],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.passenger.name'') AS [PassengerName],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.passenger.email'') AS [PassengerEmail],
									JSON_VALUE([TT].[TransferJson], ''$.data.attributes.passenger.mobile'') AS [PassengerMobile],
									JSON_VALUE([TT].[GRPaymentDetails], ''$.currency'') AS [Currency],
									JSON_VALUE([TT].[GRPaymentDetails], ''$.created_at'') AS [GRCreateAt],
									JSON_VALUE([TT].[GRPaymentDetails], ''$.payment_service_transaction_id'') AS [PaymentServiceTransactionId],
									JSON_VALUE([TT].[GRPaymentDetails], ''$.payment_method.method'') AS [PaymentMethod],
									JSON_VALUE([TT].[GRPaymentDetails], ''$.payment_service_refund_id'') AS [PaymentServiceRefeunfId],
									COUNT(*) OVER () AS [FilterCount]
					  FROM [dbo].[TaxiTransferGuestRequests] TT
					  INNER JOIN [dbo].[Customers] C
						    ON [C].[Id] = [TT].[CustomerId]
					  INNER JOIN [dbo].[CustomerGuests] CG 
						    ON [CG].[Id] = [TT].[GuestId]
					  INNER JOIN [dbo].[CustomerReservations] CR 
						    ON [CR].[Id] = [CG].[CustomerReservationId]
					  WHERE [TT].[DeletedAt] IS NULL '

					 IF(@SearchString IS NOT NULL)
					 BEGIN
					 SET @query += ' AND (' +
											'[C].[BusinessName] LIKE ''%'' + ISNULL(''' + CAST(@SearchString AS VARCHAR(100)) + ''', '''') + ''%'' OR ' +
											'[CG].[FirstName] LIKE ''%'' + ISNULL(''' + CAST(@SearchString AS VARCHAR(100)) + ''', '''') + ''%'' OR ' +
											'[CG].[LastName] LIKE ''%'' + ISNULL(''' + CAST(@SearchString AS VARCHAR(100)) + ''', '''') + ''%'' OR ' +
											'[CG].[Firstname] + '' '' + [CG].[Lastname] LIKE ''%'' + ISNULL(''' + CAST(@SearchString AS VARCHAR(100)) + ''', '''') + ''%'' OR ' +
											'[TT].[TransferStatus] LIKE ''%'' + ISNULL(''' + CAST(@SearchString AS VARCHAR(100)) + ''', '''') + ''%'' OR ' +
											'JSON_VALUE([TT].[ExtraDetailsJson], ''$.FromLocation'') + '' '' + JSON_VALUE([TT].[ExtraDetailsJson], ''$.ToLocation'') LIKE ''%'' + ISNULL(''' + CAST(@SearchString AS VARCHAR(100)) + ''', '''') + ''%'' ' +
										 ')' 
                     END

                     IF(@CustomerId > 0)
					 BEGIN
					  SET @query += 'AND [TT].[CustomerId] = '+CAST(@CustomerId AS VARCHAR(50))+'  '
					 END

                     IF(@GuestId > 0)
					 BEGIN
					  SET @query += 'AND [TT].[GuestId] = '+CAST(@GuestId AS VARCHAR(50))+'  '
					 END

					 IF(@FromCreate IS NOT NULL)
					 BEGIN
					  SET @query += 'AND CAST([TT].[CreatedAt] AS DATE) >= '''+ CAST(CAST(@FromCreate AS DATE) AS NVARCHAR(100))+'''   '
					 END

					 IF(@ToCreate IS NOT NULL)
					 BEGIN
					  SET @query += 'AND CAST([TT].[CreatedAt] AS DATE) <= '''+ CAST(CAST(@ToCreate AS DATE) AS NVARCHAR(100))+''' '
					 END

					 IF(@StatusCheck = 1)
					 BEGIN
					   SET @query += 'AND ( [TT].[TransferStatus] = ''confirmed'' OR ( [TT].[RefundStatus] = ''processing'' AND [TT].[RefundId] IS NOT NULL ) )'
					 END

					 SET @query += ' ORDER BY [TT].[CreatedAt] DESC OFFSET ' + CAST(@PageSize AS VARCHAR(100)) + ' * (' + CAST(@PageNo AS VARCHAR(100)) + ' - 1) ROWS FETCH NEXT ' + CAST(@PageSize AS VARCHAR(100)) + ' ROWS ONLY 
					                 FOR JSON PATH
									 )  AS [TransferJsonData]'

		EXEC(@query)
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
