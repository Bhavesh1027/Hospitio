using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddCustomerReservationRelatedSp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Get Customer Reservations
            migrationBuilder.Sql(@"
                                    CREATE OR ALTER PROCEDURE [dbo].[GetCustomerReservations]
                                    (
                                    	@CustomerId int = 0,
                                        @SearchColumn NVARCHAR(50) = '',
                                        @SearchValue NVARCHAR(50) = '',
                                        @PageNo INT = 1,
                                        @PageSize INT = 10,
                                        @SortColumn NVARCHAR(20) = 'ReservationNumber',
                                        @SortOrder NVARCHAR(5) = 'ASC'
                                    )
                                    AS
                                    BEGIN
                                    
                                    SET NOCOUNT ON;
                                    
                                        SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
                                        SET @SearchValue = LTRIM(RTRIM(@SearchValue))
                                    
                                        ; WITH CustomerReservations_Results AS
                                        (
                                            SELECT [Id],[CustomerId],[Uuid],[ReservationNumber],[Source],[NoOfGuestAdults],[NoOfGuestChildrens],[CheckinDate],[CheckoutDate],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy]
                                    		,COUNT(*) OVER() as FilteredCount
                                    
                                    		FROM [dbo].[CustomerReservations] WITH (NOLOCK)
                                    
                                            WHERE DeletedAt is null AND CustomerId = @CustomerId AND @SearchColumn= '' OR  (
                                                    CASE @SearchColumn
                                                        WHEN 'ReservationNumber' THEN ReservationNumber
                                                    END
                                                ) LIKE '%' + @SearchValue + '%'
                                    
                                    		ORDER BY
                                    		CASE WHEN (@SortColumn = 'ReservationNumber' AND @SortOrder='ASC')
                                            THEN ReservationNumber
                                            END ASC,
                                            
                                    		CASE WHEN (@SortColumn = 'ReservationNumber' AND @SortOrder='DESC')
                                            THEN ReservationNumber
                                            END DESC
                                    
                                    		OFFSET @PageSize * (@PageNo - 1) ROWS
                                            FETCH NEXT @PageSize ROWS ONLY
                                        )
                                    
                                        select [Id],[CustomerId],[Uuid],[ReservationNumber],[Source],[NoOfGuestAdults],[NoOfGuestChildrens],[CheckinDate],[CheckoutDate],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy],FilteredCount 
                                    	from CustomerReservations_Results
                                    	OPTION (RECOMPILE)
                                    END
                                ");

            // Get Customer Reservation by Id
            migrationBuilder.Sql(@"
                                    CREATE OR ALTER PROCEDURE [dbo].[GetCustomerReservationById]
                                    @Id int = 0
                                    AS
                                    BEGIN
                                    	SELECT [Id],[CustomerId],[Uuid],[ReservationNumber],[Source],[NoOfGuestAdults],[NoOfGuestChildrens],[CheckinDate],[CheckoutDate],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy]
                                        FROM [dbo].[CustomerReservations]
                                        where Id = @Id and DeletedAt is null
                                    END
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
