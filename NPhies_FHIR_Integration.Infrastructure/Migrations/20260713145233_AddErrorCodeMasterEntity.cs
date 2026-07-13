using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPhies_FHIR_Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorCodeMasterEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationRequests_Organizations_RecipientId",
                table: "CommunicationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationRequests_Organizations_SenderId",
                table: "CommunicationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationRequests_Patients_SubjectPatientId",
                table: "CommunicationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Communications_Organizations_RecipientId",
                table: "Communications");

            migrationBuilder.DropForeignKey(
                name: "FK_Communications_Organizations_SenderId",
                table: "Communications");

            migrationBuilder.DropForeignKey(
                name: "FK_Communications_Patients_SubjectPatientId",
                table: "Communications");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingRecords_TaskRequests_TaskRequestId",
                table: "PollingRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingRecords_TaskResponses_TaskResponseId",
                table: "PollingRecords");

            migrationBuilder.DropTable(
                name: "TaskResponses");

            migrationBuilder.DropTable(
                name: "TaskRequests");

            migrationBuilder.DropIndex(
                name: "IX_PollingRecords_IsAcknowledged",
                table: "PollingRecords");

            migrationBuilder.DropIndex(
                name: "IX_PollingRecords_PollingRecordId",
                table: "PollingRecords");

            migrationBuilder.DropIndex(
                name: "IX_PollingRecords_ResponseStatus",
                table: "PollingRecords");

            migrationBuilder.DropIndex(
                name: "IX_Communications_CommunicationId",
                table: "Communications");

            migrationBuilder.DropIndex(
                name: "IX_Communications_Status",
                table: "Communications");

            migrationBuilder.DropIndex(
                name: "IX_CommunicationRequests_CommunicationRequestId",
                table: "CommunicationRequests");

            migrationBuilder.DropIndex(
                name: "IX_CommunicationRequests_Status",
                table: "CommunicationRequests");

            migrationBuilder.RenameColumn(
                name: "TaskResponseId",
                table: "PollingRecords",
                newName: "CancellationResponseId");

            migrationBuilder.RenameColumn(
                name: "TaskRequestId",
                table: "PollingRecords",
                newName: "CancellationRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_PollingRecords_TaskResponseId",
                table: "PollingRecords",
                newName: "IX_PollingRecords_CancellationResponseId");

            migrationBuilder.RenameIndex(
                name: "IX_PollingRecords_TaskRequestId",
                table: "PollingRecords",
                newName: "IX_PollingRecords_CancellationRequestId");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderId",
                table: "PollingRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Communications",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "Communications",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PayloadAttachmentTitle",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PayloadAttachmentContentType",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentifierValue",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentifierSystem",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommunicationId",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CategorySystem",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BasedOnResourceType",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BasedOnIdentifierValue",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BasedOnIdentifierSystem",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutResourceType",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutIdentifierValue",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutIdentifierSystem",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "CommunicationRequests",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "CommunicationRequests",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentifierValue",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentifierSystem",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommunicationRequestId",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CategorySystem",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutResourceType",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutIdentifierValue",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutIdentifierSystem",
                table: "CommunicationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CancellationRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Intent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FocusResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FocusIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FocusIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReasonCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReasonCodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReasonText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AuthoredOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FhirTaskJson = table.Column<string>(type: "ntext", nullable: true),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancellationRequests_Organizations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CancellationRequests_Organizations_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ErrorCodeMasters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ErrorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ErrorDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ErrorCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsRecoverable = table.Column<bool>(type: "bit", nullable: false),
                    AllowsAppeal = table.Column<bool>(type: "bit", nullable: false),
                    StandardAppealDays = table.Column<int>(type: "int", nullable: false),
                    RecommendedAction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NphiesCodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AdjudicationImpact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorCodeMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CancellationResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferencedRequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CancellationRequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Intent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FocusResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FocusIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FocusIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResponseMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResponseStatusCode = table.Column<int>(type: "int", nullable: true),
                    AuthoredOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FhirTaskJson = table.Column<string>(type: "ntext", nullable: true),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancellationResponses_CancellationRequests_CancellationRequestId",
                        column: x => x.CancellationRequestId,
                        principalTable: "CancellationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CancellationResponses_Organizations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CancellationResponses_Organizations_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_PollingRecordId",
                table: "PollingRecords",
                column: "PollingRecordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRequests_Code",
                table: "CancellationRequests",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRequests_FocusIdentifierValue",
                table: "CancellationRequests",
                column: "FocusIdentifierValue");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRequests_OwnerId",
                table: "CancellationRequests",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRequests_ReasonCode",
                table: "CancellationRequests",
                column: "ReasonCode");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRequests_RequesterId",
                table: "CancellationRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRequests_Status",
                table: "CancellationRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRequests_TaskId",
                table: "CancellationRequests",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationResponses_CancellationRequestId",
                table: "CancellationResponses",
                column: "CancellationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationResponses_Code",
                table: "CancellationResponses",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationResponses_FocusIdentifierValue",
                table: "CancellationResponses",
                column: "FocusIdentifierValue");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationResponses_OwnerId",
                table: "CancellationResponses",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationResponses_RequesterId",
                table: "CancellationResponses",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationResponses_ResponseCode",
                table: "CancellationResponses",
                column: "ResponseCode");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationResponses_Status",
                table: "CancellationResponses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationResponses_TaskId",
                table: "CancellationResponses",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorCodeMasters_AllowsAppeal",
                table: "ErrorCodeMasters",
                column: "AllowsAppeal");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorCodeMasters_ErrorCategory",
                table: "ErrorCodeMasters",
                column: "ErrorCategory");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorCodeMasters_ErrorCode",
                table: "ErrorCodeMasters",
                column: "ErrorCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ErrorCodeMasters_IsActive",
                table: "ErrorCodeMasters",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorCodeMasters_Severity",
                table: "ErrorCodeMasters",
                column: "Severity");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationRequests_Organizations_RecipientId",
                table: "CommunicationRequests",
                column: "RecipientId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationRequests_Organizations_SenderId",
                table: "CommunicationRequests",
                column: "SenderId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationRequests_Patients_SubjectPatientId",
                table: "CommunicationRequests",
                column: "SubjectPatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Communications_Organizations_RecipientId",
                table: "Communications",
                column: "RecipientId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Communications_Organizations_SenderId",
                table: "Communications",
                column: "SenderId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Communications_Patients_SubjectPatientId",
                table: "Communications",
                column: "SubjectPatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PollingRecords_CancellationRequests_CancellationRequestId",
                table: "PollingRecords",
                column: "CancellationRequestId",
                principalTable: "CancellationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingRecords_CancellationResponses_CancellationResponseId",
                table: "PollingRecords",
                column: "CancellationResponseId",
                principalTable: "CancellationResponses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationRequests_Organizations_RecipientId",
                table: "CommunicationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationRequests_Organizations_SenderId",
                table: "CommunicationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationRequests_Patients_SubjectPatientId",
                table: "CommunicationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Communications_Organizations_RecipientId",
                table: "Communications");

            migrationBuilder.DropForeignKey(
                name: "FK_Communications_Organizations_SenderId",
                table: "Communications");

            migrationBuilder.DropForeignKey(
                name: "FK_Communications_Patients_SubjectPatientId",
                table: "Communications");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingRecords_CancellationRequests_CancellationRequestId",
                table: "PollingRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingRecords_CancellationResponses_CancellationResponseId",
                table: "PollingRecords");

            migrationBuilder.DropTable(
                name: "CancellationResponses");

            migrationBuilder.DropTable(
                name: "ErrorCodeMasters");

            migrationBuilder.DropTable(
                name: "CancellationRequests");

            migrationBuilder.DropIndex(
                name: "IX_PollingRecords_PollingRecordId",
                table: "PollingRecords");

            migrationBuilder.RenameColumn(
                name: "CancellationResponseId",
                table: "PollingRecords",
                newName: "TaskResponseId");

            migrationBuilder.RenameColumn(
                name: "CancellationRequestId",
                table: "PollingRecords",
                newName: "TaskRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_PollingRecords_CancellationResponseId",
                table: "PollingRecords",
                newName: "IX_PollingRecords_TaskResponseId");

            migrationBuilder.RenameIndex(
                name: "IX_PollingRecords_CancellationRequestId",
                table: "PollingRecords",
                newName: "IX_PollingRecords_TaskRequestId");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderId",
                table: "PollingRecords",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Communications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Communications",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "Communications",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "Communications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PayloadAttachmentTitle",
                table: "Communications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PayloadAttachmentContentType",
                table: "Communications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentifierValue",
                table: "Communications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentifierSystem",
                table: "Communications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommunicationId",
                table: "Communications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CategorySystem",
                table: "Communications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Communications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BasedOnResourceType",
                table: "Communications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BasedOnIdentifierValue",
                table: "Communications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BasedOnIdentifierSystem",
                table: "Communications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutResourceType",
                table: "Communications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutIdentifierValue",
                table: "Communications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutIdentifierSystem",
                table: "Communications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CommunicationRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "CommunicationRequests",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "CommunicationRequests",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "CommunicationRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentifierValue",
                table: "CommunicationRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentifierSystem",
                table: "CommunicationRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommunicationRequestId",
                table: "CommunicationRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CategorySystem",
                table: "CommunicationRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "CommunicationRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutResourceType",
                table: "CommunicationRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutIdentifierValue",
                table: "CommunicationRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutIdentifierSystem",
                table: "CommunicationRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TaskRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AuthoredOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FhirTaskJson = table.Column<string>(type: "ntext", nullable: true),
                    FocusIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FocusIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FocusResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Intent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReasonCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReasonCodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReasonText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskRequests_Organizations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskRequests_Organizations_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TaskRequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuthoredOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodeSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FhirTaskJson = table.Column<string>(type: "ntext", nullable: true),
                    FocusIdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FocusIdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FocusResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdentifierSystem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdentifierValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Intent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessageHeaderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferencedRequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResponseMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResponseStatusCode = table.Column<int>(type: "int", nullable: true),
                    ResultText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskResponses_Organizations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskResponses_Organizations_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskResponses_TaskRequests_TaskRequestId",
                        column: x => x.TaskRequestId,
                        principalTable: "TaskRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_IsAcknowledged",
                table: "PollingRecords",
                column: "IsAcknowledged");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_PollingRecordId",
                table: "PollingRecords",
                column: "PollingRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingRecords_ResponseStatus",
                table: "PollingRecords",
                column: "ResponseStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_CommunicationId",
                table: "Communications",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_Status",
                table: "Communications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_CommunicationRequestId",
                table: "CommunicationRequests",
                column: "CommunicationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRequests_Status",
                table: "CommunicationRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_Code",
                table: "TaskRequests",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_FocusIdentifierValue",
                table: "TaskRequests",
                column: "FocusIdentifierValue");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_OwnerId",
                table: "TaskRequests",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_ReasonCode",
                table: "TaskRequests",
                column: "ReasonCode");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_RequesterId",
                table: "TaskRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_Status",
                table: "TaskRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_TaskId",
                table: "TaskRequests",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResponses_Code",
                table: "TaskResponses",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResponses_FocusIdentifierValue",
                table: "TaskResponses",
                column: "FocusIdentifierValue");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResponses_OwnerId",
                table: "TaskResponses",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResponses_RequesterId",
                table: "TaskResponses",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResponses_ResponseCode",
                table: "TaskResponses",
                column: "ResponseCode");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResponses_Status",
                table: "TaskResponses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResponses_TaskId",
                table: "TaskResponses",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResponses_TaskRequestId",
                table: "TaskResponses",
                column: "TaskRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationRequests_Organizations_RecipientId",
                table: "CommunicationRequests",
                column: "RecipientId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationRequests_Organizations_SenderId",
                table: "CommunicationRequests",
                column: "SenderId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationRequests_Patients_SubjectPatientId",
                table: "CommunicationRequests",
                column: "SubjectPatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Communications_Organizations_RecipientId",
                table: "Communications",
                column: "RecipientId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Communications_Organizations_SenderId",
                table: "Communications",
                column: "SenderId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Communications_Patients_SubjectPatientId",
                table: "Communications",
                column: "SubjectPatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingRecords_TaskRequests_TaskRequestId",
                table: "PollingRecords",
                column: "TaskRequestId",
                principalTable: "TaskRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingRecords_TaskResponses_TaskResponseId",
                table: "PollingRecords",
                column: "TaskResponseId",
                principalTable: "TaskResponses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
