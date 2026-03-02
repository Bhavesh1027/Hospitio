using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetNotifications_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetNotifications]    Script Date: 16-05-2023 19:53:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetNotifications]
(
@PageNo Int=1,
@PageSize Int=10
)

AS 
BEGIN
SET NOCOUNT ON;
; WITH Notification_Results AS
(
SELECT [Id]
,[Title]
,[Message]
,[CreatedAt]
,COUNT(*) OVER() as FilteredCount
FROM Notifications WITH (NOLOCK)
WHERE DeletedAt is null 
ORDER BY CreatedAt Desc
OFFSET @PageSize * (@PageNo - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY
)

SELECT [Id]
,[Title]
,[Message]
,[CreatedAt]
,FilteredCount 
from Notification_Results
OPTION (RECOMPILE)	
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
