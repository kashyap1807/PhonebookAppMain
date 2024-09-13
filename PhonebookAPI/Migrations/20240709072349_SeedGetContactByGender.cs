using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhonebookAPI.Migrations
{
    public partial class SeedGetContactByGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetAllContactByGender
                                    AS
                                    BEGIN
	                                    SELECT c.Gender,COUNT(c.contactId) AS TotalContact  
	                                    FROM Contacts c
	                                    GROUP BY 
		                                    c.Gender
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetAllContactByGender");
        }
    }
}
