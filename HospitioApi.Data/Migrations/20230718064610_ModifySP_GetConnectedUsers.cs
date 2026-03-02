using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifySP_GetConnectedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER     PROCEDURE [dbo].[SP_GetConnectedUsers]
	@UserId int =0,
	@UserType nvarchar(20)
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT DISTINCT cu1.UserId, cu1.ChannelId AS ChatId,
		CASE
			WHEN (cu1.UserType = 'HospitioUser') THEN 1
			WHEN (cu1.UserType = 'CustomerUser') THEN 2
			WHEN (cu1.UserType = 'CustomerGuest') THEN 3
		END AS [UserType]
	FROM channelUsers cu1
	INNER JOIN channelUsers cu2 ON cu1.channelId = cu2.channelId
	WHERE cu2.UserId = @UserId  
		AND cu1.UserId <> @UserId
		AND cu2.UserType = @UserType;

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
