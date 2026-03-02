using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class modify_GetMusementData_SP_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetMusementData
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetMusementData]    Script Date: 21/12/2023 2:08:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[GetMusementData]
(
	@SearchString NVARCHAR(50) = NULL,
	@PageNo INT = 1,
    @PageSize INT = 10
)
AS
BEGIN
  SELECT (
			SELECT [C].[BusinessName],
			       [CR].[ReservationNumber],
				   [C].[City],
				   [C].[Country],
				   [CG].[Firstname],
				   [CG].[Lastname],
				   [MP].[Amount],
				   [MP].[Currency],
				   [MP].[OrderUUID] AS [OrderId],
				   CASE WHEN [MP].[PaymentMetod] = 1
						THEN 'STRIPE'
						WHEN [MP].[PaymentMetod] = 2
						THEN 'ADYEN'
				   END AS [PaymentMethod],
				   [MP].[CreatedAt] AS [PaymentDate],
				   COUNT(*) OVER () AS [FilterCount],
				   JSON_QUERY((
				       SELECT [MI].[Title],
					          [MI].[TicketHolder],
							  [MI].[IsCancel],
							  [MI].[TotalPrice] AS [ItemPrice]
					   FROM MusementItemInfos MI
					   WHERE [MI].[DeletedAt] IS NULL
					         AND [MI].[MusementOrderInfoId] = [MP].[OrderInfoId]
							 AND [MI].[IsCancel] = 0
                       FOR JSON PATH
				   ))AS MusementItemInfo,
				   JSON_QUERY((
				       SELECT [MI].[Title],
					          [MI].[TicketHolder],
							  [MI].[IsCancel],
							  [MI].[TotalPrice] AS [ItemPrice]
					   FROM MusementItemInfos MI
					   WHERE [MI].[DeletedAt] IS NULL
					         AND [MI].[MusementOrderInfoId] = [MP].[OrderInfoId]
							 AND [MI].[IsCancel] = 1
                       FOR JSON PATH
				   ))AS MusementCancelItemInfo
			FROM MusementPaymentInfos MP
			INNER JOIN Customers C
				  ON [C].[Id] = [MP].[CustomerId]
				  AND [C].[DeletedAt] IS NULL 
			INNER JOIN CustomerGuests CG
				  ON [CG].[Id] = [MP].[CustomerGuestId]
				  AND [CG].[DeletedAt] IS NULL
            INNER JOIN CustomerReservations CR
			      ON [CR].[Id] = [CG].[CustomerReservationId]
				  AND [CR].[DeletedAt] IS NULL
			WHERE [MP].[DeletedAt] IS NULL
			AND 
			(
				[C].[BusinessName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[CG].[FirstName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[CG].[LastName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR
				[CG].[Firstname]+ ' ' +[CG].[Lastname] LIKE '%' + ISNULL(@SearchString,'') + '%'
			)
			ORDER BY [MP].[Id] ASC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
			FOR JSON PATH
	    )
 
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
