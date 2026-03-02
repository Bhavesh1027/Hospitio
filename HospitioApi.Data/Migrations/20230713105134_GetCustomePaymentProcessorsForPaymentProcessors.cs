using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomePaymentProcessorsForPaymentProcessors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/***** Object:  StoredProcedure [dbo].[GetCustomePaymentProcessorsForPaymentProcessors]    Script Date: 13-07-2023 15:49:09 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  Create or ALTER   PROCEDURE [dbo].[GetCustomePaymentProcessorsForPaymentProcessors] 
  (
    @Id INT = 0
  )
  AS
  BEGIN
     SET NOCOUNT ON
     SET XACT_ABORT ON

     SELECT [C].[CustomerId] ,
            [C].[PaymentProcessorId] ,
			[C].[GRPaymentServiceId] , 
            [P].[GRCategory] ,
			[P].[GRGroup] ,
			[P].[GRID] , 
			[P].[GRIcon] , 
			[P].[GRName] ,
			[P].[IsActive]
     FROM [dbo].[CustomerPaymentProcessors]  C (NOLOCK)
	    INNER JOIN [dbo].[PaymentProcessors]  P (NOLOCK)
           ON [C].[PaymentProcessorId] = [P].[Id] 
		     AND [P].[DeletedAt] IS NULL
    WHERE [C].[CustomerId] = @Id 
	      AND [C].[DeletedAt] IS NULL

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
