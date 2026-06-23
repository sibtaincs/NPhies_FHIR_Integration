using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPhies_FHIR_Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunicationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunicationRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CommunicationRequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CategorySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectPatientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AboutResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AboutIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AboutIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayloadContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    FhirCommunicationRequestJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationRequests_Organizations_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationRequests_Organizations_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationRequests_Patients_SubjectPatientId",
                        column: x => x.SubjectPatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Communications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CommunicationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BasedOnResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BasedOnIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BasedOnIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CategorySystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectPatientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AboutResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AboutIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AboutIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayloadContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    PayloadAttachmentContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayloadAttachmentData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PayloadAttachmentTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PayloadAttachmentCreation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FhirCommunicationJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communications_Organizations_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Communications_Organizations_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Communications_Patients_SubjectPatientId",
                        column: x => x.SubjectPatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_CommunicationRequestId",
                table: "CommunicationRequests",
                column: "CommunicationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_RecipientId",
                table: "CommunicationRequests",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_SenderId",
                table: "CommunicationRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_Status",
                table: "CommunicationRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_SubjectPatientId",
                table: "CommunicationRequests",
                column: "SubjectPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_CommunicationId",
                table: "Communications",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_RecipientId",
                table: "Communications",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_SenderId",
                table: "Communications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_Status",
                table: "Communications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_SubjectPatientId",
                table: "Communications",
                column: "SubjectPatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunicationRequests");

            migrationBuilder.DropTable(
                name: "Communications");
        }
    }
}
