using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modified_GetTicketsSP_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetTickets]    Script Date: 06-06-2023 11:03:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER     PROCEDURE [dbo].[GetTickets]
(
--@CategoryId int=0,
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
@ShortBy tinyint = 0,  -- on CreateAt 1=Short by Date,2=Short By Month,3=Short By Year (modify in feture if need)
@CreatedFrom tinyint=0, --1 Fro Radfy ,2 for Customer,
@ApplyPagination bit=0
)
AS
SET NOCOUNT ON 
SET XACT_ABORT ON  
                                    
IF @ApplyPagination = 1
BEGIN
SELECT dbo.Tickets.Id,dbo.Tickets.CustomerId,dbo.Customers.BusinessName,dbo.Customers.Cname as CustomerName, dbo.Customers.Email, dbo.Tickets.Title, dbo.Tickets.Details, dbo.Tickets.Priority, 
dbo.Tickets.Duedate, dbo.Tickets.Status, dbo.Tickets.CloseDate, dbo.Tickets.CreatedFrom,
CASE
	WHEN dbo.Tickets.CreatedFrom = 2 AND dbo.Tickets.CSAgentId IS NULL THEN NULL
	ELSE Isnull(dbo.Users.FirstName,'') + ' ' + Isnull(dbo.Users.LastName,'')
END as CSAgentName,  
dbo.Tickets.IsActive, dbo.Tickets.CreatedAt, dbo.CustomerGuestsCheckInFormBuilders.Logo as ProfilePicture,COUNT(*) OVER() as FilteredCount
FROM dbo.Tickets WITH (NOLOCK) 
INNER JOIN dbo.Customers WITH (NOLOCK) ON dbo.Tickets.CustomerId = dbo.Customers.Id
LEFT JOIN dbo.CustomerGuestsCheckInFormBuilders ON dbo.Customers.Id = dbo.CustomerGuestsCheckInFormBuilders.CustomerId
LEFT JOIN dbo.Users WITH (NOLOCK) ON dbo.Tickets.CSAgentId = dbo.Users.Id AND dbo.Tickets.CreatedFrom <> 2
Where (dbo.Tickets.DeletedAt IS NULL)
--AND (dbo.Tickets.TicketCategoryId=@CategoryId OR @CategoryId=0)
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
END

IF @ApplyPagination = 0
BEGIN
SELECT dbo.Tickets.Id,dbo.Tickets.CustomerId,dbo.Customers.BusinessName,dbo.Customers.Cname as CustomerName, dbo.Customers.Email, dbo.Tickets.Title, dbo.Tickets.Details, dbo.Tickets.Priority, 
dbo.Tickets.Duedate, dbo.Tickets.Status, dbo.Tickets.CloseDate, dbo.Tickets.CreatedFrom,
CASE
	WHEN dbo.Tickets.CreatedFrom = 2 AND dbo.Tickets.CSAgentId IS NULL THEN NULL
	ELSE Isnull(dbo.Users.FirstName,'') + ' ' + Isnull(dbo.Users.LastName,'')
END as CSAgentName, 
dbo.Tickets.IsActive, dbo.Tickets.CreatedAt, dbo.CustomerGuestsCheckInFormBuilders.Logo as ProfilePicture,COUNT(*) OVER() as FilteredCount
FROM dbo.Tickets WITH (NOLOCK) 
--INNER JOIN dbo.TicketCategorys WITH (NOLOCK) ON dbo.Tickets.TicketCategoryId = dbo.TicketCategorys.Id 
INNER JOIN dbo.Customers WITH (NOLOCK) ON dbo.Tickets.CustomerId = dbo.Customers.Id
LEFT JOIN dbo.CustomerGuestsCheckInFormBuilders ON dbo.Customers.Id = dbo.CustomerGuestsCheckInFormBuilders.CustomerId
LEFT JOIN dbo.Users WITH (NOLOCK) ON dbo.Tickets.CSAgentId = dbo.Users.Id AND dbo.Tickets.CreatedFrom <> 2

Where (dbo.Tickets.DeletedAt IS NULL)
--AND (dbo.Tickets.TicketCategoryId=@CategoryId OR @CategoryId=0)
AND (dbo.Tickets.Status=@Status OR @Status=0)
AND (dbo.Tickets.Priority=@Priority OR @Priority=0)
AND (dbo.Tickets.CustomerId=@CustomerId OR @CustomerId=0)
AND (dbo.Tickets.CSAgentId=@CSAgentId OR @CSAgentId=0)
AND (dbo.Tickets.CreatedFrom=@CreatedFrom OR @CreatedFrom=0)
AND ((dbo.Tickets.CreatedAt BETWEEN @FromCreate AND @ToCreate) OR (@FromCreate IS NULL AND @ToCreate IS NULL))
AND ((dbo.Tickets.CloseDate BETWEEN @FromClose AND @ToClose) OR (@FromClose IS NULL AND @ToClose IS NULL))
END
                "
          );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
