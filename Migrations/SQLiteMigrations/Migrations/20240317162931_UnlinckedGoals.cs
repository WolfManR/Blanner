using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLiteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class UnlinckedGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveGoals_AspNetUsers_UserId",
                table: "ActiveGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveGoals_Goals_Id",
                table: "ActiveGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_ActiveGoals_ActiveGoalId",
                table: "ToDos");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_AspNetUsers_UserId",
                table: "ToDos");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Goals_GoalId",
                table: "ToDos");

            migrationBuilder.AddColumn<bool>(
                name: "CreateGoalOnAssemblingJob",
                table: "ActiveGoals",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoals_AspNetUsers_UserId",
                table: "ActiveGoals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoals_Goals_Id",
                table: "ActiveGoals",
                column: "Id",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_ActiveGoals_ActiveGoalId",
                table: "ToDos",
                column: "ActiveGoalId",
                principalTable: "ActiveGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_AspNetUsers_UserId",
                table: "ToDos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Goals_GoalId",
                table: "ToDos",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveGoals_AspNetUsers_UserId",
                table: "ActiveGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveGoals_Goals_Id",
                table: "ActiveGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_ActiveGoals_ActiveGoalId",
                table: "ToDos");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_AspNetUsers_UserId",
                table: "ToDos");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Goals_GoalId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "CreateGoalOnAssemblingJob",
                table: "ActiveGoals");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoals_AspNetUsers_UserId",
                table: "ActiveGoals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoals_Goals_Id",
                table: "ActiveGoals",
                column: "Id",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_ActiveGoals_ActiveGoalId",
                table: "ToDos",
                column: "ActiveGoalId",
                principalTable: "ActiveGoals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_AspNetUsers_UserId",
                table: "ToDos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Goals_GoalId",
                table: "ToDos",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id");
        }
    }
}
