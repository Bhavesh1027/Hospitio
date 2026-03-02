using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetleadSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.Sql(@"
            //GO
            ///****** Object:  StoredProcedure [dbo].[GetLeads]    Script Date: 01-05-2023 17:38:18 ******/
            //DROP PROCEDURE IF EXISTS [dbo].[GetLeads]

            //SET ANSI_NULLS ON
            //GO
            //SET QUOTED_IDENTIFIER ON
            //GO

            ///****** Object:  StoredProcedure [dbo].[GetLeads]    Script Date: 01-05-2023 17:38:18 ******/


            //CREATE Or Alter PROC [dbo].[GetLeads] (
            //                                        @SearchColumn NVARCHAR(50) = '',
            //                                        @SearchValue NVARCHAR(50) = '',
            //                                        @PageNo INT = 1,
            //                                        @PageSize INT = 10,
            //                                        @SortColumn NVARCHAR(20) = 'Firstname',
            //                                        @SortOrder NVARCHAR(5) = 'ASC'
            //                                    )

            //                                   AS 
            //                                   	SET NOCOUNT ON 
            //                                   	SET XACT_ABORT ON  

            //                                   	SELECT  [Id]
            //                                         ,[FirstName]
            //                                         ,[LastName]
            //                                         ,[Email]
            //                                         ,[Comment]
            //                                         ,[PhoneCountry]
            //                                         ,[PhoneNumber]
            //                                         ,[ContactFor]
            //                                         ,[IsActive]
            //                                         ,[CreatedAt]
            //                                         ,[UpdateAt]
            //                                         ,[DeletedAt]
            //                                         ,[CreatedBy]  from [dbo].[Leads] WITH (NOLOCK)
            //                                   	WHERE  [DeletedAt] is null
            //                                   	order by [FirstName]
            //                                   	OffSet @PageSize * (@PageNo-1) Rows
            //                                   	Fetch Next @PageSize Rows Only

            //GO
            //                "
            //             );

            //GetLeads



            //GetQuestionAnswers script
            migrationBuilder.Sql(@"GO
                                   /****** Object:  StoredProcedure [dbo].[GetQuestionAnswers]    Script Date: 01-05-2023 17:38:18 ******/
                                   DROP PROCEDURE IF EXISTS [dbo].[GetQuestionAnswers]
                                   
                                   SET ANSI_NULLS ON
                                   GO
                                   SET QUOTED_IDENTIFIER ON
                                   GO
                                   
                                   /****** Object:  StoredProcedure [dbo].[GetQuestionAnswers]   Script Date: 01-05-2023 17:38:18 ******/


                                    CREATE OR ALTER PROCEDURE [dbo].[GetQuestionAnswers]
                                    (
                                    	@SearchColumn NVARCHAR(50) = '',
                                        @SearchValue NVARCHAR(50) = '',
                                        @PageNo INT = 1,
                                        @PageSize INT = 10,
                                        @SortColumn NVARCHAR(20) = 'Firstname',
                                        @SortOrder NVARCHAR(5) = 'ASC'
                                    )
                                    AS
                                    BEGIN
                                    
                                       SET NOCOUNT ON;
                                    
                                       SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
                                       SET @SearchValue = LTRIM(RTRIM(@SearchValue))
                                    
                                       ; WITH GetQuestionAnswers_Results AS
                                       (
                                          SELECT  *  from [dbo].[QuestionAnswers] WITH (NOLOCK)
                                           WHERE DeletedAt is null AND @SearchColumn= '' OR  (
                                                   CASE @SearchColumn
                                                       WHEN 'Name' THEN Name
                                                   END
                                               ) LIKE '%' + @SearchValue + '%'
                                    
                                    	ORDER BY
                                    	CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')
                                           THEN Name
                                           END ASC,
                                           
                                    	CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')
                                           THEN Name
                                           END DESC
                                    
                                    	OFFSET @PageSize * (@PageNo - 1) ROWS
                                           FETCH NEXT @PageSize ROWS ONLY
                                       )
                                    
                                       select *
                                       from GetQuestionAnswers_Results
                                       OPTION (RECOMPILE)
                                    END
                                ");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
