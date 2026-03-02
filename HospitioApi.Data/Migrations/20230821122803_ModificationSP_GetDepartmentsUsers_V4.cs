using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModificationSP_GetDepartmentsUsers_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[GetDepartmentsUsers] 
(
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'RecentUser',
    @SortOrder NVARCHAR(5) = 'DESC'
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH Users_Results
    AS (SELECT
            (
                SELECT [Id],
                       [Name],
					   (
                           SELECT TOP 1 ([us].[Id])
                           FROM [dbo].[Users] us (NOLOCK)
                           WHERE [us].UserLevelId = 2 and [us].IsActive = 1 and [us].DeletedAt is null
                       ) AS [CeoId],
					   (
                           SELECT TOP 1 ([us].[FirstName] + SPACE(1) + [us].[LastName])
                           FROM [dbo].[Users] us (NOLOCK)
                           WHERE [us].UserLevelId = 2 and [us].IsActive = 1 and [us].DeletedAt is null
                       ) AS [CeoName],
                       [DepartmentMangerId] AS [ManagerId],
                       (
                           SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName])
                           FROM [dbo].[Users] us (NOLOCK)
                           WHERE [dp].[DepartmentMangerId] = [us].[Id]
                       ) AS [ManagerName],
                       [IsActive],
                       (
                           SELECT [Id],
                                  [Name],
                                  [GroupLeaderId],
                                  (
                                      SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName])
                                      FROM [dbo].[Users] us (NOLOCK)
                                      WHERE [gp].[GroupLeaderId] = [us].[Id]
                                  ) AS [GroupLeader],
                                  [IsActive],
                                  (
                                      SELECT [Id],
                                             [FirstName],
                                             [LastName],
                                             [IsActive],
											 ([us].[FirstName] + SPACE(1) + [us].[LastName]) As Name
                                      FROM [dbo].[Users] us (NOLOCK)
                                      WHERE [us].[GroupId] = [gp].[Id]
											  AND [us].[UserLevelId] != (
											  SELECT 
											  [id]
											  FROM [UserLevels] as UL (NOLOCK)
											  WHERE [UL].[LevelName] = 'CEO')
                                            AND [us].[DeletedAt] IS NULL
											ORDER BY CASE
												 WHEN @SortColumn = 'Staff'
													  AND @SortOrder = 'ASC' THEN
													Name
											 END ASC,
											 CASE
												 WHEN @SortColumn = 'Staff'
													  AND @SortOrder = 'DESC' THEN
													 Name
											 END DESC,
											 CASE
												 WHEN @SortColumn = 'RecentUser'
													  AND @SortOrder = 'ASC' THEN
													 CreatedAt
											 END ASC,
											 CASE
												 WHEN @SortColumn = 'RecentUser'
													  AND @SortOrder = 'DESC' THEN
													 CreatedAt
											 END DESC
                                      FOR JSON PATH
                                  ) AS [UsersOut]
                           FROM [dbo].[groups] gp (NOLOCK)
                           WHERE [gp].[DepartmentId] = [dp].[Id]
                                 AND [gp].[DeletedAt] IS NULL
								 ORDER BY CASE
									 WHEN @SortColumn = 'Group'
										  AND @SortOrder = 'ASC' THEN
										 Name
								 END ASC,
								 CASE
									 WHEN @SortColumn = 'Group'
										  AND @SortOrder = 'DESC' THEN
										 Name
								 END DESC,
								 CASE
									 WHEN @SortColumn = 'GroupLeader'
										  AND @SortOrder = 'ASC' THEN
										 (
											  SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName])
											  FROM [dbo].[Users] us (NOLOCK)
											  WHERE [gp].[GroupLeaderId] = [us].[Id]
										  )
								 END ASC,
								 CASE
									 WHEN @SortColumn = 'GroupLeader'
										  AND @SortOrder = 'DESC' THEN
										 (
											  SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName])
											  FROM [dbo].[Users] us (NOLOCK)
											  WHERE [gp].[GroupLeaderId] = [us].[Id]
										  )
								 END DESC
                           FOR JSON PATH
                       ) AS [Groups],
                       COUNT(*) OVER () as [FilteredCount]
                FROM [dbo].[Departments] dp (NOLOCK)
                WHERE [DeletedAt] IS NULL
                      AND (
                              [dp].[Name] LIKE '%' + @SearchValue + '%'
                              OR
                              (
                                  SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName])
                                  FROM [dbo].[Users] us (NOLOCK)
                                  WHERE [dp].[DepartmentMangerId] = [us].[Id]
                              ) LIKE '%' + @SearchValue + '%'
                              OR
                              (
                                  SELECT ([gp].[Name])
                                  FROM [dbo].[Groups] gp (NOLOCK)
                                  WHERE [gp].[DepartmentId] = [dp].[Id]
                                  FOR JSON PATH
                              ) LIKE '%' + @SearchValue + '%'
                              OR EXISTS
                (
                    SELECT 1
                    FROM [dbo].[Groups] gp (NOLOCK)
                        JOIN [dbo].[Users] us (NOLOCK)
                            ON [gp].[GroupLeaderId] = [us].[Id]
                    WHERE [gp].[DepartmentId] = [dp].[Id]
                          AND CONCAT(   [gp].[Name],
                              (
                                  SELECT CONCAT([us].[FirstName], ' ', [us].[LastName])
                                  FROM [dbo].[Users] us (NOLOCK)
                                  WHERE [gp].[GroupLeaderId] = [us].[Id]
                              )
                                    ) LIKE '%' + @SearchValue + '%'
                )
                              OR EXISTS
                (
                    SELECT 1
                    FROM [dbo].[Groups] gp (NOLOCK)
                        JOIN [dbo].[Users] us (NOLOCK)
                            ON [us].[GroupId] = [gp].[Id]
                    WHERE [gp].[DepartmentId] = [dp].[Id]
                          AND (
                                  [us].[FirstName] LIKE '%' + @SearchValue + '%'
                                  OR [us].[LastName] LIKE '%' + @SearchValue + '%'
                              )
                )
                          )
				ORDER BY CASE
                     WHEN @SortColumn = 'Department'
                          AND @SortOrder = 'ASC' THEN
                         Name
                 END ASC,
                 CASE
                     WHEN @SortColumn = 'Department'
                          AND @SortOrder = 'DESC' THEN
                         Name
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Manager'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         (
							SELECT [us].[FirstName] + SPACE(1) + [us].[LastName]
							FROM [dbo].[Users] us (NOLOCK)
							WHERE [dp].[DepartmentMangerId] = [us].[Id]
						)
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Manager'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         (
							SELECT [us].[FirstName] + SPACE(1) + [us].[LastName]
							FROM [dbo].[Users] us (NOLOCK)
							WHERE [dp].[DepartmentMangerId] = [us].[Id]
						)
                 END DESC, 
				 CASE
					WHEN @SortColumn = 'Group' AND @SortOrder = 'ASC' THEN
						(
							SELECT [gp].[Name]
							FROM [dbo].[Groups] gp (NOLOCK)
							WHERE [gp].[DepartmentId] = [dp].[Id]
							ORDER BY Name ASC
							FOR JSON PATH
						)
					END ASC,
				CASE
					WHEN @SortColumn = 'Group' AND @SortOrder = 'DESC' THEN
						(
							SELECT [gp].[Name]
							FROM [dbo].[Groups] gp (NOLOCK)
							WHERE [gp].[DepartmentId] = [dp].[Id]
							ORDER BY Name ASC
							FOR JSON PATH
						)
				END DESC,
				CASE
					WHEN @SortColumn = 'GroupLeader' AND @SortOrder = 'ASC' THEN
						(SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName] ) As Name
							FROM [dbo].[Users] us (NOLOCK)
							WHERE [us].[Id] IN (
								SELECT [gp].[GroupLeaderId]
								FROM [dbo].[Groups] gp (NOLOCK)
								WHERE [gp].[DepartmentId] = [dp].[Id]
							) 
							ORDER BY Name ASC
							FOR JSON PATH)
				END ASC,
				CASE
					WHEN @SortColumn = 'GroupLeader' AND @SortOrder = 'DESC' THEN
						(
							SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName] ) As Name
							FROM [dbo].[Users] us (NOLOCK)
							WHERE [us].[Id] IN (
								SELECT [gp].[GroupLeaderId]
								FROM [dbo].[Groups] gp (NOLOCK)
								WHERE [gp].[DepartmentId] =[dp].[Id]
							) 
							ORDER BY Name ASC
							FOR JSON PATH
						)
				END DESC,
				CASE
					WHEN @SortColumn = 'Staff' AND @SortOrder = 'ASC' THEN
						(
                            SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName] ) As Name
                            FROM [dbo].[Users] us (NOLOCK)
                            WHERE [us].[DeletedAt] IS NULL
								AND [us].[GroupId] IN (
									SELECT [gp].[Id]
									FROM [dbo].[Groups] gp (NOLOCK)
									WHERE [gp].[DepartmentId] = [dp].[Id]
								)
							ORDER BY Name ASC
                            FOR JSON PATH
                        )
				END ASC,
				CASE
					WHEN @SortColumn = 'Staff' AND @SortOrder = 'DESC' THEN
						(
                            SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName] ) As Name
                            FROM [dbo].[Users] us (NOLOCK)
                            WHERE [us].[DeletedAt] IS NULL
								AND [us].[GroupId] IN (
									SELECT [gp].[Id]
									FROM [dbo].[Groups] gp (NOLOCK)
									WHERE [gp].[DepartmentId] = [dp].[Id]
								)
							ORDER BY Name ASC
                            FOR JSON PATH
                        )
				END DESC,
				CASE
					WHEN @SortColumn = 'RecentUser' AND @SortOrder = 'ASC' THEN
						(
                            SELECT CreatedAt
                            FROM [dbo].[Users] us (NOLOCK)
                            WHERE [us].[DeletedAt] IS NULL
								AND [us].[GroupId] IN (
									SELECT [gp].[Id]
									FROM [dbo].[Groups] gp (NOLOCK)
									WHERE [gp].[DepartmentId] = [dp].[Id]
								)
							ORDER BY CreatedAt ASC
                            FOR JSON PATH
                        )
				END ASC,
				CASE
					WHEN @SortColumn = 'RecentUser' AND @SortOrder = 'DESC' THEN
						(
                            SELECT CreatedAt
                            FROM [dbo].[Users] us (NOLOCK)
                            WHERE [us].[DeletedAt] IS NULL
								AND [us].[GroupId] IN (
									SELECT [gp].[Id]
									FROM [dbo].[Groups] gp (NOLOCK)
									WHERE [gp].[DepartmentId] = [dp].[Id]
								)
							ORDER BY CreatedAt DESC
                            FOR JSON PATH
                        )
				END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                FOR JSON PATH
            ) as [UserByIdOut]
       )
    SELECT *
    FROM Users_Results
    OPTION (RECOMPILE)
END

");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
