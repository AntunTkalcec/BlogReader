using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogReaderInfrastructureLibrary.Migrations
{
    public partial class BlogReaderMig9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT", suppressTransaction: true);
            migrationBuilder.Sql("CREATE FULLTEXT INDEX ON dbo.Articles(ContentText) KEY INDEX PK_Articles WITH STOPLIST = SYSTEM;", suppressTransaction: true);
            migrationBuilder.Sql(@"CREATE OR ALTER FUNCTION dbo.SearchContentByTerm(@term nvarchar(100)) RETURNS TABLE AS RETURN 
            (SELECT * FROM dbo.Articles a WHERE freetext(a.ContentText, @term));", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION dbo.SearchContentByTerm;");
            migrationBuilder.Sql("DROP FULLTEXT CATALOG ON dbo.Articles");
            migrationBuilder.Sql("DROP FULLTEXT INDEX ON dbo.Articles;");
        }
    }
}
