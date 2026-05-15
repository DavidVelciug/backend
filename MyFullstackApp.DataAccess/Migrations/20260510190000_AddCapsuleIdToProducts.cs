using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFullstackApp.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddCapsuleIdToProducts : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "CapsuleId",
            table: "Products",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CapsuleId",
            table: "Products");
    }
}

