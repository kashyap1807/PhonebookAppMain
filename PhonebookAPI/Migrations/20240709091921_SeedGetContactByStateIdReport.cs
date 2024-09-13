using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhonebookAPI.Migrations
{
    public partial class SeedGetContactByStateIdReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetContactByStateIdReport
                                   @StateId INT
                                  AS
                                    BEGIN
                                    SELECT c.*,co.CountryName,s.StateName FROM Contacts c
                                    JOIN States s ON s.StateId = c.StateId
                                    JOIN Countries co ON co.CountryId =c.CountryId
                                    WHERE c.StateId = @StateId
                                    END
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"");
        }
    }
}
