using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPropertyServicesForAPPBuilder_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerPropertyServicesForAPPBuilder SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyServicesForAPPBuilder]    Script Date: 31-05-2023 11:47:00 ******/
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
SELECT CI.[Id] as ImageID
      ,CI.[ServiceImages]
FROM CustomerPropertyServiceImages CI where CustomerPropertyServiceId=C.Id
FOR JSON PATH
)) as [PropertyServiceImages]
FROM CustomerPropertyServices C
Where C.CustomerPropertyInformationId = @CustomerPropertyInformationId
AND C.DeletedAt is null
FOR JSON PATH )
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
