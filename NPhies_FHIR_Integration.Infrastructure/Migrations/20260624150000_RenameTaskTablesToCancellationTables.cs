using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPhies_FHIR_Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTaskTablesToCancellationTables : Migration
    {
        /// <inheritdoc />
     protected override void Up(MigrationBuilder migrationBuilder)
 {
        // Rename TaskRequests table to CancellationRequests
  migrationBuilder.RenameTable(
     name: "TaskRequests",
      newName: "CancellationRequests");

  // Rename TaskResponses table to CancellationResponses
          migrationBuilder.RenameTable(
        name: "TaskResponses",
     newName: "CancellationResponses");

            // Rename foreign key columns in PollingRecords table
 migrationBuilder.RenameColumn(
    name: "TaskRequestId",
      table: "PollingRecords",
     newName: "CancellationRequestId");

        migrationBuilder.RenameColumn(
       name: "TaskResponseId",
         table: "PollingRecords",
    newName: "CancellationResponseId");

            // Rename indexes in PollingRecords table
 migrationBuilder.RenameIndex(
             name: "IX_PollingRecords_TaskRequestId",
      table: "PollingRecords",
     newName: "IX_PollingRecords_CancellationRequestId");

          migrationBuilder.RenameIndex(
   name: "IX_PollingRecords_TaskResponseId",
         table: "PollingRecords",
  newName: "IX_PollingRecords_CancellationResponseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback: Rename tables back to original names
   migrationBuilder.RenameTable(
          name: "CancellationRequests",
           newName: "TaskRequests");

            migrationBuilder.RenameTable(
                name: "CancellationResponses",
     newName: "TaskResponses");

     // Rename foreign key columns back
   migrationBuilder.RenameColumn(
           name: "CancellationRequestId",
        table: "PollingRecords",
                newName: "TaskRequestId");

     migrationBuilder.RenameColumn(
                name: "CancellationResponseId",
      table: "PollingRecords",
                newName: "TaskResponseId");

     // Rename indexes back
migrationBuilder.RenameIndex(
              name: "IX_PollingRecords_CancellationRequestId",
                table: "PollingRecords",
           newName: "IX_PollingRecords_TaskRequestId");

       migrationBuilder.RenameIndex(
       name: "IX_PollingRecords_CancellationResponseId",
                table: "PollingRecords",
              newName: "IX_PollingRecords_TaskResponseId");
        }
    }
}
