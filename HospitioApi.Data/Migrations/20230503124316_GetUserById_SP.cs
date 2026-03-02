using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetUserById_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetUserById]    Script Date: 10-05-2023 17:41:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 Create or ALTER Procedure [dbo].[GetUserById]
            @Id Int=1
           as
           SELECT 
( 
SELECT u.[Id]
      ,u.[FirstName]
      ,u.[LastName]
      ,u.[Email]
      ,u.[Title]
      ,u.[ProfilePicture]
      ,u.[PhoneCountry]
      ,u.[PhoneNumber]
      ,u.[DepartmentId]
      ,u.[UserLevelId]
      ,u.[SupervisorId]
      ,u.[UserName]
      ,u.[Password]
      ,u.[IsActive]
      ,u.[GroupId]
,JSON_QUERY(( 
SELECT [Id]
      ,[PermissionId]
      ,[UserId]
      ,[IsView]
      ,[IsEdit]
      ,[IsUpload]
      ,[IsReply]
      ,[IsSend]
      ,[IsActive]
FROM UsersPermissions where UserId=u.Id
FOR JSON PATH
)) as [UserModulePermissions]
FROM Users u  where u.Id = @Id
FOR JSON PATH )");


            //GetallusersDeptwise

            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetDepartmentsUsers]    Script Date: 15-05-2023 17:40:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetDepartmentsUsers] -- 'dfhg',1,10
(
	
    --@SearchColumn NVARCHAR(50) = '',
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN

   SET NOCOUNT ON;

   --SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
   SET @SearchValue = LTRIM(RTRIM(@SearchValue))

   ; WITH Users_Results AS
   (
    SELECT 
        ( 
        SELECT Id,Name, DepartmentMangerId as ManagerId,(select (us.FirstName + SPACE(1) + us.LastName) from Users us where dp.DepartmentMangerId =us.id ) as ManagerName , IsActive,
            (SELECT Id, Name,GroupLeaderId, (select (us.FirstName + SPACE(1) + us.LastName) from Users us where gp.GroupLeaderId =us.id ) as GroupLeader , IsActive,
        	     (SELECT Id,FirstName,LastName,IsActive
                FROM users us
                WHERE us.GroupId = gp.Id
                FOR JSON PATH) AS UsersOut
        
                FROM groups gp
                WHERE gp.DepartmentId = dp.Id
                FOR JSON PATH) AS Groups,COUNT(*)
 OVER() as FilteredCount
        FROM Departments dp
        WHERE DeletedAt is null  
		AND (dp.Name LIKE '%' + @SearchValue + '%'
		OR (
                            SELECT (us.FirstName + SPACE(1) + us.LastName)
                            FROM Users us
                            WHERE dp.DepartmentMangerId = us.id
                        ) LIKE '%' + @SearchValue + '%' 
						OR (
                            SELECT (gp.Name)
                            FROM Groups gp
                            WHERE gp.DepartmentId = dp.Id
                            FOR JSON PATH
                        ) LIKE '%' + @SearchValue + '%'
						 OR EXISTS (
                    SELECT 1
                    FROM
                        Groups gp
                        JOIN Users us ON gp.GroupLeaderId = us.Id
                    WHERE
                        gp.DepartmentId = dp.Id
                        AND CONCAT(gp.Name, (SELECT CONCAT(us.FirstName, ' ', us.LastName) FROM Users us WHERE gp.GroupLeaderId = us.Id)) LIKE '%' + @SearchValue + '%'
                )
                OR EXISTS (
                    SELECT 1
                    FROM
                        Groups gp
                        JOIN Users us ON us.GroupId = gp.Id
                    WHERE
                        gp.DepartmentId = dp.Id
                        AND (us.FirstName LIKE '%' + @SearchValue + '%' OR us.LastName LIKE '%' + @SearchValue + '%')
						
						)
						)
	ORDER BY
	CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')
       THEN Name
       END ASC,
       
	CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')
       THEN Name
       END DESC
	   OFFSET @PageSize * (@PageNo - 1) ROWS
       FETCH NEXT @PageSize ROWS ONLY
	
         FOR JSON PATH ) as UserByIdOut
   )

   select *
   from Users_Results
   OPTION (RECOMPILE)
END
                      ");



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
