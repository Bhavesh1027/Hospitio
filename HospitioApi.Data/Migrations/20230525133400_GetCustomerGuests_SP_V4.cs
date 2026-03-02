using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuests_SP_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerGuests SP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuests]    Script Date: 25-05-2023 18:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create OR ALTER PROCEDURE [dbo].[GetCustomerGuests] 
(
     @CustomerId int = 0,
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Firstname',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN
                                    
    SET NOCOUNT ON;
      
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
                                    
    ; WITH CustomerGuests_Results AS
    (
        SELECT dbo.CustomerGuests.[Id],dbo.CustomerGuests.[CustomerReservationId],dbo.CustomerGuests.[Firstname]
		,dbo.CustomerGuests.[Lastname],dbo.CustomerGuests.[Email],dbo.CustomerGuests.[Picture],dbo.CustomerGuests.[PhoneCountry]
		,dbo.CustomerGuests.[PhoneNumber],dbo.CustomerGuests.[Country],dbo.CustomerGuests.[Language],dbo.CustomerGuests.[IdProof]
		,dbo.CustomerGuests.[IdProofType],dbo.CustomerGuests.[IdProofNumber],dbo.CustomerGuests.[BlePinCode]
		,dbo.CustomerGuests.[Pin],dbo.CustomerGuests.[Street],dbo.CustomerGuests.[StreetNumber]
		,dbo.CustomerGuests.[City],dbo.CustomerGuests.[Postalcode],dbo.CustomerGuests.[ArrivalFlightNumber]
		,dbo.CustomerGuests.[DepartureAirline],dbo.CustomerGuests.[DepartureFlightNumber],dbo.CustomerGuests.[Signature],dbo.CustomerGuests.[RoomNumber]
		,dbo.CustomerGuests.[TermsAccepted],dbo.CustomerGuests.[FirstJourneyStep],dbo.CustomerGuests.[Rating]
		,dbo.CustomerGuests.[IsActive]
    ,COUNT(*) OVER() as FilteredCount
                                    
    FROM [dbo].[CustomerGuests] WITH (NOLOCK)
        Inner Join dbo.CustomerReservations  WITH (NOLOCK)
		ON dbo.CustomerReservations.Id = dbo.CustomerGuests.CustomerReservationId
        WHERE dbo.CustomerGuests.DeletedAt is null
		And dbo.CustomerReservations.CustomerId = @CustomerId AND (
			dbo.CustomerGuests.Firstname LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.Lastname LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.Email LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.PhoneNumber LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.Country LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.IdProofNumber LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.[IdProofType] LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.Street LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.Postalcode LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.StreetNumber LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.ArrivalFlightNumber LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.DepartureFlightNumber LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.RoomNumber LIKE '%' + @SearchValue + '%'
			OR dbo.CustomerGuests.Rating LIKE '%' + @SearchValue + '%'             
            )
                                    
    ORDER BY
		CASE WHEN (@SortColumn = 'Firstname' AND @SortOrder='ASC')
        THEN Firstname
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'Firstname' AND @SortOrder='DESC')
        THEN Firstname
        END DESC,

		CASE WHEN (@SortColumn = 'Lastname' AND @SortOrder='ASC')
        THEN Lastname
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'Lastname' AND @SortOrder='DESC')
        THEN Lastname
        END DESC,

		CASE WHEN (@SortColumn = 'Email' AND @SortOrder='ASC')
        THEN Email
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'Email' AND @SortOrder='DESC')
        THEN Email
        END DESC,

		CASE WHEN (@SortColumn = 'PhoneNumber' AND @SortOrder='ASC')
        THEN PhoneNumber
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'PhoneNumber' AND @SortOrder='DESC')
        THEN PhoneNumber
        END DESC,

		CASE WHEN (@SortColumn = 'Country' AND @SortOrder='ASC')
        THEN Country
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'Country' AND @SortOrder='DESC')
        THEN Country
        END DESC,

		CASE WHEN (@SortColumn = 'IdProofNumber' AND @SortOrder='ASC')
        THEN IdProofNumber
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'IdProofNumber' AND @SortOrder='DESC')
        THEN IdProofNumber
        END DESC,

		CASE WHEN (@SortColumn = 'Street' AND @SortOrder='ASC')
        THEN Street
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'Street' AND @SortOrder='DESC')
        THEN Street
        END DESC,

		CASE WHEN (@SortColumn = 'Postalcode' AND @SortOrder='ASC')
        THEN Postalcode
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'Postalcode' AND @SortOrder='DESC')
        THEN Postalcode
        END DESC,

		CASE WHEN (@SortColumn = 'StreetNumber' AND @SortOrder='ASC')
        THEN StreetNumber
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'StreetNumber' AND @SortOrder='DESC')
        THEN StreetNumber
        END DESC,

		CASE WHEN (@SortColumn = 'ArrivalFlightNumber' AND @SortOrder='ASC')
        THEN ArrivalFlightNumber
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'ArrivalFlightNumber' AND @SortOrder='DESC')
        THEN ArrivalFlightNumber
        END DESC,

		CASE WHEN (@SortColumn = 'DepartureFlightNumber' AND @SortOrder='ASC')
        THEN DepartureFlightNumber
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'DepartureFlightNumber' AND @SortOrder='DESC')
        THEN DepartureFlightNumber
        END DESC,

		CASE WHEN (@SortColumn = 'RoomNumber' AND @SortOrder='ASC')
        THEN RoomNumber
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'RoomNumber' AND @SortOrder='DESC')
        THEN RoomNumber
        END DESC,

		CASE WHEN (@SortColumn = 'Rating' AND @SortOrder='ASC')
        THEN Rating
        END ASC,
                                           
		CASE WHEN (@SortColumn = 'Rating' AND @SortOrder='DESC')
        THEN Rating
        END DESC
                                    
    OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )
                                    
    select [Id],[CustomerReservationId],[Firstname],[Lastname],[Email],[Picture],[PhoneCountry],[PhoneNumber]
	,[Country],[Language],[IdProof],[IdProofType],[IdProofNumber],[BlePinCode],[Pin],[Street],[StreetNumber],[City],[Postalcode]
	,[ArrivalFlightNumber],[DepartureAirline],[DepartureFlightNumber],[Signature],[RoomNumber],[TermsAccepted],[FirstJourneyStep]
	,[Rating],[IsActive],FilteredCount 
    from CustomerGuests_Results
    OPTION (RECOMPILE)
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
