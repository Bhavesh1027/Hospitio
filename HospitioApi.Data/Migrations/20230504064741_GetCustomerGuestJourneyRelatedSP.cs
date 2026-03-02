using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestJourneyRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneys]    Script Date: 04-05-2023 12:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomersGuestJourneys] --'','',1,2,'Name','ASC',1
(
	@SearchColumn NVARCHAR(50) = null,
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH CTE_Results AS
    (
        SELECT [Id]
      ,[CutomerId]
      ,[JourneyStep]
      ,[Name]
      ,[SendType]
      ,[TimingOption1]
      ,[TimingOption2]
      ,[TimingOption3]
      ,[Timing]
      ,[NotificationDays]
      ,[NotificationTime]
      ,[GuestJourneyMessagesTemplateId]
      ,[TempletMessage]
      ,[IsActive]
	  ,COUNT(*) OVER() as FilteredCount
	  FROM CustomerGuestJournies WITH (NOLOCK)

        WHERE DeletedAt is null AND CutomerId = @CustomerId AND @SearchColumn= '' OR  (
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

	select [Id]
      ,[CutomerId]
      ,[JourneyStep]
      ,[Name]
      ,[SendType]
      ,[TimingOption1]
      ,[TimingOption2]
      ,[TimingOption3]
      ,[Timing]
      ,[NotificationDays]
      ,[NotificationTime]
      ,[GuestJourneyMessagesTemplateId]
      ,[TempletMessage]
      ,[IsActive]
	  ,FilteredCount 
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

            // Get Customer Guest Journey by Id
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneysById]    Script Date: 15-05-2023 17:28:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomersGuestJourneysById] 
    @Id Int=1
	
	
AS 

	SELECT [Id]
      ,[CutomerId]
      ,[JourneyStep]
      ,[Name]
      ,[SendType]
      ,[TimingOption1]
      ,[TimingOption2]
      ,[TimingOption3]
      ,[Timing]
      ,[NotificationDays]
      ,[NotificationTime]
      ,[GuestJourneyMessagesTemplateId]
      ,[TempletMessage]
	FROM   [dbo].[CustomerGuestJournies] WITH (NOLOCK)
	WHERE  Id = @Id and DeletedAt is null
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
