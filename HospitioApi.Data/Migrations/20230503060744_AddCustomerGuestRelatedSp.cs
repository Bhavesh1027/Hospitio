using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddCustomerGuestRelatedSp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Get Customer Guests
            migrationBuilder.Sql(@"
                                    CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuests]
                                    (
                                    	@CustomerReservationId int = 0,
                                        @SearchColumn NVARCHAR(50) = '',
                                        @SearchValue NVARCHAR(50) = '',
                                        @PageNo INT = 1,
                                        @PageSize INT = 10,
                                        @SortColumn NVARCHAR(20) = 'Firstname',
                                        @SortOrder NVARCHAR(5) = 'ASC'
                                    )
                                    AS
                                    BEGIN
                                    
                                       SET NOCOUNT ON;
                                    
                                       SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
                                       SET @SearchValue = LTRIM(RTRIM(@SearchValue))
                                    
                                       ; WITH CustomerGuests_Results AS
                                       (
                                           SELECT [Id],[CustomerReservationId],[Firstname],[Lastname],[Email],[Picture],[PhoneCountry],[PhoneNumber],[Country],[Language],[IdProof],[IdProofType],[IdProofNumber],[BlePinCode],[Pin],[Street],[StreetNumber],[City],[Postalcode],[ArrivalFlightNumber],[DepartureAirline],[DepartureFlightNumber],[Signature],[RoomNumber],[TermsAccepted],[FirstJourneyStep],[Rating],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy]
                                    	,COUNT(*) OVER() as FilteredCount
                                    
                                    	FROM [dbo].[CustomerGuests] WITH (NOLOCK)
                                    
                                           WHERE DeletedAt is null AND CustomerReservationId = @CustomerReservationId AND @SearchColumn= '' OR  (
                                                   CASE @SearchColumn
                                                       WHEN 'Firstname' THEN Firstname
                                                   END
                                               ) LIKE '%' + @SearchValue + '%'
                                    
                                    	ORDER BY
                                    	CASE WHEN (@SortColumn = 'Firstname' AND @SortOrder='ASC')
                                           THEN Firstname
                                           END ASC,
                                           
                                    	CASE WHEN (@SortColumn = 'Firstname' AND @SortOrder='DESC')
                                           THEN Firstname
                                           END DESC
                                    
                                    	OFFSET @PageSize * (@PageNo - 1) ROWS
                                           FETCH NEXT @PageSize ROWS ONLY
                                       )
                                    
                                       select [Id],[CustomerReservationId],[Firstname],[Lastname],[Email],[Picture],[PhoneCountry],[PhoneNumber],[Country],[Language],[IdProof],[IdProofType],[IdProofNumber],[BlePinCode],[Pin],[Street],[StreetNumber],[City],[Postalcode],[ArrivalFlightNumber],[DepartureAirline],[DepartureFlightNumber],[Signature],[RoomNumber],[TermsAccepted],[FirstJourneyStep],[Rating],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy],FilteredCount 
                                       from CustomerGuests_Results
                                       OPTION (RECOMPILE)
                                    END
                                ");

            // Get Customer Guest by Id
            migrationBuilder.Sql(@"
                                    CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestById]
                                    @Id int = 0
                                    AS
                                    BEGIN
                                    	SELECT [Id],[CustomerReservationId],[Firstname],[Lastname],[Email],[Picture],[PhoneCountry],[PhoneNumber],[Country],[Language],[IdProof],[IdProofType],[IdProofNumber],[BlePinCode],[Pin],[Street],[StreetNumber],[City],[Postalcode],[ArrivalFlightNumber],[DepartureAirline],[DepartureFlightNumber],[Signature],[RoomNumber],[TermsAccepted],[FirstJourneyStep],[Rating],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy]
									    FROM [dbo].[CustomerGuests]
                                        where Id = @Id and DeletedAt is null
                                    END
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
