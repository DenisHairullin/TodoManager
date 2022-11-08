using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoManager.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "Date", nullable: false),
                    ParentTaskId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Tasks_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoTagTodoTask",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTagTodoTask", x => new { x.TagsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_TodoTagTodoTask_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoTagTodoTask_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Tag1" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Tag2" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Description", "Level", "Name", "ParentTaskId" },
                values: new object[] { 1, new DateTime(2022, 11, 8, 17, 39, 48, 490, DateTimeKind.Local).AddTicks(8553), "d1", 0, "n1", null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Description", "Level", "Name", "ParentTaskId" },
                values: new object[] { 2, new DateTime(2022, 11, 8, 17, 39, 48, 490, DateTimeKind.Local).AddTicks(8569), "d2", 1, "n2", 1 });

            migrationBuilder.InsertData(
                table: "TodoTagTodoTask",
                columns: new[] { "TagsId", "TasksId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "TodoTagTodoTask",
                columns: new[] { "TagsId", "TasksId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Description", "Level", "Name", "ParentTaskId" },
                values: new object[] { 3, new DateTime(2022, 11, 8, 17, 39, 48, 490, DateTimeKind.Local).AddTicks(8571), "d3", 2, "n3", 2 });

            migrationBuilder.InsertData(
                table: "TodoTagTodoTask",
                columns: new[] { "TagsId", "TasksId" },
                values: new object[] { 2, 2 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Description", "Level", "Name", "ParentTaskId" },
                values: new object[] { 4, new DateTime(2022, 11, 8, 17, 39, 48, 490, DateTimeKind.Local).AddTicks(8572), "d4", 3, "n4", 3 });

            migrationBuilder.InsertData(
                table: "TodoTagTodoTask",
                columns: new[] { "TagsId", "TasksId" },
                values: new object[] { 1, 3 });

            migrationBuilder.InsertData(
                table: "TodoTagTodoTask",
                columns: new[] { "TagsId", "TasksId" },
                values: new object[] { 2, 4 });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ParentTaskId",
                table: "Tasks",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTagTodoTask_TasksId",
                table: "TodoTagTodoTask",
                column: "TasksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoTagTodoTask");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
