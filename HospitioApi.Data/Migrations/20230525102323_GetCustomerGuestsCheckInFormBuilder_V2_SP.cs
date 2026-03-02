using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestsCheckInFormBuilder_V2_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestsCheckInFormBuilder]    Script Date: 25-05-2023 15:55:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetCustomerGuestsCheckInFormBuilder]
  @CustomerId Int=1
 as
 SELECT 
    ( 
    SELECT [Id]
      ,[CustomerId]
      ,[Color]
      ,[Name]
      ,[Stars]
      ,[Logo]
      ,[AppImage]
      ,[SplashScreen]
      ,[IsOnlineCheckInFormEnable]
      ,[IsRedirectToGuestAppEnable]
      ,[SubmissionMail]
      ,[TermsLink]
      ,[IsActive],
        (SELECT [Name]
      ,[FieldType]
      ,[RequiredFields]
      ,[IsActive]
        FROM CustomerGuestsCheckInFormFields ff 
        WHERE  fb.id = ff.CustomerGuestsCheckInFormBuilderId
        FOR JSON PATH) AS GetCustomerGuestsCheckInFormFieldsOut
    FROM  CustomerGuestsCheckInFormBuilders fb where fb.CustomerId=@CustomerId and fb.DeletedAt is null
 FOR JSON PATH ) as GetCustomerGuestsCheckInFormBuilderResponseOut

                "
             );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
