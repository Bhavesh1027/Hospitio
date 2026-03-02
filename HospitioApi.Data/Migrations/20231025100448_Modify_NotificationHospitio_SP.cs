using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_NotificationHospitio_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region NotificationHospitio
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetSenderUserForWP]    Script Date: 25/10/2023 2:52:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetSenderUserForWP]

	@ChatId int = 0,
	@UserId int =0,
	@UserType varchar(50)

AS
BEGIN

	SET NOCOUNT ON;

	IF(@UserType = 'HospitioUser')
	BEGIN

		SELECT CU.Id,
		       CU.UserId, 
			   CU.UserType,
			   R.WhatsappNumber AS Phonenumber, 
			   null AS AppId, 
			   null AS AppPrivatKey 
	    FROM Channels C
		     INNER JOIN ChannelUsers CU 
			       ON CU.ChannelId = C.Id 
				      AND CU.DeletedAt IS NULL
		     INNER JOIN Users U 
			       ON U.Id = CU.UserId 
				      AND U.DeletedAt IS NULL
			 LEFT JOIN [dbo].[HospitioOnboardings] R
			      ON R.Id = 1

		WHERE C.Id = @ChatId 
		      AND C.DeletedAt IS NULL 
		      AND CU.UserId = @UserId
		      AND CU.UserType = @UserType

	END

	IF(@UserType = 'CustomerUser')
	BEGIN

		SELECT CU.Id,
		       CU.UserId, 
			   CU.UserType,
			   C1.WhatsappNumber AS Phonenumber,
			   VC.AppId,
			   VC.AppPrivatKey 
		FROM Channels C
		     INNER JOIN ChannelUsers CU 
			       ON CU.ChannelId = C.Id 
				      AND CU.DeletedAt IS NULL
			 INNER JOIN CustomerUsers U 
			       ON U.Id = CU.UserId 
				      AND U.DeletedAt IS NULL
			 INNER JOIN Customers C1 
			       ON C1.Id = U.CustomerId 
				      AND C1.DeletedAt IS NULL
			 INNER JOIN VonageCredentials VC 
			       ON VC.CustomerId = C1.Id 
				      AND VC.DeletedAt IS NULL
		WHERE C.Id = @ChatId
		      AND C.DeletedAt IS NULL 
	          AND CU.UserId = @UserId
		      AND CU.UserType = @UserType

	END

END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
