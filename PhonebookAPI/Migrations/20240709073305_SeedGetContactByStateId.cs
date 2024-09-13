using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhonebookAPI.Migrations
{
    public partial class SeedGetContactByStateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetContactByStateId
	                                    @StateId INT
                                    AS
                                    BEGIN
	                                    SELECT s.StateId , c.contactId,c.FirstName,c.LastName,c.Email,c.Company,c.ContactNumber,c.FileName,c.CountryId,c.Gender,c.IsFavourite,c.Image
	                                    FROM States s
	                                    JOIN 
		                                    Contacts c ON c.StateId = s.StateId
	                                    WHERE 
		                                    s.StateId = @StateId
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetContactByStateId");
        }
    }
}
