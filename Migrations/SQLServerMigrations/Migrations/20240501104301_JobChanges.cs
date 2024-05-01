using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class JobChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobChangeId",
                table: "JobsTime",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Saved = table.Column<bool>(type: "bit", nullable: false),
                    ElapsedTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Start = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    End = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ContextId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobChanges_Jobs_ContextId",
                        column: x => x.ContextId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobsTime_JobChangeId",
                table: "JobsTime",
                column: "JobChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobChanges_ContextId",
                table: "JobChanges",
                column: "ContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobsTime_JobChanges_JobChangeId",
                table: "JobsTime",
                column: "JobChangeId",
                principalTable: "JobChanges",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobsTime_JobChanges_JobChangeId",
                table: "JobsTime");

            migrationBuilder.DropTable(
                name: "JobChanges");

            migrationBuilder.DropIndex(
                name: "IX_JobsTime_JobChangeId",
                table: "JobsTime");

            migrationBuilder.DropColumn(
                name: "JobChangeId",
                table: "JobsTime");
        }
    }
}
