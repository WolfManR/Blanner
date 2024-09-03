using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class MultipleGoalsContractors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractorGoalTemplate",
                columns: table => new
                {
                    ContractorsId = table.Column<int>(type: "int", nullable: false),
                    GoalTemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorGoalTemplate", x => new { x.ContractorsId, x.GoalTemplateId });
                    table.ForeignKey(
                        name: "FK_ContractorGoalTemplate_Contractors_ContractorsId",
                        column: x => x.ContractorsId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractorGoalTemplate_GoalsTemplates_GoalTemplateId",
                        column: x => x.GoalTemplateId,
                        principalTable: "GoalsTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractorGoalTemplate_GoalTemplateId",
                table: "ContractorGoalTemplate",
                column: "GoalTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractorGoalTemplate");
        }
    }
}
