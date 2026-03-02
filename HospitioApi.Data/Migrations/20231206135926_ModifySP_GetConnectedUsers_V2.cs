using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifySP_GetConnectedUsers_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
/****** Object:  StoredProcedure [dbo].[SP_GetConnectedUsers]    Script Date: 06-12-2023 18:56:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER         PROCEDURE [dbo].[SP_GetConnectedUsers]
	@UserId int =0,
	@UserType nvarchar(20)
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT  cu1.UserId, cu1.ChannelId AS ChatId,
		CASE
			WHEN (cu1.UserType = 'HospitioUser') THEN 1
			WHEN (cu1.UserType = 'CustomerUser') THEN 2
			WHEN (cu1.UserType = 'CustomerGuest') THEN 3
			WHEN (cu1.UserType = 'ChatWidgetUser') THEN 5
		END AS [UserType] 
	FROM channelUsers cu1
	INNER JOIN channelUsers cu2 ON cu1.channelId = cu2.channelId
	WHERE cu2.UserId = @UserId  
		AND cu2.UserType = @UserType
		AND
		(
					(
							cu1.UserType = @UserType
							AND cu1.UserId <> @UserId
					)
					OR (
							cu1.UserType <> @UserType
					)
				)

END

");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
