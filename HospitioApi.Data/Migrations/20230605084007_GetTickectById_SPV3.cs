using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetTickectById_SPV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetTicketById]    Script Date: 05-06-2023 14:12:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetTicketById] -- 13
(
    @Id int = 1
)
AS
SET NOCOUNT ON
SET XACT_ABORT ON

SELECT 
(
    SELECT
        t.[Id], t.[Title], t.Details, t.CloseDate, t.CreatedFrom, t.CSAgentId,
        CASE
            WHEN t.CSAgentId IS NULL THEN ''
            ELSE ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '')
        END AS CSAgentName,
        t.CustomerId, ISNULL(C.BusinessName, '') as CustomerName,
        t.Duedate, t.Priority, t.Status, t.CreatedAt,
        JSON_QUERY(
            (
                SELECT
                    TR.Id, TR.Reply, TR.TicketId, TR.CreatedAt, TR.CreatedBy, TR.CreatedFrom,
					CONCAT_WS(' ', COALESCE(U.FirstName, CU.FirstName), COALESCE(U.LastName, CU.LastName)) AS UserName
                FROM
                    TicketReplies TR
				LEFT JOIN Users U ON TR.CreatedBy = 1 AND U.Id = TR.CreatedFrom
				LEFT JOIN CustomerUsers CU ON TR.CreatedBy = 2 AND CU.Id = TR.CreatedFrom
				--LEFT JOIN Tickets T ON TR.TicketId = T.Id
                WHERE
                    TR.TicketId = t.Id and TR.DeletedAt is null
                FOR JSON PATH
            )
        ) as [Replies]
    FROM
        Tickets t
    LEFT JOIN
        Users U ON U.Id = t.CSAgentId
    INNER JOIN
        Customers C ON C.Id = t.CustomerId
    WHERE
        t.Id = @Id AND t.DeletedAt is null
    FOR JSON PATH
);
                "
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
