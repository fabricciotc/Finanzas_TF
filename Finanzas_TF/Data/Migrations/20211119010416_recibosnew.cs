using Microsoft.EntityFrameworkCore.Migrations;

namespace Finanzas_TF.Data.Migrations
{
    public partial class recibosnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MontoInicial",
                table: "ReciboHonorarios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Retenido",
                table: "ReciboHonorarios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontoInicial",
                table: "ReciboHonorarios");

            migrationBuilder.DropColumn(
                name: "Retenido",
                table: "ReciboHonorarios");
        }
    }
}
