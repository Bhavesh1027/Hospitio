using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetTickets_SP_With_Filters_Modified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql(@"
                                    CREATE OR ALTER PROCEDURE [dbo].[GetTickets]
                                    (
                                    @CategoryId int=0,
                                    @Status int=0,
                                    @Priority int=0,
                                    @CustomerId int=0,
                                    @CSAgentId int=0,
                                    @FromCreate datetime=NULL,
                                    @ToCreate datetime=NULL,
                                    @FromClose datetime=NULL,
                                    @ToClose datetime=NULL,
                                    @PageNo INT = 1,
                                    @PageSize INT = 10,
                                    @ShortBy tinyint,  -- on CreateAt 1=Short by Date,2=Short By Month,3=Short By Year (modify in feture if need)
                                    @CreatedFrom tinyint --1 Fro Radfy ,2 for Customer
                                    )
                                    AS
                                    SET NOCOUNT ON 
                                    SET XACT_ABORT ON  
                                    
                                    SELECT dbo.Tickets.Id,dbo.Tickets.CustomerId,dbo.Customers.BusinessName,dbo.Customers.Cname as CustomerName, dbo.Customers.Email, dbo.Tickets.Title, dbo.Tickets.Details, dbo.Tickets.Priority, 
                                    dbo.Tickets.Duedate, dbo.Tickets.Status, dbo.Tickets.CloseDate, dbo.Tickets.CreatedFrom,
                                    Isnull(dbo.Users.FirstName,'') + ' ' + Isnull(dbo.Users.LastName,'') as CSAgentName, 
                                    dbo.TicketCategorys.CategoryName as TicketCategoryName, dbo.Tickets.IsActive, dbo.Tickets.CreatedAt, dbo.CustomerUsers.ProfilePicture
                                    FROM dbo.Tickets WITH (NOLOCK) 
                                    INNER JOIN dbo.TicketCategorys WITH (NOLOCK) ON dbo.Tickets.TicketCategoryId = dbo.TicketCategorys.Id 
                                    INNER JOIN dbo.Users WITH (NOLOCK) ON dbo.Tickets.CSAgentId = dbo.Users.Id 
                                    INNER JOIN dbo.Customers WITH (NOLOCK) ON dbo.Tickets.CustomerId = dbo.Customers.Id
                                    LEFT OUTER JOIN dbo.CustomerUsers ON dbo.Customers.Id = dbo.CustomerUsers.CustomerId
                                    Where (dbo.Tickets.DeletedAt IS NULL)
                                    AND (dbo.Tickets.TicketCategoryId=@CategoryId OR @CategoryId=0)
                                    AND (dbo.Tickets.Status=@Status OR @Status=0)
                                    AND (dbo.Tickets.Priority=@Priority OR @Priority=0)
                                    AND (dbo.Tickets.CustomerId=@CustomerId OR @CustomerId=0)
                                    AND (dbo.Tickets.CSAgentId=@CSAgentId OR @CSAgentId=0)
                                    AND (dbo.Tickets.CreatedFrom=@CreatedFrom OR @CreatedFrom=0)
                                    AND ((dbo.Tickets.CreatedAt BETWEEN @FromCreate AND @ToCreate) OR (@FromCreate IS NULL AND @ToCreate IS NULL))
                                    AND ((dbo.Tickets.CloseDate BETWEEN @FromClose AND @ToClose) OR (@FromClose IS NULL AND @ToClose IS NULL))
                                    --ORDER BY dbo.Tickets.CreatedAt Desc
                                    ORDER BY
                                        CASE WHEN @ShortBy = 1 THEN dbo.Tickets.CreatedAt 
                                    	     WHEN @ShortBy = 2 THEN MONTH(dbo.Tickets.CreatedAt) 
                                    		 WHEN @ShortBy = 3 THEN YEAR(dbo.Tickets.CreatedAt) 
                                    	END Desc
                                    OffSet @PageSize * (@PageNo-1) Rows
                                    Fetch Next @PageSize Rows Only
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
