using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class sp_GetPaymentProcessorsDefinationsById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetPaymentProcessorsDefinationsById]    Script Date: 13-07-2023 15:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create or ALTER PROCEDURE [dbo].[GetPaymentProcessorsDefinationsById]
@Id int =1
AS 
BEGIN 
        SET NOCOUNT ON 
		SET XACT_ABORT ON 
		          SELECT [Id],
						 [GRFields],
						 [GRSupportedCountries],
						 [GRSupportedCurrencies],
						 [GRSupportedFeatures],
						 [PaymentProcessorId]
				   FROM [dbo].[PaymentProcessorsDefinations](NOLOCK)
				   WHERE [DeletedAt] IS NULL
				         And [IsActive] = 1 
						 And [PaymentProcessorId] = @Id 

END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
