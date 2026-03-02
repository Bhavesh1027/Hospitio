using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SeedData_HospitioOnBoarding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Default Hospitio DashBoard Data

            migrationBuilder.Sql(@"INSERT INTO [dbo].[HospitioOnboardings]([WhatsappCountry],[WhatsappNumber],[ViberCountry],[ViberNumber],[TelegramCounty],[TelegramNumber],[PhoneCountry],[PhoneNumber],[SmsTitle],[Messenger],[Email],[Cname],[IncomingTranslationLangage],[NoTranslateWords],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy])VALUES(NUll,'+306980829333',NUll,NUll,NUll,NUll,NUll,NUll,NUll,NUll,NUll,NUll,'English',NUll,NUll,NUll,NUll,NUll,NUll)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
