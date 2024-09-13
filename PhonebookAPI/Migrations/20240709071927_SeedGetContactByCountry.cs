using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhonebookAPI.Migrations
{
    public partial class SeedGetContactByCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetAllContactByCountry
                                    AS
                                    BEGIN
	                                    SELECT c.CountryId,c.CountryName, COUNT(co.contactId) AS TotalContacts
	                                    FROM Countries c
	                                    JOIN
		                                    Contacts co ON co.CountryId = c.CountryId
	                                    GROUP BY 
		                                    c.CountryId,c.CountryName
                                    END
                                    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllContactByCountry");
        }
    }
}
