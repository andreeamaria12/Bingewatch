using Microsoft.EntityFrameworkCore.Migrations;

namespace Personalized_movie_recommendation_system.Migrations
{
    public partial class FixedClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Match",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "Movies",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ReleaseYear",
                table: "Movies",
                newName: "VoteCount");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Movies",
                newName: "ReleaseDate");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "varchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoteCount",
                table: "Movies",
                newName: "ReleaseYear");

            migrationBuilder.RenameColumn(
                name: "ReleaseDate",
                table: "Movies",
                newName: "Duration");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Movies",
                newName: "VideoUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Match",
                table: "Movies",
                type: "varchar(20)",
                nullable: true);
        }
    }
}
