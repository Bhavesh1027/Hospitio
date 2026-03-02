using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GetCustomerPropertyServiceByIdSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                    Create or ALTER   Procedure [dbo].[GetCustomerPropertyServiceById] 
                                    @Id int = 0
                                    AS
                                    BEGIN
                                    	SELECT CustomerPropertyServices.Id, CustomerPropertyServices.Name, CustomerPropertyServices.Icon, CustomerPropertyServices.Description, CustomerPropertyServices.IsActive
                                    	FROM CustomerPropertyServices
                                    	left join CustomerPropertyServiceImages on CustomerPropertyServices.Id = CustomerPropertyServiceImages.Id
                                    	where CustomerPropertyServices.Id = @Id AND CustomerPropertyServices.DeletedAt is null
                                    END
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
