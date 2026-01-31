using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumium.Infrastructure.Persistence.Migrations.Application;

/// <inheritdoc />
public partial class AddUsers : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "users",
            table => new
            {
                id = table.Column<Guid>("uuid", nullable: false),
                tenant_id = table.Column<string>("text", nullable: false),
                email = table.Column<string>("text", nullable: false),
                password_hash = table.Column<string>("text", nullable: false),
                first_name = table.Column<string>("text", nullable: false),
                last_name = table.Column<string>("text", nullable: false),
                is_active = table.Column<bool>("boolean", nullable: false),
                created_at = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_users", x => x.id); });

        migrationBuilder.CreateIndex(
            "IX_users_email",
            "users",
            "email");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "users");
    }
}