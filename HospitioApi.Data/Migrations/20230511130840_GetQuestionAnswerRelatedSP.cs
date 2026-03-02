using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetQuestionAnswerRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetQuestionAnswers]    Script Date: 23-05-2023 14:48:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[GetQuestionAnswers]  
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = '',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CategoryId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH CTE_Results AS
    (
        SELECT QA.[Id]
      ,QA.[QuestionAnswerCategoryId]
      ,QA.[Name]
      ,QA.[Description]
      ,QA.[Icon]
      ,QA.[IsActive]
      ,QA.[IsPublish]
	  ,QAA.[Id] as QuestionAnswerAttachementId
	  ,QAA.[AttachmentType]
	  ,QAA.[Attachment]
	  ,COUNT(*) OVER() as FilteredCount
	  FROM dbo.QuestionAnswers QA WITH (NOLOCK) 
	  Inner Join dbo.QuestionAnswerAttachements QAA WITH (NOLOCK)
	  ON QA.Id = QAA.QuestionAnswerId

        WHERE QA.DeletedAt is null AND(@CategoryId is null or QA.QuestionAnswerCategoryId = @CategoryId) AND (
                Name LIKE '%' + @SearchValue + '%' OR 
                Description LIKE '%' + @SearchValue + '%' 
            )

		ORDER BY
		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')
        THEN Name
        END ASC,
        
		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')
        THEN Name
        END DESC,

		CASE WHEN (@SortColumn = 'Description' AND @SortOrder='ASC')
        THEN Description
        END ASC,
        
		CASE WHEN (@SortColumn = 'Description' AND @SortOrder='DESC')
        THEN Description
        END DESC

		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select [Id]
      ,[QuestionAnswerCategoryId]
      ,[Name]
      ,[Description]
      ,[Icon]
      ,[IsActive]
      ,[IsPublish]
	  ,FilteredCount 
	  ,QuestionAnswerAttachementId
	  ,[AttachmentType]
	  ,[Attachment]
	from CTE_Results
	OPTION (RECOMPILE)
	--With Total Rows
    --,CTE_TotalRows AS
    --(
    --    select count(ID) as TotalRows from Modules
    --    WHERE @SearchColumn= '' OR  (
    --            CASE @SearchColumn
    --                WHEN 'Name' THEN Name
    --                --WHEN 'ModuleType' THEN ModuleType
    --            END
    --        ) LIKE '%' + @SearchValue + '%'
    --)
    --Select TotalRows, t.Id, t.Name, t.ModuleType,t.IsActive from dbo.Modules as t, CTE_TotalRows
    --WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = t.ID)

    
END
                
"
             );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
