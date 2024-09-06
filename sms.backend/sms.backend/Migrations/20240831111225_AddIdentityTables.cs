using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sms.backend.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if the AspNetRoles table exists before creating it
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetRoles' AND xtype='U')
                BEGIN
                    CREATE TABLE [AspNetRoles] (
                        [Id] nvarchar(450) NOT NULL,
                        [Name] nvarchar(256) NULL,
                        [NormalizedName] nvarchar(256) NULL,
                        [ConcurrencyStamp] nvarchar(max) NULL,
                        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
                    );
                END
            ");

            // Check if the AspNetUsers table exists before creating it
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetUsers' AND xtype='U')
                BEGIN
                    CREATE TABLE [AspNetUsers] (
                        [Id] nvarchar(450) NOT NULL,
                        [Role] nvarchar(max) NOT NULL,
                        [UserName] nvarchar(256) NULL,
                        [NormalizedUserName] nvarchar(256) NULL,
                        [Email] nvarchar(256) NULL,
                        [NormalizedEmail] nvarchar(256) NULL,
                        [EmailConfirmed] bit NOT NULL,
                        [PasswordHash] nvarchar(max) NULL,
                        [SecurityStamp] nvarchar(max) NULL,
                        [ConcurrencyStamp] nvarchar(max) NULL,
                        [PhoneNumber] nvarchar(max) NULL,
                        [PhoneNumberConfirmed] bit NOT NULL,
                        [TwoFactorEnabled] bit NOT NULL,
                        [LockoutEnd] datetimeoffset NULL,
                        [LockoutEnabled] bit NOT NULL,
                        [AccessFailedCount] int NOT NULL,
                        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
                    );
                END
            ");

            // Check if the AspNetUserClaims table exists before creating it
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetUserClaims' AND xtype='U')
                BEGIN
                    CREATE TABLE [AspNetUserClaims] (
                        [Id] int NOT NULL IDENTITY,
                        [UserId] nvarchar(450) NOT NULL,
                        [ClaimType] nvarchar(max) NULL,
                        [ClaimValue] nvarchar(max) NULL,
                        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            // Check if the AspNetUserRoles table exists before creating it
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetUserRoles' AND xtype='U')
                BEGIN
                    CREATE TABLE [AspNetUserRoles] (
                        [UserId] nvarchar(450) NOT NULL,
                        [RoleId] nvarchar(450) NOT NULL,
                        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
                        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            // Add similar checks for other tables as needed
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}