using Microsoft.EntityFrameworkCore.Migrations;

namespace Finanzas_TF.Data.Migrations
{
    public partial class mondea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Moneda",
                table: "ReciboHonorarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Moneda",
                table: "ReciboHonorarios");
        }
    }
}
