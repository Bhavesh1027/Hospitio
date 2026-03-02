using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetGetQuestionAnswers_SP_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	exec [dbo].[GetQuestionAnswers] '',1,10,'','',0,0
*/

CREATE OR ALTER     PROC [dbo].[GetQuestionAnswers]  
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC',
    @CategoryId INT = 0,
	@IsViewAll BIT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH CTE_Results
    AS (SELECT [QA].[Id],
               [QA].[QuestionAnswerCategoryId],
               [QA].[Name],
               [QA].[Description],
               [QA].[Icon],
               [QA].[IsActive],
               [QA].[IsPublish],
               [QAA].[Id] AS [QuestionAnswerAttachementId],
               [QAA].[AttachmentType],
               [QAA].[Attachment],
			   CASE 
					WHEN ([QA].CreatedAt >= DATEADD(day, -1, GETDATE()) AND [QA].CreatedAt = [QA].UpdateAt) THEN  'New'	 
					WHEN [QA].UpdateAt >= DATEADD(day, -1, GETDATE()) THEN 'Update'
					WHEN (CAST([QA].CreatedAt AS DATE) < CAST(GETDATE() AS DATE) ) THEN 'Old'
			   END AS [Status],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[QuestionAnswers] QA WITH (NOLOCK)
            INNER JOIN [dbo].[QuestionAnswerAttachements] QAA WITH (NOLOCK)
                ON [QA].[Id] = [QAA].[QuestionAnswerId]
        WHERE [QA].[DeletedAt] IS NULL
              AND (
                      [QA].[QuestionAnswerCategoryId] = @CategoryId
                      OR 0 = @CategoryId
                  )
              AND (
                      [Name] LIKE '%' + @SearchValue + '%'
                      OR [Description] LIKE '%' + @SearchValue + '%'
                  )
			  AND (
                      @IsViewAll = 1
					  OR (
						 @IsViewAll = 0
						 AND (([QA].CreatedAt >= DATEADD(day, -1, GETDATE()) AND [QA].CreatedAt = [QA].UpdateAt) OR [QA].UpdateAt >= DATEADD(day, -1, GETDATE()))
						 )
                  )
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         [Name]
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         [Name]
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Description'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         [Description]
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Description'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         [Description]
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [QuestionAnswerCategoryId],
           [Name],
           [Description],
           [Icon],
           [IsActive],
           [IsPublish],
           [FilteredCount],
           [QuestionAnswerAttachementId],
           [AttachmentType],
           [Attachment],
		   [Status]
    FROM CTE_Results
    OPTION (RECOMPILE)

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
