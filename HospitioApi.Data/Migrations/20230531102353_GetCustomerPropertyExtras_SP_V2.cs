using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPropertyExtras_SP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerPropertyExtras SP V2

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyExtras]    Script Date: 31-05-2023 15:30:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerPropertyExtras]
@CustomerPropertyInformationId int =0
as
SELECT 
( 
SELECT  CE.[Id]
      ,CE.[CustomerPropertyInformationId]
      ,CE.[ExtraType]
      ,CE.[Name]
,JSON_QUERY(( 
SELECT CED.[Id]
      ,CED.[CustomerPropertyExtraId]
      ,CED.[Description]
      ,CED.[Link]
FROM CustomerPropertyExtraDetails CED where CED.CustomerPropertyExtraId=CE.Id
FOR JSON PATH
)) as [CustomerPropertyExtraDetailsOuts]
FROM CustomerPropertyExtras CE
WHERE CE.CustomerPropertyInformationId = @CustomerPropertyInformationId
FOR JSON PATH )");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
