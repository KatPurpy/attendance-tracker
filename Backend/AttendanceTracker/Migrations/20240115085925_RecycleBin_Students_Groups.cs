using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class RecycleBin_Students_Groups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecycleBinGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    ExpiresBy = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_RecycleBinGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecycleBinStudents",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    ExpiresBy = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_RecycleBinStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecycleBinGroups_GroupId",
                table: "RecycleBinGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RecycleBinStudents_StudentId",
                table: "RecycleBinStudents",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecycleBinGroups");

            migrationBuilder.DropTable(
                name: "RecycleBinStudents");
        }
    }
}
