using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RL.Data.Migrations
{
    public partial class AddAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcedureId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlanProcedurePlanId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlanProcedureProcedureId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_PlanProcedures_PlanProcedurePlanId_PlanProcedureProcedureId",
                        columns: x => new { x.PlanProcedurePlanId, x.PlanProcedureProcedureId },
                        principalTable: "PlanProcedures",
                        principalColumns: new[] { "PlanId", "ProcedureId" });
                    table.ForeignKey(
                        name: "FK_Assignments_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Procedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedures",
                        principalColumn: "ProcedureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_PlanId",
                table: "Assignments",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_PlanProcedurePlanId_PlanProcedureProcedureId",
                table: "Assignments",
                columns: new[] { "PlanProcedurePlanId", "PlanProcedureProcedureId" });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ProcedureId",
                table: "Assignments",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");
        }
    }
}
