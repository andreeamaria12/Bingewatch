using Microsoft.EntityFrameworkCore.Migrations;

namespace Personalized_movie_recommendation_system.Migrations
{
    public partial class FixedClasses2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cast",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Movies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cast",
                table: "Movies",
                type: "varchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Movies",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");
        }
    }
}
