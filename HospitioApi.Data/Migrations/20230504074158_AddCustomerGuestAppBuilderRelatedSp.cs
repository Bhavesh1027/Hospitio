using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddCustomerGuestAppBuilderRelatedSp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Get Customer Guest App Builders
            migrationBuilder.Sql(@"
                                    CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestAppBuilders]
                                    (
                                     	@CustomerId int = 0,
                                         @SearchColumn NVARCHAR(50) = '',
                                         @SearchValue NVARCHAR(50) = '',
                                         @PageNo INT = 1,
                                         @PageSize INT = 10,
                                         @SortColumn NVARCHAR(20) = 'CustomerRoomNameId',
                                         @SortOrder NVARCHAR(5) = 'ASC'
                                     )
                                     AS
                                     BEGIN
                                     
                                     SET NOCOUNT ON;
                                     
                                         SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
                                         SET @SearchValue = LTRIM(RTRIM(@SearchValue))
                                     
                                         ; WITH CustomerGuestAppBuilders_Results AS
                                         (
                                             SELECT [Id],[CustomerId],[CustomerRoomNameId],[Message],[SecondaryMessage],[LocalExperience],[Ekey],[PropertyInfo],[EnhanceYourStay],[Reception],[Housekeeping],[RoomService],[Concierge],[TransferServices],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy]
                                     		,COUNT(*) OVER() as FilteredCount
                                     
                                     		FROM [dbo].[CustomerGuestAppBuilders] WITH (NOLOCK)
                                     
                                             WHERE DeletedAt is null AND CustomerId = @CustomerId AND @SearchColumn= '' OR  (
                                                     CASE @SearchColumn
                                                         WHEN 'CustomerRoomNameId' THEN CustomerRoomNameId
                                                     END
                                                 ) LIKE '%' + @SearchValue + '%'
                                     
                                     		ORDER BY
                                     		CASE WHEN (@SortColumn = 'CustomerRoomNameId' AND @SortOrder='ASC')
                                             THEN CustomerRoomNameId
                                             END ASC,
                                             
                                     		CASE WHEN (@SortColumn = 'CustomerRoomNameId' AND @SortOrder='DESC')
                                             THEN CustomerRoomNameId
                                             END DESC
                                     
                                     		OFFSET @PageSize * (@PageNo - 1) ROWS
                                             FETCH NEXT @PageSize ROWS ONLY
                                         )
                                     
                                         select [Id],[CustomerId],[CustomerRoomNameId],[Message],[SecondaryMessage],[LocalExperience],[Ekey],[PropertyInfo],[EnhanceYourStay],[Reception],[Housekeeping],[RoomService],[Concierge],[TransferServices],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy],FilteredCount 
                                     	from CustomerGuestAppBuilders_Results
                                     	OPTION (RECOMPILE)
                                     END
                                ");

            // Get Customer Guest App Builder by Id
            migrationBuilder.Sql(@"
                                    CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestAppBuilderById]
                                     @Id int = 0
                                    AS
                                    BEGIN
                                    	SELECT [Id],[CustomerId],[CustomerRoomNameId],[Message],[SecondaryMessage],[LocalExperience],[Ekey],[PropertyInfo],[EnhanceYourStay],[Reception],[Housekeeping],[RoomService],[Concierge],[TransferServices],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy]
                                    	FROM [dbo].[CustomerGuestAppBuilders]
                                        where Id = @Id and DeletedAt is null
                                    END
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
