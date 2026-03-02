using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetTicketsByUserId_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER  PROC [dbo].[GetTicketsByUserId] 
	(
	@Id INT = 1
	) 
	AS 
	BEGIN 
		SET NOCOUNT ON 
		SET XACT_ABORT ON 
		DECLARE @GroupId INT = (
		    SELECT 
		      [U].[GroupId] 
		    FROM 
		      [dbo].[Users] U (NOLOCK) 
		    WHERE 
		      [U].[Id] = @Id 
		      AND [U].[DeletedAt] IS NULL
		  )
		  DECLARE @LevelId INT = (
		    SELECT 
		      [U].[UserLevelId] 
		    FROM 
		      [dbo].[Users] U (NOLOCK) 
		    WHERE 
		      [U].[Id] = @Id 
		      AND [U].[DeletedAt] IS NULL
		  )
		  
		  IF(@LevelId = 1)
		  BEGIN 
			SELECT 
			  [T].[Id], 
			  [T].[CustomerId], 
			  [C].[Cname] as [CustomerName], 
			  [C].[BusinessName], 
			  [T].[Title], 
			  [T].[Details], 
			  [T].[Priority], 
			  [T].[Duedate], 
			  [T].[CSAgentId], 
			  ([U].[FirstName]+ ' '+ [U].[LastName]) as [CSAgentName], 
			  [T].[TicketCategoryId], 
			  [Tc].[CategoryName] as [TicketCategoryName], 
			  [T].[Status], 
			  [T].[CloseDate], 
			  [T].[CreatedFrom]			 
			FROM 
			  [dbo].[Tickets] T (NOLOCK) 
			  INNER  JOIN [dbo].[Users] U (NOLOCK) on [U].[Id] = [T].[CSAgentId] 
			  AND U.DeletedAt IS NULL 
			  INNER  JOIN [dbo].[Customers] C (NoLock) on [C].[ID] = [T].[CustomerId] 
			  AND C.DeletedAt IS NULL 
			  INNER  JOIN [dbo].[TicketCategorys] Tc (NoLock) on [Tc].[Id] = [T].[TicketCategoryId] 
			  AND Tc.DeletedAt IS NULL 
		  END

		ELSE
		  BEGIN 
			  SELECT 
			  [T].[Id], 
			  [T].[CustomerId], 
			  [C].[Cname] as [CustomerName], 
			  [C].[BusinessName], 
			  [T].[Title], 
			  [T].[Details], 
			  [T].[Priority], 
			  [T].[Duedate], 
			  [T].[CSAgentId], 
			  ([U].[FirstName]+ ' '+ [U].[LastName]) as [CSAgentName], 
			  [T].[TicketCategoryId], 
			  [Tc].[CategoryName] as [TicketCategoryName], 
			  [T].[Status], 
			  [T].[CloseDate], 
			  [T].[CreatedFrom]
			FROM 
			  [dbo].[Tickets] T (NOLOCK) 
			  INNER  JOIN [dbo].[Users] U (NOLOCK) on [U].[Id] = [T].[CSAgentId] 
			  AND U.DeletedAt IS NULL 
			  INNER  JOIN [dbo].[Customers] C (NoLock) on [C].[ID] = [T].[CustomerId] 
			  AND C.DeletedAt IS NULL 
			  INNER  JOIN [dbo].[TicketCategorys] Tc (NoLock) on [Tc].[Id] = [T].[TicketCategoryId] 
			  AND Tc.DeletedAt IS NULL 
			WHERE 
			  [T].[GroupId] = @GroupId 
			  AND [T].[CSAgentId] IS NULL 
			  AND [T].[DeletedAt] IS NULL 
			UNION 
			SELECT 
			  [T].[Id], 
			  [T].[CustomerId], 
			  [C].[Cname] as [CustomerName], 
			  [C].[BusinessName], 
			  [T].[Title], 
			  [T].[Details], 
			  [T].[Priority], 
			  [T].[Duedate], 
			  [T].[CSAgentId], 
			  ([U].[FirstName]+ ' '+ [U].[LastName]) as [CSAgentName], 
			  [T].[TicketCategoryId], 
			  [Tc].[CategoryName] as [TicketCategoryName], 
			  [T].[Status], 
			  [T].[CloseDate], 
			  [T].[CreatedFrom]
			FROM 
			  [dbo].[Tickets] T (NOLOCK) 
			  INNER  JOIN [dbo].[Users] U (NOLOCK) on [U].[Id] = [T].[CSAgentId] 
			  AND U.DeletedAt IS NULL 
			  INNER  JOIN [dbo].[Customers] C (NoLock) on [C].[ID] = [T].[CustomerId] 
			  AND C.DeletedAt IS NULL 
			  INNER  JOIN [dbo].[TicketCategorys] Tc (NoLock) on [Tc].[Id] = [T].[TicketCategoryId] 
			  AND Tc.DeletedAt IS NULL 
			WHERE 
			  [T].[CSAgentId] = @Id 
			  AND [T].[DeletedAt] IS NULL 
		  END 
  END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
