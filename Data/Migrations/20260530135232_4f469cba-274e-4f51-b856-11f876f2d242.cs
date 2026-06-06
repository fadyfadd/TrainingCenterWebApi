using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _4f469cba274e4f51b85611f876f2d242 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "administarator_sequence");

            migrationBuilder.AddColumn<int>(
                name: "AdministratorId",
                table: "courses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "administrators",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"administarator_sequence\"')"),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    enrolled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrators", x => x.id);
                    table.ForeignKey(
                        name: "FK_administrators_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_courses_AdministratorId",
                table: "courses",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_administrators_user_id",
                table: "administrators",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_courses_administrators_AdministratorId",
                table: "courses",
                column: "AdministratorId",
                principalTable: "administrators",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_administrators_AdministratorId",
                table: "courses");

            migrationBuilder.DropTable(
                name: "administrators");

            migrationBuilder.DropIndex(
                name: "IX_courses_AdministratorId",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "AdministratorId",
                table: "courses");

            migrationBuilder.DropSequence(
                name: "administarator_sequence");
        }
    }
}
