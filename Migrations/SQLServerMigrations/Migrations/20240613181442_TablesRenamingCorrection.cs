using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class TablesRenamingCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Active_AspNetUsers_UserId",
                table: "Active");

            migrationBuilder.DropForeignKey(
                name: "FK_Active_Contractors_ContractorId",
                table: "Active");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveGoalsTime_Active_ActiveGoalId",
                table: "ActiveGoalsTime");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_AspNetUsers_UserId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Contractors_ContractorId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Active_GoalId",
                table: "ToDos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Templates",
                table: "Templates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Active",
                table: "Active");

            migrationBuilder.RenameTable(
                name: "Templates",
                newName: "GoalsTemplates");

            migrationBuilder.RenameTable(
                name: "Active",
                newName: "Goals");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_UserId",
                table: "GoalsTemplates",
                newName: "IX_GoalsTemplates_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_ContractorId",
                table: "GoalsTemplates",
                newName: "IX_GoalsTemplates_ContractorId");

            migrationBuilder.RenameIndex(
                name: "IX_Active_UserId",
                table: "Goals",
                newName: "IX_Goals_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Active_ContractorId",
                table: "Goals",
                newName: "IX_Goals_ContractorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoalsTemplates",
                table: "GoalsTemplates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goals",
                table: "Goals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoalsTime_Goals_ActiveGoalId",
                table: "ActiveGoalsTime",
                column: "ActiveGoalId",
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
                name: "FK_Goals_Contractors_ContractorId",
                table: "Goals",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalsTemplates_AspNetUsers_UserId",
                table: "GoalsTemplates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalsTemplates_Contractors_ContractorId",
                table: "GoalsTemplates",
                column: "ContractorId",
                principalTable: "Contractors",
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
                name: "FK_ActiveGoalsTime_Goals_ActiveGoalId",
                table: "ActiveGoalsTime");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Contractors_ContractorId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalsTemplates_AspNetUsers_UserId",
                table: "GoalsTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalsTemplates_Contractors_ContractorId",
                table: "GoalsTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Goals_GoalId",
                table: "ToDos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GoalsTemplates",
                table: "GoalsTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goals",
                table: "Goals");

            migrationBuilder.RenameTable(
                name: "GoalsTemplates",
                newName: "Templates");

            migrationBuilder.RenameTable(
                name: "Goals",
                newName: "Active");

            migrationBuilder.RenameIndex(
                name: "IX_GoalsTemplates_UserId",
                table: "Templates",
                newName: "IX_Templates_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GoalsTemplates_ContractorId",
                table: "Templates",
                newName: "IX_Templates_ContractorId");

            migrationBuilder.RenameIndex(
                name: "IX_Goals_UserId",
                table: "Active",
                newName: "IX_Active_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Goals_ContractorId",
                table: "Active",
                newName: "IX_Active_ContractorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Templates",
                table: "Templates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Active",
                table: "Active",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Active_AspNetUsers_UserId",
                table: "Active",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Active_Contractors_ContractorId",
                table: "Active",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoalsTime_Active_ActiveGoalId",
                table: "ActiveGoalsTime",
                column: "ActiveGoalId",
                principalTable: "Active",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_AspNetUsers_UserId",
                table: "Templates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Contractors_ContractorId",
                table: "Templates",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Active_GoalId",
                table: "ToDos",
                column: "GoalId",
                principalTable: "Active",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
