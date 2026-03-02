using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SP_Get_Communication_CustomerGuest_AND_SP_GetUserDetailFromPhoneNumber_SPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_Get_Communication_CustomerGuest
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_Get_Communication_CustomerGuest]    Script Date: 31/10/2023 11:20:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_Get_Communication_CustomerGuest] 
(
	@CustomerId INT = 0,
	@SearchString NVARCHAR(50) = NULL,
	@PageNo INT = 1,
    @PageSize INT = 10,
	@CustomerUserLevel NVARCHAR(100) = NULL
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

	SET @SearchString = LTRIM(RTRIM(@SearchString));

	IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
        DROP TABLE #TempTables;

    CREATE TABLE #TempTables
    (
        [Id] INT,
		[CustomerId] INT,
        [CustomerReservationId] NVARCHAr(100),
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(50),
        [Email] NVARCHAR(100),
        [Picture] NVARCHAR(500),
        [PhoneCountry] NVARCHAR(3),
        [PhoneNumber] NVARCHAR(20),
        [Country] NVARCHAR(10),
        [Language] NVARCHAR(10),
		[UserType] NVARCHAR(20),
		[ChatType] NVARCHAR(20)
   )

   INSERT INTO #TempTables ([Id],[CustomerId],[CustomerReservationId],[FirstName],[LastName],[Email],[Picture],[PhoneCountry],[PhoneNumber],[Country],[Language],[UserType],[ChatType])
    SELECT [CG].[Id],
           [CR].[CustomerId],
		   [CG].[CustomerReservationId],
           [CG].[Firstname],
           [CG].[Lastname],
           [CG].[Email],
           [CG].[Picture],
           [CG].[PhoneCountry],
           [CG].[PhoneNumber],
           [CG].[Country],
           [CG].[Language],
		   'CustomerGuest',
		   (CASE
				WHEN (
						SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
					) > 0
				THEN 'inbox'
				WHEN 
					(SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
					) = 0
					AND (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
					AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE()))
				
				THEN 'in-bound'
				WHEN (
						(
						SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
						) = 0
						AND
						(
							(CONVERT(DATE, CR.CheckinDate) > CONVERT(DATE, GETDATE())
							AND CONVERT(DATE, CR.CheckoutDate) > CONVERT(DATE, GETDATE()))
						)
				)
				THEN 'contacted'
				ELSE ''
			END) AS ChatType
    FROM [dbo].[CustomerGuests] CG (NOLOCK)
        INNER JOIN [dbo].[CustomerReservations] CR (NOLOCK) ON [CR].[Id] = [CG].[CustomerReservationId] AND [CG].[DeletedAt] IS NULL
		LEFT JOIN [dbo].[ChannelUsers] CU1 ON CU1.UserId = CG.Id AND CU1.UserType = 'CustomerGuest'
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
    WHERE [CR].[DeletedAt] IS NULL 
		AND [CR].[CustomerId] = @CustomerId
    UNION
    SELECT [U].[Id],
           NULL AS [CustomerId],
		   NULL AS [CustomerReservationId],
           [U].[FirstName],
           [U].[LastName],
           [U].[Email],
           [U].[ProfilePicture],
           [U].[PhoneCountry],
           [U].[PhoneNumber],
           NULL AS [Country],
           NULL AS [Language],
		   'HospitioUser',
		   '' AS [ChatType]
    FROM [dbo].[Users] U (NOLOCK)
    WHERE [U].[DeletedAt] IS NULL 
	      AND UserLevelId = 1
	      AND @CustomerUserLevel = 'Super Admin'

            SELECT	[Id],
					[CustomerId],
					[CustomerReservationId],
					[FirstName],
					[LastName],
					[Email],
					[Picture],
					[PhoneCountry],
					[PhoneNumber],
					[Country],
					[Language],
					[UserType],
					[ChatType]
            FROM #TempTables
			WHERE 
			(
				[FirstName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[LastName] LIKE '%' + ISNULL(@SearchString,'') + '%'
			)
			ORDER BY [CustomerId] ASC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY

END");
            #endregion

            #region SP_GetUserDetailFromPhoneNumber
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserDetailFromPhoneNumber]    Script Date: 31/10/2023 11:16:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetUserDetailFromPhoneNumber] -- '306932626372' , '0'
    @PhoneNumber NVARCHAR(50),
	@AnonymousUsersType NVARCHAR(50) =0
AS
BEGIN
	DECLARE @UserId INT = null;
	DECLARE @UserType VARCHAR(20) = '';

    DECLARE @NormalizedPhoneNumber NVARCHAR(50)
    SET @NormalizedPhoneNumber = REPLACE(REPLACE(@PhoneNumber, '+', ''), ' ', '')

    -- Find matching records in the 'users' table
    SELECT TOP 1 @UserId = Id, @UserType = '1'
    FROM HospitioOnboardings
    WHERE REPLACE(REPLACE(WhatsappNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
	AND DeletedAt IS NULL

    -- Find matching records in the 'customerGuest' table
    IF @UserId IS NULL
    BEGIN
        SELECT TOP 1 @UserId = Id, @UserType = '3'
        FROM CustomerGuests
        WHERE REPLACE(REPLACE(PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
		AND DeletedAt IS NULL
    END
    -- Find matching records in the 'customers' table
    IF @UserId IS NULL
    BEGIN
        SELECT TOP 1 @UserId = cu.Id, @UserType = '2'
        FROM Customers c
        INNER JOIN CustomerUsers cu ON c.Id = cu.CustomerId
        WHERE REPLACE(REPLACE(c.WhatsappNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
		AND cu.DeletedAt IS NULL
    END
	 IF @UserId IS NULL
    BEGIN
	  IF(@AnonymousUsersType = '1')
		  BEGIN
			SELECT TOP 1 @UserId = c.Id, @UserType = '4'
			FROM AnonymousUsers c
			WHERE REPLACE(REPLACE(c.PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
				  AND c.UserType = 2
				  AND DeletedAt IS NULL
		  END
	  ELSE 
		  BEGIN
		  	SELECT TOP 1 @UserId = c.Id, @UserType = '4'
			FROM AnonymousUsers c
			WHERE REPLACE(REPLACE(c.PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
				  AND c.UserType = 3
				  AND DeletedAt IS NULL
		  END

    END

	select @UserId As UserId, @UserType As UserType
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
