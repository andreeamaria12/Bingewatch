using Microsoft.EntityFrameworkCore.Migrations;

namespace Personalized_movie_recommendation_system.Data.Migrations
{
    public partial class FavMovies2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteMovie_Movies_MovieId",
                table: "FavoriteMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteMovie_AspNetUsers_UserId",
                table: "FavoriteMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteMovie",
                table: "FavoriteMovie");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "Movies");

            migrationBuilder.RenameTable(
                name: "FavoriteMovie",
                newName: "FavoriteMovies");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteMovie_UserId",
                table: "FavoriteMovies",
                newName: "IX_FavoriteMovies_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteMovies",
                table: "FavoriteMovies",
                columns: new[] { "MovieId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteMovies_Movies_MovieId",
                table: "FavoriteMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteMovies_AspNetUsers_UserId",
                table: "FavoriteMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteMovies_Movies_MovieId",
                table: "FavoriteMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteMovies_AspNetUsers_UserId",
                table: "FavoriteMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteMovies",
                table: "FavoriteMovies");

            migrationBuilder.RenameTable(
                name: "FavoriteMovies",
                newName: "FavoriteMovie");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteMovies_UserId",
                table: "FavoriteMovie",
                newName: "IX_FavoriteMovie_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "Video",
                table: "Movies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteMovie",
                table: "FavoriteMovie",
                columns: new[] { "MovieId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteMovie_Movies_MovieId",
                table: "FavoriteMovie",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteMovie_AspNetUsers_UserId",
                table: "FavoriteMovie",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
