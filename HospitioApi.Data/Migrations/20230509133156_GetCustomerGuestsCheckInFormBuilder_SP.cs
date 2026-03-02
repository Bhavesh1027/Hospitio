using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestsCheckInFormBuilder_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
 /****** Object:  StoredProcedure [dbo].[GetCustomerGuestsCheckInFormBuilder]    Script Date: 09-05-2023 18:59:22 ******/
 SET ANSI_NULLS ON
 GO
 SET QUOTED_IDENTIFIER ON
 GO

 Create or ALTER Procedure [dbo].[GetCustomerGuestsCheckInFormBuilder]
  @Id Int=1
 as
 SELECT 
    ( 
    SELECT *,
        (SELECT *
        FROM CustomerGuestsCheckInFormFields ff 
        WHERE  fb.id = ff.CustomerGuestsCheckInFormBuilderId
        FOR JSON PATH) AS GetCustomerGuestsCheckInFormFieldsOut
    FROM  CustomerGuestsCheckInFormBuilders fb where fb.Id=@Id and fb.DeletedAt is null
 FOR JSON PATH ) as GetCustomerGuestsCheckInFormBuilderResponseOut
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
