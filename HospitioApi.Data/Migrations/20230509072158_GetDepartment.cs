using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Get Department

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetDepartmentById]    Script Date: 09-05-2023 12:53:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create OR ALTER Procedure [dbo].[GetDepartmentById]
@Id int=1
as
select d.Id,d.Name,d.DepartmentMangerId  from Departments d
where d.DeletedAt is null 
and d.IsActive =1
and d.Id = @Id");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetDepartments]    Script Date: 09-05-2023 12:53:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create OR ALTER Procedure [dbo].[GetDepartments]
as
select Id,Name,DepartmentMangerId from Departments 
where DeletedAt is null 
and IsActive = 1");

            // Get Customer Property Extra 

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyExtraById]    Script Date: 15-05-2023 12:19:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerPropertyExtraById]
@Id int=0
as
select * from CustomerPropertyExtras 
where DeletedAt is null 
and Id = @Id");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyExtras]    Script Date: 15-05-2023 12:20:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerPropertyExtras]

as
select * from CustomerPropertyExtras 
where DeletedAt is null");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
