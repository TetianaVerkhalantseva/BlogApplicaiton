using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApplicaiton.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPublicToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Blogs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Blogs");
        }
    }
}
