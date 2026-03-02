using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_SP_GetReceiverUserForWP_AND_SP_GetSenderUserForWP_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetReceiverUserForWP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetReceiverUserForWP]    Script Date: 3/10/2023 3:01:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetReceiverUserForWP]
	@ChatId int = 0,
	@UserId int =0,
	@UserType varchar(50)
AS
BEGIN

	SET NOCOUNT ON;
	IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN Users U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL 
		AND CU.UserType = 'HospitioUser' 

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,C1.WhatsappNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN CustomerUsers U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		INNER JOIN Customers C1 ON C1.Id = U.CustomerId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL 
		AND CU.UserType = 'CustomerUser'

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN AnonymousUsers U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL AND U.UserType = 2
		AND CU.UserType = 'AnonymousUser'
	END

	IF(@UserType = 'CustomerUser')
	BEGIN
		SELECT CU.Id,CU.UserId, CU.UserType
		,U.PhoneNumber AS Phonenumber 
		FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN Users U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = 1 AND C.DeletedAt IS NULL 
		AND CU.UserType = 'HospitioUser'

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN AnonymousUsers U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL
		AND CU.UserType = 'AnonymousUser'

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN CustomerGuests U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL
		AND CU.UserType = 'CustomerGuest'
	END
END");
            #endregion

            #region SP_GetSenderUserForWP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetSenderUserForWP]    Script Date: 3/10/2023 3:26:58 PM ******/
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
			   U.PhoneNumber AS Phonenumber, 
			   null AS AppId, 
			   null AS AppPrivatKey 
	    FROM Channels C
		     INNER JOIN ChannelUsers CU 
			       ON CU.ChannelId = C.Id 
				      AND CU.DeletedAt IS NULL
		     INNER JOIN Users U 
			       ON U.Id = CU.UserId 
				      AND U.DeletedAt IS NULL

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
