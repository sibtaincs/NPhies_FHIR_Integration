using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPhies_FHIR_Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPollingRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PollingRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PollingRecordId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RequestTaskId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaskRequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestedMessageTypes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseTaskId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaskResponseId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResponseStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResponseReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessagesReceived = table.Column<int>(type: "int", nullable: false),
                    ReceivedMessageTypes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestBundleJson = table.Column<string>(type: "ntext", nullable: true),
                    ResponseBundleJson = table.Column<string>(type: "ntext", nullable: true),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ErrorCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DurationMs = table.Column<long>(type: "bigint", nullable: true),
                    CycleStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    MaxRetries = table.Column<int>(type: "int", nullable: true),
                    NextRetryAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SourceIpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequestSourceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollingRecords_Organizations_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PollingRecords_TaskRequests_TaskRequestId",
                        column: x => x.TaskRequestId,
                        principalTable: "TaskRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PollingRecords_TaskResponses_TaskResponseId",
                        column: x => x.TaskResponseId,
                        principalTable: "TaskResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_CycleStatus",
                table: "PollingRecords",
                column: "CycleStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_IsAcknowledged",
                table: "PollingRecords",
                column: "IsAcknowledged");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_PollingRecordId",
                table: "PollingRecords",
                column: "PollingRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_ProcessingStatus",
                table: "PollingRecords",
                column: "ProcessingStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_ProviderId",
                table: "PollingRecords",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_RequestSentAt",
                table: "PollingRecords",
                column: "RequestSentAt");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_ResponseReceivedAt",
                table: "PollingRecords",
                column: "ResponseReceivedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_ResponseStatus",
                table: "PollingRecords",
                column: "ResponseStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_TaskRequestId",
                table: "PollingRecords",
                column: "TaskRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_TaskResponseId",
                table: "PollingRecords",
                column: "TaskResponseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PollingRecords");
        }
    }
}
