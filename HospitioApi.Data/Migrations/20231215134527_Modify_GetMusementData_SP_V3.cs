using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetMusementData_SP_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetMusementData
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetMusementData]    Script Date: 15/12/2023 6:31:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[GetMusementData]
AS
BEGIN
  SELECT (
			SELECT [C].[BusinessName],
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
					          [MI].[TicketHolder]
					   FROM MusementItemInfos MI
					   WHERE [MI].[DeletedAt] IS NULL
					         AND [MI].[MusementOrderInfoId] = [MP].[OrderInfoId]
                       FOR JSON PATH
				   ))AS MusementItemInfo
			FROM MusementPaymentInfos MP
			INNER JOIN Customers C
				  ON [C].[Id] = [MP].[CustomerId]
				  AND [C].[DeletedAt] IS NULL 
			INNER JOIN CustomerGuests CG
				  ON [CG].[Id] = [MP].[CustomerGuestId]
				  AND [CG].[DeletedAt] IS NULL
			WHERE [MP].[DeletedAt] IS NULL
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
