using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPropertyExtras_SP_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyExtras]    Script Date: 01-06-2023 10:54:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetCustomerPropertyExtras]
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
FROM CustomerPropertyExtraDetails CED where CED.CustomerPropertyExtraId=CE.Id and DeletedAt is null
FOR JSON PATH
)) as [CustomerPropertyExtraDetailsOuts]
FROM CustomerPropertyExtras CE
WHERE CE.CustomerPropertyInformationId = @CustomerPropertyInformationId and DeletedAt is null
FOR JSON PATH )
                "
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
