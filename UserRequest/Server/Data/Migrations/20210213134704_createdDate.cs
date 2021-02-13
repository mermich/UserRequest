using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserRequest.Server.Data.Migrations
{
    public partial class createdDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicComments_Topics_TopicId",
                table: "TopicComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicVotes_Topics_TopicId",
                table: "TopicVotes");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "TopicVotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "TopicComments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "TopicComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_TopicComments_Topics_TopicId",
                table: "TopicComments",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicVotes_Topics_TopicId",
                table: "TopicVotes",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicComments_Topics_TopicId",
                table: "TopicComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicVotes_Topics_TopicId",
                table: "TopicVotes");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "TopicComments");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "TopicVotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "TopicComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicComments_Topics_TopicId",
                table: "TopicComments",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicVotes_Topics_TopicId",
                table: "TopicVotes",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
