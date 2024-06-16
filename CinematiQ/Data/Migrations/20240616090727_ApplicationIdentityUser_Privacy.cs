using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinematiQ.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationIdentityUser_Privacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PublicLastWatchedMovies",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PublicMovieMarkers",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PublicStatus",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicLastWatchedMovies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicMovieMarkers",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicStatus",
                table: "AspNetUsers");
        }
    }
}
