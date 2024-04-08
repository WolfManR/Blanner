using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkComment",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "Marked",
                table: "Jobs",
                newName: "Saved");

            migrationBuilder.AddColumn<int>(
                name: "JobContextId",
                table: "JobsTime",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Jobs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 4, 7));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ElapsedTime",
                table: "Jobs",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "End",
                table: "Jobs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Start",
                table: "Jobs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobsTime_JobContextId",
                table: "JobsTime",
                column: "JobContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId",
                table: "Jobs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_JobsTime_Jobs_JobContextId",
                table: "JobsTime",
                column: "JobContextId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_JobsTime_Jobs_JobContextId",
                table: "JobsTime");

            migrationBuilder.DropIndex(
                name: "IX_JobsTime_JobContextId",
                table: "JobsTime");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobContextId",
                table: "JobsTime");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ElapsedTime",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "End",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "Saved",
                table: "Jobs",
                newName: "Marked");

            migrationBuilder.AddColumn<string>(
                name: "MarkComment",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
