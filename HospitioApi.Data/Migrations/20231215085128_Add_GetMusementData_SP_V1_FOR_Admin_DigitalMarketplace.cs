using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GetMusementData_SP_V1_FOR_Admin_DigitalMarketplace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetMusementData
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetMusementData]    Script Date: 15/12/2023 12:24:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER  PROCEDURE [dbo].[GetMusementData]
AS
BEGIN
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
		   [MP].[CreatedAt] AS [PaymentDate]
	FROM MusementPaymentInfos MP
	INNER JOIN Customers C
	      ON [C].[Id] = [MP].[CustomerId]
		  AND [C].[DeletedAt] IS NULL 
    INNER JOIN CustomerGuests CG
	      ON [CG].[Id] = [MP].[CustomerGuestId]
		  AND [CG].[DeletedAt] IS NULL
 
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
