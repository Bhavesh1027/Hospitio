using Microsoft.EntityFrameworkCore.Migrations;
using HospitioApi.Data.Models;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class paymentprocessorsslist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetPaymentProcessors]    Script Date: 13-07-2023 10:48:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create or ALTER   proc [dbo].[GetPaymentProcessors]
As
BEGIN
SET NOCOUNT ON
SET XACT_ABORT ON

SELECT[Id],
[GRCategory],
[GRGroup],
[GRID],
[GRIcon],
[GRName]
FROM[dbo].[PaymentProcessors](NOLOCK)
WHERE[IsActive] = 1
And[DeletedAt] IS NULL

END");


        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
