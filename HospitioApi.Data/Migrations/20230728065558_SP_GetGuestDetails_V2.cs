using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetGuestDetails_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER Proc [dbo].[SP_GetGuestDetails]
(
@GuestId int = 0,
@UserType VARCHAR(20) = 'CustomerGuest'
)

AS
BEGIN
DECLAre @ChannelId  Int = 0
 Select @ChannelId = IsNULL(CU.ChannelId,0) from Channels C
	INNEr JOIN ChannelUsers CU
	ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
	Where CU.UserId = @GuestId
	AND CU.UserType = @UserType

	If (@ChannelId = 0)	
		BEGIN
			DECLARE @CustomerUserId INT = 0
			DECLARE @CustomerUserType Varchar(20) = 'CustomerUser'
			
			Select @CustomerUserId = CU.Id
			from CustomerGuests CG
			INNER JOIN CustomerReservations CR
			ON CG.CustomerReservationId = CR.Id AND CR.DeletedAt Is NULL
			INNER JOIN Customers C
			ON  CR.CustomerId = C.Id AND C.DeletedAt Is NULL
			INNER JOIN CustomerUsers CU
			ON CU.CustomerId = C.Id AND CU.DeletedAt Is NULL
			Where CG.Id = @GuestId AND CG.DeletedAt Is NULL
			print @CustomerUserId
			INSERT INTO Channels (Uuid,CreateForm,channelUserID,IsActive,CreatedAt,UpdateAt) 
			Values (Lower(NEWID()),@UserType,@GuestId,1,GETUTCDATE(),GETUTCDATE())		
			
			SET @ChannelId = SCOPE_IDENTITY()

			INSERT INTO ChannelUsers (ChannelId,LastMessageReadTime,LastMessageReadId,UserType,UserId,IsActive,CreatedAt,UpdateAt)
			Values (@ChannelId,GETUTCDATE(),NULL,@UserType,@GuestId,1,GETUTCDATE(),GETUTCDATE())
					   
			INSERT INTO ChannelUsers (ChannelId,LastMessageReadTime,LastMessageReadId,UserType,UserId,IsActive,CreatedAt,UpdateAt)
			Values (@ChannelId,GETUTCDATE(),NULL,@CustomerUserType,@CustomerUserId,0,GETUTCDATE(),GETUTCDATE())
		END
	Select Distinct CG.Firstname as GuestFirstName, CG.Lastname AS GuestLastName ,CG.Picture as GuestProfile, CU.FirstName as CustomerFirstName, CU.LastName AS CustomerLastName, 
	CU.ProfilePicture AS CustomerProfile,@ChannelId AS ChannelId
	from CustomerGuests CG
	INNER JOIN CustomerReservations CR
	ON CG.CustomerReservationId = CR.Id AND CR.DeletedAt Is NULL
	INNER JOIN Customers C
	ON  CR.CustomerId = C.Id AND C.DeletedAt Is NULL
	INNER JOIN CustomerUsers CU
	ON CU.CustomerId = C.Id AND CU.DeletedAt Is NULL
	Where CG.Id = @GuestId AND CG.DeletedAt Is NULL
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
