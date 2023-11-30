using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class addguidsalternatekey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Students_Guid",
                table: "Students",
                column: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Students_Guid",
                table: "Students");
        }
    }
}
