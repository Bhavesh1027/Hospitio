using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetQuestionAnswerByIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetQuestionAnswerById]    Script Date: 12-05-2023 12:03:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetQuestionAnswerById] --4
@Id int
AS

 
SELECT 
( 
SELECT q.[Id], q.[QuestionAnswerCategoryId], q.[Name], q.[Description], q.[Icon], q.[IsActive], q.[IsPublish]
,JSON_QUERY(( 
SELECT [Id], [Attachment], [AttachmentType]
FROM QuestionAnswerAttachements where QuestionAnswerId=q.Id
FOR JSON PATH
)) as [QuestionAnswerAttachements]
FROM QuestionAnswers q where DeletedAt is null and q.Id = @Id
FOR JSON PATH )

                "
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
