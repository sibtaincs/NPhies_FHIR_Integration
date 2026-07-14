using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPhies_FHIR_Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppealTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create AppealRequests table
            migrationBuilder.CreateTable(
        name: "AppealRequests",
       schema: "RCM",
      columns: table => new
      {
          Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
          AppealNumber = table.Column<string>(type: "nvarchar(50)", nullable: false),
          AppealIdentifierSystem = table.Column<string>(type: "nvarchar(500)", nullable: false),
          AppealIdentifierValue = table.Column<string>(type: "nvarchar(100)", nullable: false),
          ClaimId = table.Column<string>(type: "nvarchar(450)", nullable: false),
          ClaimResponseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
          PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
          InsurerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
          ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
          AppealStatus = table.Column<string>(type: "nvarchar(50)", nullable: false),
          AppealLevel = table.Column<int>(type: "int", nullable: false),
          ErrorCodeBeingAppealed = table.Column<string>(type: "nvarchar(20)", nullable: false),
          ErrorDescription = table.Column<string>(type: "nvarchar(500)", nullable: true),
          AppealReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
          SupportingDocumentation = table.Column<string>(type: "nvarchar(max)", nullable: true),
          DenialDate = table.Column<DateTime>(type: "datetime2", nullable: false),
          AppealDeadlineDate = table.Column<DateTime>(type: "datetime2", nullable: false),
          AppealSubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
          ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
          ReviewCompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
          ExpectedDecisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
          AppealOutcome = table.Column<string>(type: "nvarchar(50)", nullable: true),
          ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
          DecisionExplanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
          AllowsEscalation = table.Column<bool>(type: "bit", nullable: false),
          EscalatedAppealId = table.Column<string>(type: "nvarchar(450)", nullable: true),
          IsActive = table.Column<bool>(type: "bit", nullable: false),
          IsWithdrawn = table.Column<bool>(type: "bit", nullable: false),
          WithdrawnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
          WithdrawalReason = table.Column<string>(type: "nvarchar(500)", nullable: true),
          InternalReferenceNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
          Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
          LastStatusUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
          CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
          UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
          IsDeleted = table.Column<bool>(type: "bit", nullable: false)
      },
     constraints: table =>
{
    table.PrimaryKey("PK_AppealRequests", x => x.Id);
});

            // Create AppealStatusHistory table
            migrationBuilder.CreateTable(
               name: "AppealStatusHistory",
          schema: "RCM",
                    columns: table => new
                    {
                        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        AppealId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        Status = table.Column<string>(type: "nvarchar(50)", nullable: false),
                        ChangedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                        ChangeReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        StatusChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                        Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                        IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                    },
          constraints: table =>
      {
          table.PrimaryKey("PK_AppealStatusHistory", x => x.Id);
          table.ForeignKey(
name: "FK_AppealStatusHistory_AppealRequests_AppealId",
column: x => x.AppealId,
principalSchema: "RCM",
principalTable: "AppealRequests",
principalColumn: "Id",
onDelete: ReferentialAction.Cascade);
      });

            // Create AppealDocuments table
            migrationBuilder.CreateTable(
   name: "AppealDocuments",
                 schema: "RCM",
               columns: table => new
               {
                   Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   AppealId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   DocumentType = table.Column<string>(type: "nvarchar(100)", nullable: false),
                   DocumentTitle = table.Column<string>(type: "nvarchar(200)", nullable: false),
                   DocumentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   FilePath = table.Column<string>(type: "nvarchar(500)", nullable: false),
                   FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                   MimeType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                   AttachedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                   IsVerified = table.Column<bool>(type: "bit", nullable: false),
                   Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                   UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                   IsDeleted = table.Column<bool>(type: "bit", nullable: false)
               },
                 constraints: table =>
     {
         table.PrimaryKey("PK_AppealDocuments", x => x.Id);
         table.ForeignKey(
          name: "FK_AppealDocuments_AppealRequests_AppealId",
 column: x => x.AppealId,
    principalSchema: "RCM",
         principalTable: "AppealRequests",
       principalColumn: "Id",
               onDelete: ReferentialAction.Cascade);
     });

            // Create Indexes for Performance
            migrationBuilder.CreateIndex(
                   name: "IX_AppealRequests_AppealNumber",
             schema: "RCM",
                     table: "AppealRequests",
                  column: "AppealNumber",
                   unique: true);

            migrationBuilder.CreateIndex(
               name: "IX_AppealRequests_ClaimId",
            schema: "RCM",
     table: "AppealRequests",
     column: "ClaimId");

            migrationBuilder.CreateIndex(
 name: "IX_AppealRequests_PatientId",
         schema: "RCM",
         table: "AppealRequests",
   column: "PatientId");

            migrationBuilder.CreateIndex(
                   name: "IX_AppealRequests_InsurerId",
                     schema: "RCM",
               table: "AppealRequests",
            column: "InsurerId");

            migrationBuilder.CreateIndex(
             name: "IX_AppealRequests_ProviderId",
               schema: "RCM",
          table: "AppealRequests",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                      name: "IX_AppealRequests_AppealStatus",
            schema: "RCM",
            table: "AppealRequests",
                   column: "AppealStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AppealRequests_AppealLevel",
                  schema: "RCM",
      table: "AppealRequests",
             column: "AppealLevel");

            migrationBuilder.CreateIndex(
                  name: "IX_AppealRequests_AppealDeadlineDate",
                     schema: "RCM",
        table: "AppealRequests",
           column: "AppealDeadlineDate");

            migrationBuilder.CreateIndex(
                 name: "IX_AppealRequests_IsActive",
                  schema: "RCM",
              table: "AppealRequests",
                   column: "IsActive");

            migrationBuilder.CreateIndex(
            name: "IX_AppealStatusHistory_AppealId",
             schema: "RCM",
         table: "AppealStatusHistory",
                column: "AppealId");

            migrationBuilder.CreateIndex(
          name: "IX_AppealDocuments_AppealId",
       schema: "RCM",
            table: "AppealDocuments",
                   column: "AppealId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
              name: "AppealDocuments",
           schema: "RCM");

            migrationBuilder.DropTable(
             name: "AppealStatusHistory",
            schema: "RCM");

            migrationBuilder.DropTable(
                 name: "AppealRequests",
          schema: "RCM");
        }
    }
}
