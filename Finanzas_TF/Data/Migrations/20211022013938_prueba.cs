using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Finanzas_TF.Data.Migrations
{
    public partial class prueba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReciboHonorarios_Clientes_ClienteId",
                table: "ReciboHonorarios");

            migrationBuilder.DropIndex(
                name: "IX_ReciboHonorarios_ClienteId",
                table: "ReciboHonorarios");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "ReciboHonorarios");

            migrationBuilder.RenameColumn(
                name: "FechaSubida",
                table: "ReciboHonorarios",
                newName: "FechaPago");

            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "Clientes",
                newName: "RazonSocial");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "ReciboHonorarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEmision",
                table: "ReciboHonorarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "IdCliente",
                table: "ReciboHonorarios",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReciboHonorarios_IdCliente",
                table: "ReciboHonorarios",
                column: "IdCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_ReciboHonorarios_Clientes_IdCliente",
                table: "ReciboHonorarios",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReciboHonorarios_Clientes_IdCliente",
                table: "ReciboHonorarios");

            migrationBuilder.DropIndex(
                name: "IX_ReciboHonorarios_IdCliente",
                table: "ReciboHonorarios");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "ReciboHonorarios");

            migrationBuilder.DropColumn(
                name: "FechaEmision",
                table: "ReciboHonorarios");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "ReciboHonorarios");

            migrationBuilder.RenameColumn(
                name: "FechaPago",
                table: "ReciboHonorarios",
                newName: "FechaSubida");

            migrationBuilder.RenameColumn(
                name: "RazonSocial",
                table: "Clientes",
                newName: "Fullname");

            migrationBuilder.AddColumn<Guid>(
                name: "ClienteId",
                table: "ReciboHonorarios",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReciboHonorarios_ClienteId",
                table: "ReciboHonorarios",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReciboHonorarios_Clientes_ClienteId",
                table: "ReciboHonorarios",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
