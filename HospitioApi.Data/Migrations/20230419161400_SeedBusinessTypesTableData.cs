using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SeedBusinessTypesTableData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Add Business Types 
            migrationBuilder.Sql("INSERT INTO [dbo].[BusinessTypes]([BizType],[IsActive],[CreatedAt]) VALUES('Hotel',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[BusinessTypes]([BizType],[IsActive],[CreatedAt]) VALUES('Hostel',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[BusinessTypes]([BizType],[IsActive],[CreatedAt]) VALUES('Vacation Rental',1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[BusinessTypes]([BizType],[IsActive],[CreatedAt]) VALUES('Property Manager',1,GETUTCDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
