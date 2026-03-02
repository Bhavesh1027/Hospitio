using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CreateSP_GetReceiverUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetReceiverUser]
	@ChatId int = 0,
	@UserId int =0,
	@UserType varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT CU.Id,CU.UserId, CU.UserType FROM Channels C
	INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
	WHERE C.Id = @ChatId AND C.DeletedAt IS NULL 
	AND (
		(
				CU.UserType = @UserType
				AND CU.UserId <> @UserId
		)
		OR (
				CU.UserType <> @UserType
				AND CU.UserType <> @UserType
		)
	)

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
