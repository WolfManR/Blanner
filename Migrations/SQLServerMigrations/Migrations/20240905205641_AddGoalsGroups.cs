using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalsGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoalGroupId",
                table: "Goals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GoalsGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalsGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goals_GoalGroupId",
                table: "Goals",
                column: "GoalGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_GoalsGroups_GoalGroupId",
                table: "Goals",
                column: "GoalGroupId",
                principalTable: "GoalsGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_GoalsGroups_GoalGroupId",
                table: "Goals");

            migrationBuilder.DropTable(
                name: "GoalsGroups");

            migrationBuilder.DropIndex(
                name: "IX_Goals_GoalGroupId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "GoalGroupId",
                table: "Goals");
        }
    }
}
