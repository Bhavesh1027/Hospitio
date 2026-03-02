using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class NamingModificationsOfTablesAndColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuesionAnswerAttachements");

            migrationBuilder.DropTable(
                name: "QuesionAnswers");

            migrationBuilder.DropTable(
                name: "QuesionAnswerCategories");

            migrationBuilder.RenameColumn(
                name: "Ttitle",
                table: "Tickets",
                newName: "Title");

            migrationBuilder.CreateTable(
                name: "QuestionAnswerCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionAnswerCategoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_QuestionAnswerCategories_QuestionAnswerCategoryId",
                        column: x => x.QuestionAnswerCategoryId,
                        principalTable: "QuestionAnswerCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswerAttachements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionAnswerId = table.Column<int>(type: "int", nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AttachmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerAttachements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerAttachements_QuestionAnswers_QuestionAnswerId",
                        column: x => x.QuestionAnswerId,
                        principalTable: "QuestionAnswers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerAttachements_QuestionAnswerId",
                table: "QuestionAnswerAttachements",
                column: "QuestionAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_QuestionAnswerCategoryId",
                table: "QuestionAnswers",
                column: "QuestionAnswerCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionAnswerAttachements");

            migrationBuilder.DropTable(
                name: "QuestionAnswers");

            migrationBuilder.DropTable(
                name: "QuestionAnswerCategories");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Tickets",
                newName: "Ttitle");

            migrationBuilder.CreateTable(
                name: "QuesionAnswerCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuesionAnswerCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuesionAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QacategorieId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuesionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuesionAnswers_QuesionAnswerCategories_QacategorieId",
                        column: x => x.QacategorieId,
                        principalTable: "QuesionAnswerCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuesionAnswerAttachements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuesionAnswerId = table.Column<int>(type: "int", nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AttachmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuesionAnswerAttachements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuesionAnswerAttachements_QuesionAnswers_QuesionAnswerId",
                        column: x => x.QuesionAnswerId,
                        principalTable: "QuesionAnswers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuesionAnswerAttachements_QuesionAnswerId",
                table: "QuesionAnswerAttachements",
                column: "QuesionAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuesionAnswers_QacategorieId",
                table: "QuesionAnswers",
                column: "QacategorieId");
        }
    }
}
