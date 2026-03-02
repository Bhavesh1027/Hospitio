using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPropertyServicesForAPPBuilderSP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerPropertyServicesForAPPBuilder SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyServicesForAPPBuilder]    Script Date: 05-06-2023 14:36:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerPropertyServicesForAPPBuilder] 
@CustomerPropertyInformationId int = 0
AS

SELECT 
( 
SELECT C.[Id]
      ,C.[CustomerPropertyInformationId]
      ,C.[Name]
      ,C.[Icon]
      ,C.[Description]
,JSON_QUERY(( 
SELECT CI.[Id]
	  ,CI.[CustomerPropertyServiceId] 
      ,CI.[ServiceImages]
FROM CustomerPropertyServiceImages CI where CustomerPropertyServiceId=C.Id
AND DeletedAt is null
FOR JSON PATH
)) as [customerPropertyInfoServiceImagesOuts]
FROM CustomerPropertyServices C
Where C.CustomerPropertyInformationId = @CustomerPropertyInformationId
AND DeletedAt is null
FOR JSON PATH )
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
