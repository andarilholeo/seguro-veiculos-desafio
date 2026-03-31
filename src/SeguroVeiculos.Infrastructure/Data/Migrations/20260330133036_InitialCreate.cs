using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeguroVeiculos.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Segurados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Idade = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segurados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MarcaModelo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seguros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VeiculoId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeguradoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Calculo_ValorVeiculo = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Calculo_TaxaRisco = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Calculo_PremioRisco = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Calculo_PremioPuro = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Calculo_PremioComercial = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seguros_Segurados_SeguradoId",
                        column: x => x.SeguradoId,
                        principalTable: "Segurados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seguros_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguros_SeguradoId",
                table: "Seguros",
                column: "SeguradoId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguros_VeiculoId",
                table: "Seguros",
                column: "VeiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seguros");

            migrationBuilder.DropTable(
                name: "Segurados");

            migrationBuilder.DropTable(
                name: "Veiculos");
        }
    }
}
