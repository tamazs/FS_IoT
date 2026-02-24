using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    turbineId = table.Column<string>(type: "text", nullable: false),
                    farmId = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    severity = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    turbineId = table.Column<string>(type: "text", nullable: false),
                    turbineName = table.Column<string>(type: "text", nullable: false),
                    farmId = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    windSpeed = table.Column<double>(type: "double precision", nullable: false),
                    windDirection = table.Column<double>(type: "double precision", nullable: false),
                    ambientTemperature = table.Column<double>(type: "double precision", nullable: false),
                    rotorSpeed = table.Column<double>(type: "double precision", nullable: false),
                    powerOutput = table.Column<double>(type: "double precision", nullable: false),
                    nacelleDirection = table.Column<double>(type: "double precision", nullable: false),
                    bladePitch = table.Column<double>(type: "double precision", nullable: false),
                    generatorTemp = table.Column<double>(type: "double precision", nullable: false),
                    gearboxTemp = table.Column<double>(type: "double precision", nullable: false),
                    vibration = table.Column<double>(type: "double precision", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    RefreshTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TurbineActions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TurbineId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    IntervalValue = table.Column<int>(type: "integer", nullable: true),
                    StopReason = table.Column<string>(type: "text", nullable: true),
                    PitchAngle = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurbineActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurbineActions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TurbineActions_UserId",
                table: "TurbineActions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "TurbineActions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
