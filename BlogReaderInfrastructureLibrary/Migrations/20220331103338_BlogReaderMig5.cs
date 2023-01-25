using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogReaderInfrastructureLibrary.Migrations
{
    public partial class BlogReaderMig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ArticleID",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleID",
                table: "Articles",
                column: "ArticleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_ArticleID",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "ArticleID",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
            migrationBuilder.Sql(@"DROP FUNCTION dbo.SearchContentByTerm;");
        }
    }
}
