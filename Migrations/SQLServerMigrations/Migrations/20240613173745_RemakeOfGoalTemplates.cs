using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class RemakeOfGoalTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_ActiveGoals_ActiveGoalId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_ActiveGoals_ActiveGoalId",
                table: "ToDos");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Goals_GoalId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_ActiveGoalId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_Goals_ActiveGoalId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "ActiveGoalId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "ActiveGoalId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "GoalId",
                table: "ActiveGoals");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Goals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Goals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ActiveGoals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_ActiveGoals_GoalId",
                table: "ToDos",
                column: "GoalId",
                principalTable: "ActiveGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_ActiveGoals_GoalId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Goals");

            migrationBuilder.AddColumn<int>(
                name: "ActiveGoalId",
                table: "ToDos",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Goals",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ActiveGoalId",
                table: "Goals",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ActiveGoals",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "GoalId",
                table: "ActiveGoals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_ActiveGoalId",
                table: "ToDos",
                column: "ActiveGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_ActiveGoalId",
                table: "Goals",
                column: "ActiveGoalId",
                unique: true,
                filter: "[ActiveGoalId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_ActiveGoals_ActiveGoalId",
                table: "Goals",
                column: "ActiveGoalId",
                principalTable: "ActiveGoals",
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
                name: "FK_ToDos_Goals_GoalId",
                table: "ToDos",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
