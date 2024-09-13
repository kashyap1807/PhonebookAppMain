using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhonebookAPI.Migrations
{
    public partial class SeedGetData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetAllContacts
                                   AS
                                   BEGIN
                                    SELECT * FROM Contacts 
                                   END
                                   GO
                                    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllContacts");
        }
    }
}
