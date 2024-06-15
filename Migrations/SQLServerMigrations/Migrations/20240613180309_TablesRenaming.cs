using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class TablesRenaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveGoals_AspNetUsers_UserId",
                table: "ActiveGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveGoals_Contractors_ContractorId",
                table: "ActiveGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveGoalsTime_ActiveGoals_ActiveGoalId",
                table: "ActiveGoalsTime");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Contractors_ContractorId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_ActiveGoals_GoalId",
                table: "ToDos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goals",
                table: "Goals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveGoals",
                table: "ActiveGoals");

            migrationBuilder.RenameTable(
                name: "Goals",
                newName: "Templates");

            migrationBuilder.RenameTable(
                name: "ActiveGoals",
                newName: "Active");

            migrationBuilder.RenameIndex(
                name: "IX_Goals_UserId",
                table: "Templates",
                newName: "IX_Templates_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Goals_ContractorId",
                table: "Templates",
                newName: "IX_Templates_ContractorId");

            migrationBuilder.RenameIndex(
                name: "IX_ActiveGoals_UserId",
                table: "Active",
                newName: "IX_Active_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ActiveGoals_ContractorId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "Goals");

            migrationBuilder.RenameTable(
                name: "Active",
                newName: "ActiveGoals");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_UserId",
                table: "Goals",
                newName: "IX_Goals_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_ContractorId",
                table: "Goals",
                newName: "IX_Goals_ContractorId");

            migrationBuilder.RenameIndex(
                name: "IX_Active_UserId",
                table: "ActiveGoals",
                newName: "IX_ActiveGoals_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Active_ContractorId",
                table: "ActiveGoals",
                newName: "IX_ActiveGoals_ContractorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goals",
                table: "Goals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveGoals",
                table: "ActiveGoals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoals_AspNetUsers_UserId",
                table: "ActiveGoals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoals_Contractors_ContractorId",
                table: "ActiveGoals",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveGoalsTime_ActiveGoals_ActiveGoalId",
                table: "ActiveGoalsTime",
                column: "ActiveGoalId",
                principalTable: "ActiveGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Contractors_ContractorId",
                table: "Goals",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_ActiveGoals_GoalId",
                table: "ToDos",
                column: "GoalId",
                principalTable: "ActiveGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
