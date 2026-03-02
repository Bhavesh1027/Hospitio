using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestsCheckInFormBuilderRelatedSPV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestsCheckInFormBuilder]    Script Date: 31-05-2023 19:17:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     Procedure [dbo].[GetCustomerGuestsCheckInFormBuilder]
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
	  ,[DisplayOrder]
        FROM CustomerGuestsCheckInFormFields ff 
        WHERE  fb.id = ff.CustomerGuestsCheckInFormBuilderId
		ORDER BY ff.DisplayOrder ASC
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
