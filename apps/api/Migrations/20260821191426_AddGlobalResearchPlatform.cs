using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Virenza.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGlobalResearchPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KnowledgeSources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BaseUrl = table.Column<string>(type: "text", nullable: false),
                    ApiUrl = table.Column<string>(type: "text", nullable: true),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    License = table.Column<string>(type: "text", nullable: true),
                    LicenseUrl = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSuccessfulSyncAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchSources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchTopics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchDatasets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Publisher = table.Column<string>(type: "text", nullable: true),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    DatasetUrl = table.Column<string>(type: "text", nullable: true),
                    APIUrl = table.Column<string>(type: "text", nullable: true),
                    License = table.Column<string>(type: "text", nullable: true),
                    LicenseUrl = table.Column<string>(type: "text", nullable: true),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetrievedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchDatasets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchDatasets_KnowledgeSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "KnowledgeSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchPublications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Abstract = table.Column<string>(type: "text", nullable: true),
                    PublicationType = table.Column<string>(type: "text", nullable: true),
                    Publisher = table.Column<string>(type: "text", nullable: true),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    DOI = table.Column<string>(type: "text", nullable: true),
                    OriginalUrl = table.Column<string>(type: "text", nullable: true),
                    License = table.Column<string>(type: "text", nullable: true),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetrievedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchPublications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchPublications_KnowledgeSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "KnowledgeSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResearchSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ResourceType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Url = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Publisher = table.Column<string>(type: "text", nullable: true),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchResources_ResearchSources_ResearchSourceId",
                        column: x => x.ResearchSourceId,
                        principalTable: "ResearchSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceBookmarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResearchResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceBookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceBookmarks_ResearchResources_ResearchResourceId",
                        column: x => x.ResearchResourceId,
                        principalTable: "ResearchResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResearchResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceLikes_ResearchResources_ResearchResourceId",
                        column: x => x.ResearchResourceId,
                        principalTable: "ResearchResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResearchDatasets_SourceId",
                table: "ResearchDatasets",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchPublications_SourceId",
                table: "ResearchPublications",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchResources_CountryCode",
                table: "ResearchResources",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchResources_Language",
                table: "ResearchResources",
                column: "Language");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchResources_ResearchSourceId",
                table: "ResearchResources",
                column: "ResearchSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchResources_Subject",
                table: "ResearchResources",
                column: "Subject");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceBookmarks_ResearchResourceId_UserId",
                table: "ResourceBookmarks",
                columns: new[] { "ResearchResourceId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLikes_ResearchResourceId_UserId",
                table: "ResourceLikes",
                columns: new[] { "ResearchResourceId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchDatasets");

            migrationBuilder.DropTable(
                name: "ResearchPublications");

            migrationBuilder.DropTable(
                name: "ResearchTopics");

            migrationBuilder.DropTable(
                name: "ResourceBookmarks");

            migrationBuilder.DropTable(
                name: "ResourceLikes");

            migrationBuilder.DropTable(
                name: "KnowledgeSources");

            migrationBuilder.DropTable(
                name: "ResearchResources");

            migrationBuilder.DropTable(
                name: "ResearchSources");
        }
    }
}
