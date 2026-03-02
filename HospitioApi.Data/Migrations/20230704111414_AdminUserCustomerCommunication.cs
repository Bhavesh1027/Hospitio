using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AdminUserCustomerCommunication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[SP_Get_CustomerCommunicationByReservationId]
(
    @Id INT = 0,
    @ReservationId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

    IF (ISNULL(@ReservationId, '') = '' OR @ReservationId = 0)
    BEGIN
        SELECT DISTINCT
            [U].[Id],
            [U].[FirstName],
            [U].[LastName],
            [U].[Email],
            [U].[ProfilePicture],
            [U].[PhoneCountry],
            [U].[PhoneNumber],
            NULL AS [Country],
            NULL AS [Language],
            NULL AS [RoomNumber],
            [U].[CreatedAt],
            NULL AS [CheckinDate],
            NULL AS [CheckoutDate]
        FROM [dbo].[Users] U (NOLOCK)
        WHERE [U].[DeletedAt] IS NULL
              AND [U].[Id] = @Id
    END
    ELSE
    BEGIN
        SELECT DISTINCT
            [CR].[Id],
            [CG].[Firstname] AS [FirstName],
            [CG].[Lastname] AS [LastName],
            [CG].[Email],
            [CG].[Picture] AS [ProfilePicture],
            [CG].[PhoneCountry],
            [CG].[PhoneNumber],
            [CG].[Country],
            [CG].[Language],
            [CG].[RoomNumber],
            [CR].[CreatedAt],
            [CR].[CheckinDate],
            [CR].[CheckoutDate]
        FROM [dbo].CustomerGuests CG (NOLOCK)
            INNER JOIN [dbo].[CustomerReservations] CR
                ON [CR].[Id] = [CG].[CustomerReservationId]
                   AND [CG].[DeletedAt] IS NULL
        WHERE [CR].[DeletedAt] IS NULL
              AND [CR].[Id] = @ReservationId
			  AND [CG].[Id] = @Id
    END
END");
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_Get_Communication_CustomerGuest] 
(
	@CustomerId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [CG].[Id],
           [CR].[CustomerId],
           [CG].[CustomerReservationId],
           [CG].[Firstname] AS [FirstName],
           [CG].[Lastname] AS [LastName],
           [CG].[Email],
           [CG].[Picture],
           [CG].[PhoneCountry],
           [CG].[PhoneNumber],
           [CG].[Country],
           [CG].[Language]
    FROM [dbo].[CustomerGuests] CG (NOLOCK)
        INNER JOIN [dbo].[CustomerReservations] CR (NOLOCK)
            ON [CR].[Id] = [CG].[CustomerReservationId]
               AND [CG].[DeletedAt] IS NULL
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
           NULL AS [Country]
    FROM [dbo].[Users] U (NOLOCK)
    WHERE [U].[DeletedAt] IS NULL

END");
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_Get_AdminUserCustomersSearch]
(
	@SearchString NVARCHAR(50) = NULL,
	@PageNo INT = 1,
    @PageSize INT = 10
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
        [CustomerId] INT,
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(50),
        [Email] NVARCHAR(100),
        [Title] NVARCHAR(5),
        [ProfilePicture] NVARCHAR(500),
        [PhoneCountry] NVARCHAR(3),
        [PhoneNumber] NVARCHAR(20),
        [UserName] NVARCHAR(100),
        [Password] NVARCHAR(255)
   )

	INSERT INTO #TempTables ([CustomerId],[FirstName],[LastName],[Email],[Title],[ProfilePicture],[PhoneCountry],[PhoneNumber],[UserName],[Password])
	SELECT	[CU].[CustomerId],[CU].[FirstName],[CU].[LastName],[CU].[Email],[CU].[Title],[CU].[ProfilePicture],[CU].[PhoneCountry],[CU].[PhoneNumber],[CU].[UserName],[CU].[Password]
	FROM [dbo].[CustomerUsers] CU (NOLOCK) 
	WHERE [CU].[DeletedAt] IS NULL 
	--UNION ALL
	--SELECT	NULL AS [CustomerId],[U].[FirstName],[U].[LastName],[U].[Email],[U].[Title],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],[U].[UserName],[U].[Password],[U].[DepartmentId],
	--		[U].[UserLevelId],[U].[SupervisorId],[U].[GroupId] 
	--FROM [dbo].[Users] U (NOLOCK)
	--WHERE [U].[DeletedAt] IS NULL 

	SELECT [CustomerId],[FirstName],[LastName],[Email],[Title],[ProfilePicture],[PhoneCountry],[PhoneNumber],[UserName],[Password]
	FROM #TempTables
	WHERE 
			(
				[FirstName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[LastName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[Email] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[UserName] LIKE '%' + ISNULL(@SearchString,'') + '%'
			)
	ORDER BY [CustomerId] ASC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
END");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
