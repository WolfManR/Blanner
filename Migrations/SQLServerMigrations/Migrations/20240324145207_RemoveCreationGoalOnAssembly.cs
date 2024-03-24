using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCreationGoalOnAssembly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateGoalOnAssemblingJob",
                table: "ActiveGoals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CreateGoalOnAssemblingJob",
                table: "ActiveGoals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
