using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetHospitioOnboarding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetHospitioOnboarding]    Script Date: 21-06-2023 10:26:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[GetHospitioOnboarding]
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id]
      ,[WhatsappCountry]
      ,[WhatsappNumber]
      ,[ViberCountry]
      ,[ViberNumber]
      ,[TelegramCounty] as TelegramCountry
      ,[TelegramNumber] 
      ,[PhoneCountry]
      ,[PhoneNumber]
      ,[SmsTitle]
      ,[Messenger]
      ,[Email]
      ,[Cname]
      ,[IncomingTranslationLangage] as IncomingTranslationLanguage
      ,[NoTranslateWords]
    FROM [dbo].[HospitioOnboardings] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
