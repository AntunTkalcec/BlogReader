using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogReaderInfrastructureLibrary.Migrations
{
    public partial class BlogReaderMig6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.Sql(@"CREATE OR ALTER FUNCTION dbo.SearchContentByTerm(@term nvarchar(100)) RETURNS TABLE AS RETURN 
            (SELECT * FROM dbo.Articles a WHERE freetext(a.ContentText, @term));");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql(@"DROP FUNCTION dbo.SearchContentByTerm;");
        }
    }
}
