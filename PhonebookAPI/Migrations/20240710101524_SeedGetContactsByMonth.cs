using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhonebookAPI.Migrations
{
    public partial class SeedGetContactsByMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetContactsByMonth
                                    @Month int
                                    AS
                                    BEGIN
	                                    SELECT c.*,co.CountryName,s.StateName FROM Contacts c
	                                    JOIN
		                                    Countries co ON co.CountryId = c.CountryId
	                                    JOIN
		                                    States s ON s.StateId = c.StateId
	                                    WHERE MONTH(c.BirthDate) = @Month;
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetContactsByMonth");
        }
    }
}
