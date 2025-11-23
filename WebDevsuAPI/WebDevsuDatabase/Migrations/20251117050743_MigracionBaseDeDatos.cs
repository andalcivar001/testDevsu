using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebDevsuDatabase.Migrations
{
    /// <inheritdoc />
    public partial class MigracionBaseDeDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    idCliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    genero = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    fechaNacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    numeroIdentificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fechaCreacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.idCliente);
                });

            migrationBuilder.CreateTable(
                name: "cuenta",
                columns: table => new
                {
                    idCuenta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numeroCuenta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tipoCuenta = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    idCliente = table.Column<int>(type: "integer", nullable: false),
                    saldoInicial = table.Column<decimal>(type: "numeric", nullable: true),
                    estado = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cuenta", x => x.idCuenta);
                });

            migrationBuilder.CreateTable(
                name: "movimiento",
                columns: table => new
                {
                    idMovimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    tipoMovimiento = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    valor = table.Column<decimal>(type: "numeric", nullable: false),
                    idCuenta = table.Column<int>(type: "integer", nullable: false),
                    saldo = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimiento", x => x.idMovimiento);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "cuenta");

            migrationBuilder.DropTable(
                name: "movimiento");
        }
    }
}
