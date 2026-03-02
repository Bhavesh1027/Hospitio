using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetPaymentServiceByPaymentHospitioProcessorIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetPaymentServiceByPaymentHospitioProcessorId]    Script Date: 17-07-2023 14:11:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     Procedure [dbo].[GetPaymentServiceByPaymentHospitioProcessorId] -- 1
@HospitioPaymentProcessorId int=1
AS

 
SELECT 
( 
SELECT [Id]
      ,[PaymentProcessorId]
      ,[GRPaymentServiceId]
      ,[GRWebhookURL]
      ,[IsActive]
      ,[GR3DSecureEnabled]
      ,[GRAcceptedCountries]
      ,[GRAcceptedCurrencies]
      ,[GRFields]
      ,[GRIsDeleted]
      ,[GRMerchantProfile]
,JSON_QUERY(( 
SELECT [Id]
      ,[IsActive]
      ,[GRCategory]
      ,[GRGroup]
      ,[GRID]
      ,[GRIcon]
      ,[GRName]
FROM [dbo].[PaymentProcessors] where Id=r.PaymentProcessorId and DeletedAt is null
FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
)) as [PaymentProcessorsOuts]
,JSON_QUERY(( 
SELECT [Id]
      ,[GRFields]
      ,[GRSupportedCountries]
      ,[GRSupportedCurrencies]
      ,[GRSupportedFeatures]
      ,[PaymentProcessorId]
	  ,[IsActive]
FROM [dbo].[PaymentProcessorsDefinations] where PaymentProcessorId=r.PaymentProcessorId and DeletedAt is null
FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
)) as [PaymentProcessorsDefinationsOuts]
FROM  [dbo].[HospitioPaymentProcessors] r
where Id = @HospitioPaymentProcessorId and DeletedAt is null
FOR JSON PATH )
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
