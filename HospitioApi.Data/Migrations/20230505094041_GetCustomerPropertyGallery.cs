using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPropertyGallery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
           /****** Object:  StoredProcedure [dbo].[GetCustPropGalleryById]    Script Date: 05-05-2023 18:59:22 ******/
           SET ANSI_NULLS ON
           GO
           SET QUOTED_IDENTIFIER ON
           GO

            Create or ALTER Procedure [dbo].[GetCustPropGalleryById]
            @CustomerPropertyInformationId Int=1
            as
              SELECT Id,PropertyImage
              FROM [CustomerPropertyGalleries] where CustomerPropertyInformationId=@CustomerPropertyInformationId and DeletedAt is null and IsActive =1
              ORDER BY Id
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
