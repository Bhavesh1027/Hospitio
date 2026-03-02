using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestsCheckInFormBuilderRelatedSPV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestsCheckInFormBuilder]    Script Date: 29-08-2023 12:17:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerGuestsCheckInFormBuilder] 
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [Id],
                   [CustomerId],
                   [Color],
                   [Name],
                   [Stars],
                   [Logo],
                   [AppImage],
                   [SplashScreen],
                   [IsOnlineCheckInFormEnable],
                   [IsRedirectToGuestAppEnable],
                   [SubmissionMail],
                   [TermsLink],
                   [IsActive],
				   [GuestWelcomeMessage],
                   (
                       SELECT [Name],
                              [FieldType],
                              [RequiredFields],
                              [IsActive],
                              [DisplayOrder]
                       FROM [dbo].[CustomerGuestsCheckInFormFields] ff (NOLOCK)
                       WHERE [fb].[id] = [ff].[CustomerGuestsCheckInFormBuilderId]
                             AND [ff].[DeletedAt] IS NULL
                       ORDER BY [ff].[DisplayOrder] ASC
                       FOR JSON PATH
                   ) AS [GetCustomerGuestsCheckInFormFieldsOut]
            FROM [dbo].[CustomerGuestsCheckInFormBuilders] fb
            WHERE [fb].[DeletedAt] IS NULL
                  AND [fb].[CustomerId] = @CustomerId
            FOR JSON PATH
        ) as [GetCustomerGuestsCheckInFormBuilderResponseOut]
END
                "
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
