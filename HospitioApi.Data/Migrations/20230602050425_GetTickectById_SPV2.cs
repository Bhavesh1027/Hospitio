using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetTickectById_SPV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetTicketById]    Script Date: 02-06-2023 10:32:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetTicketById] -- 13
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
        t.CustomerId, ISNULL(C.FirstName, '') + ' ' + ISNULL(C.LastName, '') as CustomerName,
        t.Duedate, t.Priority, t.Status, t.CreatedAt,
        JSON_QUERY(
            (
                SELECT
                    TR.Id, TR.Reply, TR.TicketId, TR.CreatedAt
                FROM
                    TicketReplies TR
                WHERE
                    TicketId = t.Id
                FOR JSON PATH
            )
        ) as [Replies]
    FROM
        Tickets t
    LEFT JOIN
        Users U ON U.Id = t.CSAgentId
    INNER JOIN
        CustomerUsers C ON C.Id = t.CustomerId
    WHERE
        t.Id = @Id
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
