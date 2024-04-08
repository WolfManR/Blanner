using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class JobTimeConstraintCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobsTime_Jobs_JobContextId",
                table: "JobsTime");

            migrationBuilder.DropIndex(
                name: "IX_JobsTime_JobContextId",
                table: "JobsTime");

            migrationBuilder.DropColumn(
                name: "JobContextId",
                table: "JobsTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobContextId",
                table: "JobsTime",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobsTime_JobContextId",
                table: "JobsTime",
                column: "JobContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobsTime_Jobs_JobContextId",
                table: "JobsTime",
                column: "JobContextId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}
