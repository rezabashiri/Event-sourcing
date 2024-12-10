using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.EmployeeManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "StaffManagement");

            migrationBuilder.CreateTable(
                name: "StaffMembers",
                schema: "StaffManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NationalCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Family = table.Column<string>(type: "text", nullable: false),
                    EmployeedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffTask",
                schema: "StaffManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false),
                    StaffMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Due = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffTask_StaffMembers_StaffMemberId",
                        column: x => x.StaffMemberId,
                        principalSchema: "StaffManagement",
                        principalTable: "StaffMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffTask_StaffMemberId",
                schema: "StaffManagement",
                table: "StaffTask",
                column: "StaffMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffTask",
                schema: "StaffManagement");

            migrationBuilder.DropTable(
                name: "StaffMembers",
                schema: "StaffManagement");
        }
    }
}
