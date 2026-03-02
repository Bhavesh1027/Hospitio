using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerDigitalAssistantRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP PROCEDURE IF EXISTS [dbo].[GetCustomerDigitalAssistantsListV2]
                                ");

            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerDigitalAssistants]    Script Date: 22-05-2023 10:21:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerDigitalAssistants] -- 1
(
	@CustomerId INT = 1
)
AS
SET NOCOUNT ON 
SET XACT_ABORT ON  

SELECT dbo.CustomerDigitalAssistants.Id,dbo.CustomerDigitalAssistants.CustomerId,dbo.CustomerDigitalAssistants.Name,dbo.CustomerDigitalAssistants.Details,dbo.CustomerDigitalAssistants.Icon,dbo.CustomerDigitalAssistants.IsActive
FROM dbo.CustomerDigitalAssistants WITH (NOLOCK)
WHERE dbo.CustomerDigitalAssistants.DeletedAt is null AND dbo.CustomerDigitalAssistants.CustomerId = @CustomerId 

                "
             );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
