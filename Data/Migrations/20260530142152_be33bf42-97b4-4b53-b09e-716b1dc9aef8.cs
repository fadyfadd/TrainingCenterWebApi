using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class be33bf4297b44b53b09e716b1dc9aef8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_administrators_AdministratorId",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_AdministratorId",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "AdministratorId",
                table: "courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdministratorId",
                table: "courses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_AdministratorId",
                table: "courses",
                column: "AdministratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_administrators_AdministratorId",
                table: "courses",
                column: "AdministratorId",
                principalTable: "administrators",
                principalColumn: "id");
        }
    }
}
