using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinematiQ.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_TotalSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalEpisodes",
                table: "Seasons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalEpisodes",
                table: "Seasons");
        }
    }
}
