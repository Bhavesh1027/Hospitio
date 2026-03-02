using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CreateSP_UpdateQustionAnswerReadAndModifySP_GetTotalUnreadQACount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_UpdateQustionAnswerRead] 
(
    @UserId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

	 IF EXISTS (
        SELECT 1
			FROM QuestionAnswers 
				WHERE Id NOT IN (
					SELECT QuestionAnswerId
					FROM QuestionAnswersRead
					WHERE UserId = @UserId
				) 
				AND DeletedAt IS NULL
				AND IsActive = 1
    )
    BEGIN
      
        INSERT INTO [dbo].[QuestionAnswersRead] ([QuestionAnswerId], [UserId], [IsActive],[CreatedAt],[UpdateAt],[CreatedBy])
		SELECT Id,@UserId,1,GETUTCDATE(),GETUTCDATE(),@UserId
			FROM QuestionAnswers 
				WHERE Id NOT IN (
					SELECT QuestionAnswerId
					FROM QuestionAnswersRead
					WHERE UserId = @UserId
				) 
				AND DeletedAt IS NULL
				AND IsActive = 1
    END

   
    SELECT COUNT(Id) AS [TotalUnReadCount]
	FROM QuestionAnswers 
		WHERE Id NOT IN (
			SELECT QuestionAnswerId
			FROM QuestionAnswersRead
			WHERE UserId = @UserId
		) 
		AND DeletedAt IS NULL
		AND IsActive = 1
END

");

            migrationBuilder.Sql(@"CREATE OR ALTER       PROCEDURE [dbo].[SP_GetTotalUnreadQACount]
(
    @UserId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;

	SELECT COUNT(Id) AS [TotalUnReadCount]
	FROM QuestionAnswers 
		WHERE 
		DeletedAt IS NULL
		AND IsActive = 1
		AND Id NOT IN (
			SELECT QuestionAnswerId
			FROM QuestionAnswersRead
			WHERE UserId = @UserId
		)
						
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
