using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nphies.Core.Data.Common;
using System.Linq;

namespace Nphies.Core.Data.Entities
{
    public partial class ZyklusCoreContext : DbContext
    {
        public ZyklusCoreContext()
        {
        }

        public ZyklusCoreContext(DbContextOptions<ZyklusCoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HisIcd10AmDisease> HisIcd10AmDiseases { get; set; }
        public virtual DbSet<MiddbRcmServiceDetail> MiddbRcmServiceDetails { get; set; }
        public virtual DbSet<RcmAdapter> RcmAdapters { get; set; }
        public virtual DbSet<RcmAdapterColumnMappingMaster> RcmAdapterColumnMappingMasters { get; set; }
        public virtual DbSet<RcmAddResponseBinding> RcmAddResponseBindings { get; set; }
        public virtual DbSet<RcmAdapterMapping> RcmAdapterMappings { get; set; }
        public virtual DbSet<RcmAdapterParameter> RcmAdapterParameters { get; set; }
        public virtual DbSet<RcmAdapterParameterMaster> RcmAdapterParameterMasters { get; set; }
        public virtual DbSet<RcmAdapterTableColumnMaster> RcmAdapterTableColumnMasters { get; set; }
        public virtual DbSet<RcmAdapterType> RcmAdapterTypes { get; set; }
        public virtual DbSet<RcmAlert> RcmAlerts { get; set; }
        public virtual DbSet<RcmAlertDetail> RcmAlertDetails { get; set; }
        public virtual DbSet<RcmAlertGroup> RcmAlertGroups { get; set; }
        public virtual DbSet<RcmAlertGroupMember> RcmAlertGroupMembers { get; set; }
        public virtual DbSet<RcmAttachment> RcmAttachments { get; set; }
        public virtual DbSet<RcmBulkAction> RcmBulkActions { get; set; }
        public virtual DbSet<RcmBulkUploadClaim> RcmBulkUploadClaims { get; set; }
        public virtual DbSet<RcmBulkUploadClaimDentalDetail> RcmBulkUploadClaimDentalDetails { get; set; }
        public virtual DbSet<RcmBulkUploadClaimDiagnosisDetail> RcmBulkUploadClaimDiagnosisDetails { get; set; }
        public virtual DbSet<RcmBulkUploadClaimLabDetail> RcmBulkUploadClaimLabDetails { get; set; }
        public virtual DbSet<RcmBulkUploadClaimOpticalDetail> RcmBulkUploadClaimOpticalDetails { get; set; }
        public virtual DbSet<RcmBulkUploadClaimPatientDetail> RcmBulkUploadClaimPatientDetails { get; set; }
        public virtual DbSet<RcmBulkUploadClaimRadDetail> RcmBulkUploadClaimRadDetails { get; set; }
        public virtual DbSet<RcmBulkUploadClaimServiceDetail> RcmBulkUploadClaimServiceDetails { get; set; }
        public virtual DbSet<RcmBulkUploadClaimVitalSignDetail> RcmBulkUploadClaimVitalSignDetails { get; set; }
        public virtual DbSet<RcmBulkUploadDischargeDiagnosis> RcmBulkUploadDischargeDiagnoses { get; set; }
        public virtual DbSet<RcmBulkUploadDischargeMedicine> RcmBulkUploadDischargeMedicines { get; set; }
        public virtual DbSet<RcmBulkUploadDischargeSummary> RcmBulkUploadDischargeSummaries { get; set; }
        public virtual DbSet<RcmBulkUploadMedicalInfo> RcmBulkUploadMedicalInfos { get; set; }
        public virtual DbSet<RcmBulkUploadProcessLog> RcmBulkUploadProcessLogs { get; set; }
        public virtual DbSet<RcmBulkUploadSegment> RcmBulkUploadSegments { get; set; }
        public virtual DbSet<RcmBulkUploadTemplate> RcmBulkUploadTemplates { get; set; }
        public virtual DbSet<RcmBulkuploadErrorLog> RcmBulkuploadErrorLogs { get; set; }
        public virtual DbSet<RcmChangeLog> RcmChangeLogs { get; set; }
        public virtual DbSet<RcmChangeLogBkp> RcmChangeLogBkps { get; set; }
        public virtual DbSet<RcmCheckList> RcmCheckLists { get; set; }
        public virtual DbSet<RcmCheckListMidTable> RcmCheckListMidTables { get; set; }
        public virtual DbSet<RcmClaim> RcmClaims { get; set; }
        public virtual DbSet<RcmClaimDentalDetail> RcmClaimDentalDetails { get; set; }
        public virtual DbSet<RcmClaimDiagnosis> RcmClaimDiagnoses { get; set; }
        public virtual DbSet<RcmClaimDischargeDiagnosis> RcmClaimDischargeDiagnoses { get; set; }
        public virtual DbSet<RcmClaimDischargeMedicine> RcmClaimDischargeMedicines { get; set; }
        public virtual DbSet<RcmClaimDischargeSummary> RcmClaimDischargeSummaries { get; set; }
        public virtual DbSet<RcmClaimLabDetail> RcmClaimLabDetails { get; set; }
        public virtual DbSet<RcmClaimMedicalInfo> RcmClaimMedicalInfos { get; set; }
        public virtual DbSet<RcmClaimMedicalInfoDataDischargeSummary> RcmClaimMedicalInfoDataDischargeSummaries { get; set; }
        public virtual DbSet<RcmClaimMedicalInfoDatum> RcmClaimMedicalInfoData { get; set; }
        public virtual DbSet<RcmClaimMedicalInfoDetail> RcmClaimMedicalInfoDetails { get; set; }
        public virtual DbSet<RcmClaimOpticalDetail> RcmClaimOpticalDetails { get; set; }
        public virtual DbSet<RcmClaimPackageDetail> RcmClaimPackageDetails { get; set; }
        public virtual DbSet<RcmClaimPatientDetail> RcmClaimPatientDetails { get; set; }
        public virtual DbSet<RcmClaimRadDetail> RcmClaimRadDetails { get; set; }
        public virtual DbSet<RcmClaimSearch> RcmClaimSearches { get; set; }
        public virtual DbSet<RcmClaimServicesDetail> RcmClaimServicesDetails { get; set; }
        public virtual DbSet<RcmClaimTimeLine> RcmClaimTimeLines { get; set; }
        public virtual DbSet<RcmClaimVitalSign> RcmClaimVitalSigns { get; set; }
        public virtual DbSet<RcmClientReport> RcmClientReports { get; set; }
        public virtual DbSet<RcmClientReportNew> RcmClientReportNews { get; set; }
        public virtual DbSet<RcmClinic> RcmClinics { get; set; }
        public virtual DbSet<RcmDiagnosis> RcmDiagnoses { get; set; }
        public virtual DbSet<RcmDiagnosisDetailsTemp> RcmDiagnosisDetailsTemps { get; set; }
        public virtual DbSet<RcmDischargeDiagnosis> RcmDischargeDiagnoses { get; set; }
        public virtual DbSet<RcmDischargeMedicine> RcmDischargeMedicines { get; set; }
        public virtual DbSet<RcmDischargeSummary> RcmDischargeSummaries { get; set; }
        public virtual DbSet<RcmDoctor> RcmDoctors { get; set; }
        public virtual DbSet<RcmDoctorLicense> RcmDoctorLicenses { get; set; }
        public virtual DbSet<RcmDoctorSpeciality> RcmDoctorSpecialities { get; set; }
        public virtual DbSet<RcmEncounterMedicalDetail> RcmEncounterMedicalDetails { get; set; }
        public virtual DbSet<RcmExternalServiceConfiguration> RcmExternalServiceConfigurations { get; set; }
        public virtual DbSet<RcmExternalServiceDetail> RcmExternalServiceDetails { get; set; }
        public virtual DbSet<RcmFacility> RcmFacilities { get; set; }
        public virtual DbSet<RcmFacilityGroup> RcmFacilityGroups { get; set; }
        public virtual DbSet<RcmLabDetailsVidum> RcmLabDetailsVida { get; set; }
        public virtual DbSet<RcmMappingType> RcmMappingTypes { get; set; }
        public virtual DbSet<RcmMedicalInfoTemp> RcmMedicalInfoTemps { get; set; }
        public virtual DbSet<RcmNphiesclaimCommunicationLog> RcmNphiesclaimCommunicationLogs { get; set; }
        public virtual DbSet<RcmNphiesclaimLog> RcmNphiesclaimLogs { get; set; }
        public virtual DbSet<RcmNphiesclaimRejectionAttachmentsReference> RcmNphiesclaimRejectionAttachmentsReferences { get; set; }
        public virtual DbSet<RcmNphiesclaimRejectionRemark> RcmNphiesclaimRejectionRemarks { get; set; }
        public virtual DbSet<RcmNphiesprocessQueue> RcmNphiesprocessQueues { get; set; }
        public virtual DbSet<RcmOrganization> RcmOrganizations { get; set; }
        public virtual DbSet<RcmParameter> RcmParameters { get; set; }
        public virtual DbSet<RcmParameterGroup> RcmParameterGroups { get; set; }
        public virtual DbSet<RcmParameterType> RcmParameterTypes { get; set; }
        public virtual DbSet<RcmPayer> RcmPayers { get; set; }
        public virtual DbSet<RcmPayerDetail> RcmPayerDetails { get; set; }
        public virtual DbSet<RcmPayerPolicy> RcmPayerPolicies { get; set; }
        public virtual DbSet<RcmPayerPolicyClass> RcmPayerPolicyClasses { get; set; }
        public virtual DbSet<RcmPayerPolicyClassDetail> RcmPayerPolicyClassDetails { get; set; }
        public virtual DbSet<RcmPayerPolicyDetail> RcmPayerPolicyDetails { get; set; }
        public virtual DbSet<RcmPaymentReconcilation> RcmPaymentReconcilations { get; set; }
        public virtual DbSet<RcmPaymentReconcilationDetail> RcmPaymentReconcilationDetails { get; set; }
        public virtual DbSet<RcmProcessLog> RcmProcessLogs { get; set; }
        public virtual DbSet<RcmRadDetailsTemp> RcmRadDetailsTemps { get; set; }
        public virtual DbSet<RcmReferenceDimension> RcmReferenceDimensions { get; set; }
        public virtual DbSet<RcmRejectionReason> RcmRejectionReasons { get; set; }
        public virtual DbSet<RcmRemittanceLog> RcmRemittanceLogs { get; set; }
        public virtual DbSet<RcmReportDiscripancy> RcmReportDiscripancies { get; set; }
        public virtual DbSet<RcmReportRequest> RcmReportRequests { get; set; }
        public virtual DbSet<RcmReportTemplate> RcmReportTemplates { get; set; }
        public virtual DbSet<RcmReportTemplateBkp> RcmReportTemplateBkps { get; set; }
        public virtual DbSet<RcmReportdiscrepancy> RcmReportdiscrepancies { get; set; }
        public virtual DbSet<RcmReportsDayWise> RcmReportsDayWises { get; set; }
        public virtual DbSet<RcmReportsDayWiseHeader> RcmReportsDayWiseHeaders { get; set; }
        public virtual DbSet<RcmRuleEngineErrorLog> RcmRuleEngineErrorLogs { get; set; }
        public virtual DbSet<RcmServiceCatalog> RcmServiceCatalogs { get; set; }
        public virtual DbSet<RcmServiceCategory> RcmServiceCategories { get; set; }
        public virtual DbSet<RcmServiceCharge> RcmServiceCharges { get; set; }
        public virtual DbSet<RcmServiceComponent> RcmServiceComponents { get; set; }
        public virtual DbSet<RcmServiceComponentCharge> RcmServiceComponentCharges { get; set; }
        public virtual DbSet<RcmServiceDetail> RcmServiceDetails { get; set; }
        public virtual DbSet<RcmServiceGroup> RcmServiceGroups { get; set; }
        public virtual DbSet<RcmServiceSubGroup> RcmServiceSubGroups { get; set; }
        public virtual DbSet<RcmStandardCode> RcmStandardCodes { get; set; }
        public virtual DbSet<RcmSubmissionError> RcmSubmissionErrors { get; set; }
        public virtual DbSet<RcmSubmissionErrorLogDetail> RcmSubmissionErrorLogDetails { get; set; }
        public virtual DbSet<RcmTaskAssignment> RcmTaskAssignments { get; set; }
        public virtual DbSet<RcmTaskAssignmentLog> RcmTaskAssignmentLogs { get; set; }
        public virtual DbSet<RcmTaskManagementDetail> RcmTaskManagementDetails { get; set; }
        public virtual DbSet<RcmTaskManagementMaster> RcmTaskManagementMasters { get; set; }
        public virtual DbSet<RcmTemplateMapping> RcmTemplateMappings { get; set; }
        public virtual DbSet<RcmTransactionMapingValue> RcmTransactionMapingValues { get; set; }
        public virtual DbSet<RcmTransactionMapping> RcmTransactionMappings { get; set; }
        public virtual DbSet<RcmTransactionReferenceDetail> RcmTransactionReferenceDetails { get; set; }
        public virtual DbSet<RcmVitalSignDetailsTemp> RcmVitalSignDetailsTemps { get; set; }
        public virtual DbSet<ReEntityMaster> ReEntityMasters { get; set; }
        public virtual DbSet<ReEventMaster> ReEventMasters { get; set; }
        public virtual DbSet<RePayerEventRuleGroup> RePayerEventRuleGroups { get; set; }
        public virtual DbSet<ReResultSeverityLevel> ReResultSeverityLevels { get; set; }
        public virtual DbSet<ReRule> ReRules { get; set; }
        public virtual DbSet<ReRuleExpression> ReRuleExpressions { get; set; }
        public virtual DbSet<ReRuleFieldParameter> ReRuleFieldParameters { get; set; }
        public virtual DbSet<ReRuleFieldParameterGroup> ReRuleFieldParameterGroups { get; set; }
        public virtual DbSet<ReRuleGroup> ReRuleGroups { get; set; }
        public virtual DbSet<ReRuleResult> ReRuleResults { get; set; }
        public virtual DbSet<ReRuleUserFunction> ReRuleUserFunctions { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<RcmClaimNphiesPostTrail> ClaimNphiesPostTrails { get; set; }
        public virtual DbSet<CoDskResponseData> CoDskResponseData { get; set; }
        public virtual DbSet<CoDskEncounter> CoDskEncounter { get; set; }
        public virtual DbSet<RcmClaimSubmissionResponse> RcmClaimSubmissionResponses { get; set; }
        public virtual DbSet<RcmLabResult> RcmLabResults { get; set; }
        public virtual DbSet<Claim_Service_Observation> Claim_Service_Observations { get; set; }
        public virtual DbSet<CoDskEncounterDiagnosis> CoDskEncounterDiagnoses { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=ag4-lsnr;Database=ZyklusCore;user ID=ZyklusAdmin; password=Rf=z3m@N.nh]/e%M");
        //            }
        //        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoDskEncounter>(entity =>
            {
                entity.HasKey(e => new { e.OrganizationId, e.FacilityId, e.EncounterType, e.EncounterNo });

                entity.ToTable("CoDsk_Encounter");

                entity.Property(e => e.EncounterNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdmissionDate).HasColumnType("datetime");

                entity.Property(e => e.AdmissionType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AdmittedClinicName).HasMaxLength(250);

                entity.Property(e => e.ApprovedOn).HasColumnType("datetime");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CodedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DischargeDate).HasColumnType("datetime");

                entity.Property(e => e.DischargeStatus)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DrginvoicedAmount).HasColumnName("DRGInvoicedAmount");

                entity.Property(e => e.EhealthId)
                    .HasColumnName("EHealthId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.InsuranceExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.MedicalRecordNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MembershipNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MentalHealthLegalStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.MrpdoctorName)
                    .HasColumnName("MRPDoctorName")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PatientIdentificationNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientIdentificationType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PatientName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.PayerPolicyClassId).HasColumnName("PayerPolicyClassID");

                entity.Property(e => e.PayerPolicyId).HasColumnName("PayerPolicyID");

                entity.Property(e => e.PolicyNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RejectedOn).HasColumnType("datetime");

                entity.Property(e => e.RejectedRemarks)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBySupervisor).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CoDskICDmaster>(entity => 
             {
                 entity.HasKey(e => new { e.OrganizationId, e.Code });

                 entity.ToTable("CoDsk_ICD_master");

                 entity.Property(e => e.OrganizationId).HasMaxLength(4);

                 entity.Property(e => e.Code)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                 entity.Property(e => e.Description).HasColumnType("varchar(max)");

                 entity.Property(e => e.ShortDesc).HasColumnType("varchar(max)");

                 entity.Property(e => e.IsActive).HasColumnType("bit").HasDefaultValueSql("((0))");

                 entity.Property(e => e.CreatedBy).HasMaxLength(4);

                 entity.Property(e => e.CreatedOn).HasColumnType("datetime");

             });

            modelBuilder.Entity<CoDskEncounterDiagnosis>(entity =>
            {
                entity.HasKey(e => new { e.OrganizationId, e.FacilityId, e.EncounterType, e.EncounterNo, e.RecordType, e.ICDCode });

                entity.ToTable("CoDsk_EncounterDiagnosis");

               

                entity.Property(e => e.SequenceNo).HasMaxLength(4);

                entity.Property(e => e.OrganizationId).HasMaxLength(4);

                entity.Property(e => e.FacilityId).HasMaxLength(4);

                entity.Property(e => e.EncounterType).HasColumnType("TINYINT");

                entity.Property(e => e.EncounterNo)
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.RecordType).HasColumnType("TINYINT");

                entity.Property(e => e.ICDCode)
                           .HasMaxLength(12)
                           .IsUnicode(false);
                entity.Property(e => e.IsPrincipal).HasColumnType("bit").HasDefaultValueSql("((0))");

                entity.Property(e => e.IsActive).HasColumnType("bit").HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedBy).HasMaxLength(4);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(4);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.COF).HasColumnType("TINYINT");

                entity.Property(e => e.IsMorphoCode).HasColumnType("TINYINT");

                entity.Property(e => e.MorphoCodeDesc).HasColumnType("varchar(max)");

               
            });

            modelBuilder.Entity<CoDskResponseData>(entity =>
            {
                entity.HasKey(e => new { e.OrganizationId, e.FacilityId, e.EncounterType, e.EncounterNo, e.RecordType });

                entity.ToTable("CoDsk_ResponseData");

                entity.Property(e => e.EncounterNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Drg)
                    .HasColumnName("DRG")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Drgdesc)
                    .HasColumnName("DRGDesc")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.GrouperStatusCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Mdc)
                    .HasColumnName("MDC")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Mdcdesc)
                    .HasColumnName("MDCDesc")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<HisIcd10AmDisease>(entity =>
            {
                entity.HasKey(e => new { e.SetupId, e.CodeId })
                    .HasName("ICD10_AM_DISEASE_x")
                    .IsClustered(false);

                entity.ToTable("HIS_ICD10_AM_DISEASE");

                entity.Property(e => e.SetupId)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("SetupID")
                    .IsFixedLength();

                entity.Property(e => e.CodeId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("code_id")
                    .IsFixedLength();

                entity.Property(e => e.AsciiDesc)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ascii_Desc");

                entity.Property(e => e.AsciiShor)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ascii_Shor");

                entity.Property(e => e.AustCode).HasColumnName("Aust_Code");

                entity.Property(e => e.Chapter)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodeType).HasDefaultValueSql("((1))");

                entity.Property(e => e.CodeUnformatted)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasComputedColumnSql("(replace(replace(replace([code_id],char((32)),''),char((160)),''),'.',''))", false);

                entity.Property(e => e.ConceptCh)
                    .HasColumnType("datetime")
                    .HasColumnName("Concept_Ch");

                entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Dagger).HasColumnName("dagger");

                entity.Property(e => e.EditedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Effective)
                    .HasColumnType("datetime")
                    .HasColumnName("Effective_");

                entity.Property(e => e.Inactive).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Reactivate).HasColumnType("datetime");

                entity.Property(e => e.RowVer)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<MiddbRcmServiceDetail>(entity =>
            {
                entity.HasKey(e => e.ServiceDetailId)
                    .HasName("PK_Olaya_MIDDB_RCM_Service_Details");

                entity.ToTable("MIDDB_RCM_Service_Details");

                entity.Property(e => e.ApprovalNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.FacilityGroupId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FacilityGroupID")
                    .IsFixedLength();

                entity.Property(e => e.ObservationRemarks).IsUnicode(false);

                entity.Property(e => e.ParentServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProcessedOn).HasColumnType("datetime");

                entity.Property(e => e.RefundReferenceDate).HasColumnType("datetime");

                entity.Property(e => e.RefundReferenceNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceDeductibleAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDeductiblePct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceGrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceNetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceOrderDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceQuantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceReferenceIdentity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceReferenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceReimbursementAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StandardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TargetOrgId).HasColumnName("TargetOrgID");

                entity.Property(e => e.ToothNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmAdapter>(entity =>
            {
                entity.HasKey(e => e.AdapterId);

                entity.ToTable("RCM_Adapter");

                entity.Property(e => e.AdapterId).HasColumnName("AdapterID");

                entity.Property(e => e.AdapterName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.AdaptorCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FileType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.HasOne(d => d.AdapterType)
                    .WithMany(p => p.RcmAdapters)
                    .HasForeignKey(d => d.AdapterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_Adapter_RCM_AdapterType");
            });

            modelBuilder.Entity<RcmAdapterColumnMappingMaster>(entity =>
            {
                entity.ToTable("RCM_AdapterColumnMappingMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdapterId).HasColumnName("AdapterID");

                entity.Property(e => e.ColumnId).HasColumnName("ColumnID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Adapter)
                    .WithMany(p => p.RcmAdapterColumnMappingMasters)
                    .HasForeignKey(d => d.AdapterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_AdapterColumnMappingMaster_RCM_Adapter");
            });

            modelBuilder.Entity<RcmAdapterMapping>(entity =>
            {
                entity.HasKey(e => e.AdapterMappingId);

                entity.ToTable("RCM_AdapterMapping");

                entity.Property(e => e.AdapterMappingId).HasColumnName("AdapterMappingID");

                entity.Property(e => e.AdapterId).HasColumnName("AdapterID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterMasterId).HasColumnName("ParameterMasterID");

                entity.Property(e => e.ParameterType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Adapter)
                    .WithMany(p => p.RcmAdapterMappings)
                    .HasForeignKey(d => d.AdapterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_AdapterMapping_RCM_Adapter");

                entity.HasOne(d => d.ParameterMaster)
                    .WithMany(p => p.RcmAdapterMappings)
                    .HasForeignKey(d => d.ParameterMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_AdapterMapping_RCM_AdapterParameters");
            });

            modelBuilder.Entity<RcmAdapterParameter>(entity =>
            {
                entity.HasKey(e => e.ParameterId);

                entity.ToTable("RCM_AdapterParameters");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.AdapterTypeId).HasColumnName("AdapterTypeID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ParameterMasterId).HasColumnName("ParameterMasterID");

                entity.HasOne(d => d.AdapterType)
                    .WithMany(p => p.RcmAdapterParameters)
                    .HasForeignKey(d => d.AdapterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_AdapterParameters_RCM_AdapterType");

                entity.HasOne(d => d.ParameterMaster)
                    .WithMany(p => p.RcmAdapterParameters)
                    .HasForeignKey(d => d.ParameterMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_AdapterParameters_RCM_AdapterParameterMaster");
            });

            modelBuilder.Entity<RcmAdapterParameterMaster>(entity =>
            {
                entity.HasKey(e => e.ParameterMasterId)
                    .HasName("PK_RCM_ParameterMaster");

                entity.ToTable("RCM_AdapterParameterMaster");

                entity.Property(e => e.ParameterMasterId).HasColumnName("ParameterMasterID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmAdapterTableColumnMaster>(entity =>
            {
                entity.ToTable("RCM_AdapterTableColumnMaster");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ColumnDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HasChild).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");
            });

            modelBuilder.Entity<RcmAdapterType>(entity =>
            {
                entity.HasKey(e => e.AdapterTypeId)
                    .HasName("PK_RCM_AdapterType_1");

                entity.ToTable("RCM_AdapterType");

                entity.Property(e => e.AdapterTypeId).HasColumnName("AdapterTypeID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmAlert>(entity =>
            {
                entity.HasKey(e => new { e.AlertId, e.OrganizationId });

                entity.ToTable("RCM_Alerts");

                entity.Property(e => e.AlertId).ValueGeneratedOnAdd();

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsHtml).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsReply).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSystemGenerated).HasDefaultValueSql("((0))");

                entity.Property(e => e.Message).IsUnicode(false);

                entity.Property(e => e.MessageType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmAlertDetail>(entity =>
            {
                entity.HasKey(e => new { e.AlertDetailId, e.OrganizationId })
                    .HasName("PK_RCM_MessageDetails");

                entity.ToTable("RCM_AlertDetails");

                entity.Property(e => e.AlertDetailId).ValueGeneratedOnAdd();

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FollowUp).HasDefaultValueSql("((0))");

                entity.Property(e => e.FollowUpDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<RcmAlertGroup>(entity =>
            {
                entity.HasKey(e => e.AlertGroupId);

                entity.ToTable("RCM_AlertGroup");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<RcmAlertGroupMember>(entity =>
            {
                entity.HasKey(e => e.AlertGroupMemberId)
                    .HasName("PK_RCM_AlertGroupMember_1");

                entity.ToTable("RCM_AlertGroupMember");

                entity.Property(e => e.AlertGroupMemberId).HasColumnName("AlertGroupMemberID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.HasOne(d => d.AlertGroup)
                    .WithMany(p => p.RcmAlertGroupMembers)
                    .HasForeignKey(d => d.AlertGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ALERTGRUPID");
            });

            modelBuilder.Entity<RcmAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PK__RCM_Atta__442C64DE1837CEB6");

                entity.ToTable("RCM_Attachment");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FileType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.SourceType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RcmBulkAction>(entity =>
            {
                entity.ToTable("RCM_BulkAction");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<RcmBulkUploadClaim>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimId);

                entity.ToTable("RCM_BulkUploadClaims");

                entity.Property(e => e.BedNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClinicId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ClinicID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DoctorId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DoctorID");

                entity.Property(e => e.EligibilityCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EncounterEndTime).HasColumnType("datetime");

                entity.Property(e => e.EncounterStartTime).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.FacilityId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FacilityID");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PayerId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PayerID");

                entity.Property(e => e.PolicyNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreAuthNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RcmclinicId).HasColumnName("RCMClinicID");

                entity.Property(e => e.RcmdoctorId).HasColumnName("RCMDoctorID");

                entity.Property(e => e.RcmfacilityId).HasColumnName("RCMFacilityID");

                entity.Property(e => e.RcmpayerId).HasColumnName("RCMPayerID");

                entity.Property(e => e.ReferenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RoomNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VisitType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RcmBulkUploadClaimDentalDetail>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimDentallDetailId);

                entity.ToTable("RCM_BulkUploadClaimDentalDetails");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ToothNumber)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmBulkUploadClaimDiagnosisDetail>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimDiagnosisDetailId);

                entity.ToTable("RCM_BulkUploadClaimDiagnosisDetails");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DiagnoisInfoCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnoisInfoType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisCodeType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmBulkUploadClaimLabDetail>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimLabDetailId);

                entity.ToTable("RCM_BulkUploadClaimLabDetails");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabHigh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabLow)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabProfile)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LabResult).IsUnicode(false);

                entity.Property(e => e.LabSection)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LabTestName).IsUnicode(false);

                entity.Property(e => e.LabUnits)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ReferenceRange)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.VisitDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmBulkUploadClaimOpticalDetail>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimOpticalDetailId);

                entity.ToTable("RCM_BulkUploadClaimOpticalDetails");

                entity.Property(e => e.ContactLenses)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FramesLensIndicator)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LensSpecifications)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NumberOfPairs)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegularLensesType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmBulkUploadClaimPatientDetail>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimPatientDetailId);

                entity.ToTable("RCM_BulkUploadClaimPatientDetails");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NationalityId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NationalityID");

                entity.Property(e => e.PatientDob)
                    .HasColumnType("datetime")
                    .HasColumnName("PatientDOB");

                entity.Property(e => e.PatientFileNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientGender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientMembershipNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PatientMobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PatientNationality)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientSurnameFamilyName)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmBulkUploadClaimRadDetail>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimRadDetailId);

                entity.ToTable("RCM_BulkUploadClaimRadDetails");

                entity.Property(e => e.ClinicalData).IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RadiologyResult).IsUnicode(false);

                entity.Property(e => e.RadiologyVisitDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmBulkUploadClaimServiceDetail>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimServiceDetailId);

                entity.ToTable("RCM_BulkUploadClaimServiceDetails");

                entity.HasIndex(e => new { e.BulkUploadProcessLogId, e.MidTableReferenceId }, "Idx_RCM_BulkUploadClaimServiceDetails_1");

                entity.HasIndex(e => e.ServiceReferenceNumber, "NonClusteredIndex-20220329-175143");

                entity.Property(e => e.ApprovalNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterEndType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EncounterStartType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EncounterType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ObservationRemarks).IsUnicode(false);

                entity.Property(e => e.ParentServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RcmcategoryId).HasColumnName("RCMCategoryID");

                entity.Property(e => e.RcmserviceId).HasColumnName("RCMServiceID");

                entity.Property(e => e.RcmstandardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RCMStandardCode");

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceDeductibleAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDeductiblePct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ServiceGrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceNetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceQuantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceReferenceIdentity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ServiceReferenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceReimbursementAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.StandardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToothNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionReferenceDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionReferenceNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmBulkUploadClaimVitalSignDetail>(entity =>
            {
                entity.HasKey(e => e.BulkUploadClaimVitalSignDetailId);

                entity.ToTable("RCM_BulkUploadClaimVitalSignDetails");

                entity.Property(e => e.BloodPressure)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BodyMassIndex).IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.Height).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MainSymptoms).IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PhysicianNotesConditions).IsUnicode(false);

                entity.Property(e => e.SignificantSigns).IsUnicode(false);

                entity.Property(e => e.Temperature).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VitalSignCreatedon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<RcmBulkUploadDischargeDiagnosis>(entity =>
            {
                entity.HasKey(e => e.RcmdischargeDiagnosisId);

                entity.ToTable("RCM_BulkUploadDischargeDiagnosis");

                entity.Property(e => e.RcmdischargeDiagnosisId).HasColumnName("RCMDischargeDiagnosisID");

                entity.Property(e => e.CodeId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CodeID")
                    .IsFixedLength();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DiagnosisDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.Isactive).HasColumnName("ISActive");

                entity.Property(e => e.Remarks).HasMaxLength(80);
            });

            modelBuilder.Entity<RcmBulkUploadDischargeMedicine>(entity =>
            {
                entity.HasKey(e => e.RcmdischargeMedicineId)
                    .HasName("PK_RCM_BulkUploadDischargeMedicine_1");

                entity.ToTable("RCM_BulkUploadDischargeMedicine");

                entity.Property(e => e.RcmdischargeMedicineId).HasColumnName("RCMDischargeMedicineID");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Direction)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DoseDailyUnitId).HasColumnName("DoseDailyUnitID");

                entity.Property(e => e.DoseDetail)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DoseTimingId).HasColumnName("DoseTimingID");

                entity.Property(e => e.FrequencyId).HasColumnName("FrequencyID");

                entity.Property(e => e.GenericName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ReceivedOn)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.RouteId).HasColumnName("RouteID");
            });

            modelBuilder.Entity<RcmBulkUploadDischargeSummary>(entity =>
            {
                entity.HasKey(e => e.RcmdischargeSummaryId)
                    .HasName("PK_BulkUploadDischargeSummary_1");

                entity.ToTable("RCM_BulkUploadDischargeSummary");

                entity.Property(e => e.RcmdischargeSummaryId).HasColumnName("RCMDischargeSummaryID");

                entity.Property(e => e.AdmissionDate).HasColumnType("smalldatetime");

                entity.Property(e => e.BedId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BedID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.ClinicName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionOnDischarge)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DischargeDate).HasColumnType("smalldatetime");

                entity.Property(e => e.DischargeInstructions)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.DoctorName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EditedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Ercare)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("ERCare");

                entity.Property(e => e.FinalDiagnosis)
                    .IsRequired()
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.FollowupPlan)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.Investigations)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PastHistory)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PatientCondition)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Persentation)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PlanOfCare)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PlanedProcedure)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ReeivedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.RoomId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RoomID");

                entity.Property(e => e.SignificantFindings)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmBulkUploadMedicalInfo>(entity =>
            {
                entity.HasKey(e => e.BulkUploadMedicalInfoId)
                    .HasName("PK_BulkUploadMedicalInfoId");

                entity.ToTable("RCM_BulkUploadMedicalInfo");

                entity.HasIndex(e => new { e.IsProcessed, e.EncounterNo }, "IDX_RCM_BulkUploadMedicalInfo_01");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterValue).IsUnicode(false);

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmBulkUploadProcessLog>(entity =>
            {
                entity.HasKey(e => e.BulkUploadProcessLogId);

                entity.ToTable("RCM_BulkUploadProcessLog");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.FileCaption)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.SourceType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TotalCountRecords).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<RcmBulkUploadSegment>(entity =>
            {
                entity.HasKey(e => e.BulkUploadSegmentId)
                    .HasName("PK_RCM_BulkUploadSegmentId");

                entity.ToTable("RCM_BulkUploadSegments");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.SegmentMapWithHeader)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.SegmentName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SegmentType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RcmBulkUploadTemplate>(entity =>
            {
                entity.HasKey(e => e.BulkUploadTemplateId)
                    .HasName("PK_RCM_BulkUploadTemplateId");

                entity.ToTable("RCM_BulkUploadTemplate");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EntityMapWithHeader)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.TemplateType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RcmBulkuploadErrorLog>(entity =>
            {
                entity.HasKey(e => e.BulkuploadErrorId);

                entity.ToTable("RCM_BulkuploadErrorLog");

                entity.Property(e => e.BulkuploadProcessLogId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.SourceType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RcmChangeLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__RCM_Chan__5E5499A8687FF140");

                entity.ToTable("RCM_ChangeLog");

                entity.HasIndex(e => e.TransactionId, "Indx_ClaimID");

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.ChangeLogId).HasColumnName("ChangeLogID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.SessionId).HasColumnName("SessionID");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            });

            modelBuilder.Entity<RcmChangeLogBkp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RCM_ChangeLog_bkp");

                entity.Property(e => e.ChangeLogId).HasColumnName("ChangeLogID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LogId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("LogID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.SessionId).HasColumnName("SessionID");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            });

            modelBuilder.Entity<RcmCheckList>(entity =>
            {
                entity.HasKey(e => e.CheckListId)
                    .HasName("PK__RCM_Chec__56318361DB24652B");

                entity.ToTable("RCM_CheckList");

                entity.Property(e => e.CheckListId).HasColumnName("CheckListID");

                entity.Property(e => e.CheckList)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.CheckListN).HasColumnType("text");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.PayerPolicyId).HasColumnName("PayerPolicyID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");
            });

            modelBuilder.Entity<RcmCheckListMidTable>(entity =>
            {
                entity.HasKey(e => e.CheckListId)
                    .HasName("PK__RCM_Chec__563183611ADD6F47");

                entity.ToTable("RCM_CheckListMidTable");

                entity.Property(e => e.CheckListId).HasColumnName("CheckListID");

                entity.Property(e => e.CheckList)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.CheckListN).HasColumnType("text");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ReceivedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");
            });

            modelBuilder.Entity<RcmClaim>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId });

                entity.ToTable("RCM_Claim");

                entity.HasIndex(e => new { e.ClaimId, e.Status }, "ClaimId_Status-20221207-160221");

                entity.HasIndex(e => new { e.OrganizationId, e.Status, e.InvoiceDate }, "Indx_OrgID_Status_InvoiceDate");

                entity.HasIndex(e => new { e.OrganizationId, e.FacilityId, e.EncounterNo, e.Status }, "Indx_Org_FID_EncounterNo_Status");

                entity.HasIndex(e => e.OrganizationId, "Indx_RCM_Claim_1")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.ClaimId, e.PayerId, e.PayerPolicyId }, "Indx_RCM_Claim_2");

                entity.HasIndex(e => new { e.FacilityId, e.EncounterDateTime }, "Indx_RCM_Claim_3");

                entity.HasIndex(e => new { e.OrganizationId, e.FacilityId }, "Indx_RCM_Claim_OrgIdFalID");

                entity.HasIndex(e => new { e.InvoiceDate, e.FacilityId }, "InvoiceDate_Facility-20221207-160528");

                entity.HasIndex(e => new { e.ClinicId, e.OrganizationId, e.FacilityId }, "NonClusteredIndex-20220330-153423");

                entity.HasIndex(e => new { e.InvoiceDate, e.FacilityId, e.PayerId, e.Status }, "NonClusteredIndex-20221207-155646");

                entity.HasIndex(e => new { e.InvoiceDate, e.FacilityId, e.PayerId }, "NonClusteredIndex-20221207-160026");

                entity.Property(e => e.ClaimId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ApproverId)
                    .IsUnicode(false)
                    .HasColumnName("ApproverID");

                entity.Property(e => e.BatchBundleId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BatchIdentifier)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BatchRefNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BedNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClaimBundleId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClaimIdentifier)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClaimSubmisionRefrenceNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Comments)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.CommunicationIdentifier)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CommunicationUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DischargeDateTime).HasColumnType("datetime");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Discovery)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.EligibilityCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EncounterDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExternalReferenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.GrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.IsDocumentProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsErrorResolved)
                    .HasColumnName("isErrorResolved")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsProcessedByBgjob).HasColumnName("IsProcessedByBGJob");

                entity.Property(e => e.ItemReason)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MedicalError).HasDefaultValueSql("((0))");

                entity.Property(e => e.MedicalRejectionRemarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NphiesApprovalAuthRef)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NphiesApprovalIdentifier)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NphiesApprovalSystem)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NphiesEligibility)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NphiesEligibilitySystem)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NphieseRemarks).IsUnicode(false);

                entity.Property(e => e.OfflineApproval)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OfflineEligibility)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PatientShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PatientTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.PolicyNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreAuthNo).IsUnicode(false);

                entity.Property(e => e.ReasonCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefundAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RejectionCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseBundleId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ResponseBundleID");

                entity.Property(e => e.ResponseReceivedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.ReturnAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RoomNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SettlementAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SubStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SubmittedDateTime).HasColumnType("datetime");

                entity.Property(e => e.SubmittedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.TechnicalError).HasDefaultValueSql("((0))");

                entity.Property(e => e.TechnicalRejectionRemarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TotalApproved)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalSubmitted)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.RcmClaims)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_Claim_RCM_Organization");

                entity.HasOne(d => d.RcmPayer)
                    .WithMany(p => p.RcmClaims)
                    .HasForeignKey(d => new { d.PayerId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_Claim_RCM_Payer");
            });

            modelBuilder.Entity<RcmClaimDentalDetail>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId, e.ServiceId, e.ServiceLineitemNo, e.LineItemNo });

                entity.ToTable("RCM_ClaimDentalDetail");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ToothNumber)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmClaimDiagnosis>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId, e.DiagnosisCode, e.LineitemNo })
                    .HasName("PK_RCM_ClaimDiagnosis_1");

                entity.ToTable("RCM_ClaimDiagnosis");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.DiagnosisCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DiagnoisInfoCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnoisInfoType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Isdischarge).HasColumnName("ISDischarge");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.RcmClaim)
                    .WithMany(p => p.RcmClaimDiagnoses)
                    .HasForeignKey(d => new { d.ClaimId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ClaimDiagnosis_RCM_Claim");
            });

            modelBuilder.Entity<RcmClaimDischargeDiagnosis>(entity =>
            {
                entity.HasKey(e => e.RcmdischargeDiagnosisId);

                entity.ToTable("RCM_ClaimDischargeDiagnosis");

                entity.Property(e => e.RcmdischargeDiagnosisId).HasColumnName("RCMDischargeDiagnosisID");

                entity.Property(e => e.CodeId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CodeID")
                    .IsFixedLength();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DiagnosisDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.Isactive).HasColumnName("ISActive");

                entity.Property(e => e.Remarks).HasMaxLength(80);
            });

            modelBuilder.Entity<RcmClaimDischargeMedicine>(entity =>
            {
                entity.HasKey(e => e.ClaimDischargeMedicineId)
                    .HasName("PK_RCM_ClaimDischargeMedicine_1");

                entity.ToTable("RCM_ClaimDischargeMedicine");

                entity.Property(e => e.ClaimDischargeMedicineId).HasColumnName("ClaimDischargeMedicineID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Direction)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DoseDailyUnitId).HasColumnName("DoseDailyUnitID");

                entity.Property(e => e.DoseDetail)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DoseTimingId).HasColumnName("DoseTimingID");

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.FrequencyId).HasColumnName("FrequencyID");

                entity.Property(e => e.GenericName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ReceivedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.RouteId).HasColumnName("RouteID");
            });

            modelBuilder.Entity<RcmClaimDischargeSummary>(entity =>
            {
                entity.HasKey(e => e.RcmdischargeSummaryId)
                    .HasName("PK_ClaimDischargeSummary_1");

                entity.ToTable("RCM_ClaimDischargeSummary");

                entity.Property(e => e.RcmdischargeSummaryId).HasColumnName("RCMDischargeSummaryID");

                entity.Property(e => e.AdmissionDate).HasColumnType("smalldatetime");

                entity.Property(e => e.BedId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BedID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.ClinicName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionOnDischarge)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DischargeDate).HasColumnType("smalldatetime");

                entity.Property(e => e.DischargeInstructions)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.DoctorName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EditedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Ercare)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("ERCare");

                entity.Property(e => e.FinalDiagnosis)
                    .IsRequired()
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.FollowupPlan)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.Investigations)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PastHistory)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PatientCondition)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Persentation)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PlanOfCare)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PlanedProcedure)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.RoomId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RoomID");

                entity.Property(e => e.SignificantFindings)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmClaimLabDetail>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId, e.ServiceId, e.ServiceLineitemNo, e.LineItemNo });

                entity.ToTable("RCM_ClaimLabDetail");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.LabHigh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabLow)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabProfile)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LabResult).IsUnicode(false);

                entity.Property(e => e.LabSection)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LabTestName).IsUnicode(false);

                entity.Property(e => e.LabUnits)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceRange)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VisitDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmClaimMedicalInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RCM_ClaimMedicalInfo");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.MedicalInfoId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MedicalInfoID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<RcmClaimMedicalInfoDataDischargeSummary>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RCM_ClaimMedicalInfoDataDischargeSummary");

                entity.Property(e => e.ClaimMedicalInfoId).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterValue).IsUnicode(false);

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmClaimMedicalInfoDatum>(entity =>
            {
                entity.HasKey(e => e.ClaimMedicalInfoId)
                    .HasName("PK_RCM_ClaimMedicalInfoData_TEMP");

                entity.ToTable("RCM_ClaimMedicalInfoData");

                entity.HasIndex(e => new { e.OrganizationId, e.ClaimId }, "IDX_RCM_ClaimMedicalInfoData_1");

                entity.HasIndex(e => new { e.OrganizationId, e.FacilityId, e.ClaimId, e.MedicalRecordsType, e.EncounterNo }, "IDX_RCM_ClaimMedicalInfoData_2");

                entity.HasIndex(e => new { e.EncounterNo, e.MedicalRecordsType }, "IDX_RCM_ClaimMedicalInfoData_3");

                entity.HasIndex(e => e.ReceivedOn, "Indx_ReceivedOn");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrderBy).HasDefaultValueSql("((1))");

                entity.Property(e => e.ParameterName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterValue).IsUnicode(false);

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmClaimMedicalInfoDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RCM_ClaimMedicalInfoDetails");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.MedicalInfoDetailsId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MedicalInfoDetailsID");

                entity.Property(e => e.MedicalInfoId).HasColumnName("MedicalInfoID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterValue).IsUnicode(false);
            });

            modelBuilder.Entity<RcmClaimOpticalDetail>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId });

                entity.ToTable("RCM_ClaimOpticalDetail");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.HasOne(d => d.RcmClaim)
                    .WithOne(p => p.RcmClaimOpticalDetail)
                    .HasForeignKey<RcmClaimOpticalDetail>(d => new { d.ClaimId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ClaimOpticalDetail_RCM_Claim");
            });

            modelBuilder.Entity<RcmClaimPackageDetail>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId, e.ServiceId, e.LineitemNo })
                    .HasName("PK_RCM_ClaimPackageDetail_1");

                entity.ToTable("RCM_ClaimPackageDetails");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ActivityEncounterType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovalNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyTaxPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterEndType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EncounterStartType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentInvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PatientTaxPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RefundAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RejectionDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ServiceGrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceNetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServicePatientShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServicePatientSharePct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceQuantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceReferenceIdentity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceReferenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.SettlementAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StandardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmClaimPatientDetail>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId });

                entity.ToTable("RCM_ClaimPatientDetail");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Address1)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NationalityId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NationalityID");

                entity.Property(e => e.PatientDob)
                    .HasColumnType("datetime")
                    .HasColumnName("PatientDOB");

                entity.Property(e => e.PatientFileNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientMembershipNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PatientMobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PatientNationality)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientSurnameFamilyName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TownCity)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.RcmClaim)
                    .WithOne(p => p.RcmClaimPatientDetail)
                    .HasForeignKey<RcmClaimPatientDetail>(d => new { d.ClaimId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ClaimPatientDetail_RCM_Claim");
            });

            modelBuilder.Entity<RcmClaimRadDetail>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId, e.ServiceId, e.ServiceLineItemNo, e.LineItemNo });

                entity.ToTable("RCM_ClaimRadDetail");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ClinicalData).IsUnicode(false);

                entity.Property(e => e.RadiologyResult).IsUnicode(false);

                entity.Property(e => e.RadiologyVisitDate).HasColumnType("datetime");
            });
        
            
            
            modelBuilder.Entity<RcmClaimSearch>(entity =>
            {
                entity.ToTable("RCM_ClaimSearch");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.SearchData)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SearchName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<RcmClaimServicesDetail>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId, e.ServiceId, e.LineitemNo })
                    .HasName("PK_RCM_ClaimServicesDetail_1");

                entity.ToTable("RCM_ClaimServicesDetail");

                entity.HasIndex(e => e.InvoiceDate, "INDX_InvoiceDate");

                entity.HasIndex(e => e.RowId, "INDX_ROWID")
                    .IsUnique();

                entity.HasIndex(e => new { e.ClaimId, e.OrganizationId }, "Indx_ClaimID_ORGID");

                entity.HasIndex(e => new { e.OrganizationId, e.ClaimId, e.ServiceId, e.ServiceReferenceNumber, e.ServiceReferenceIdentity }, "Indx_Org_CID_SID_SRN_SRI");

                entity.HasIndex(e => new { e.ClaimId, e.ServiceCode, e.ServiceReferenceNumber, e.ServiceReferenceIdentity }, "Indx_ServiceCode_Reference_Identity");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ActivityEncounterType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovalNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovalNoOld)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BenefitAmount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClaimIdentifier)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyTaxPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DaysSupplies)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterEndType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EncounterStartType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HasChild).HasDefaultValueSql("((0))");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.ItemReason)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NphieseRemarks).IsUnicode(false);

                entity.Property(e => e.ParentServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentServiceId).HasDefaultValueSql("((0))");

                entity.Property(e => e.PatientTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PatientTaxPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReasonCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefundAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RejectionDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseReceivedOn).HasColumnType("datetime");

                entity.Property(e => e.ReturnAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RowId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ROW_ID");

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ServiceGrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceNetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServicePatientShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServicePatientSharePct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceQuantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceReferenceIdentity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ServiceReferenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.SettlementAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StandardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandardCode2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubmittedAmount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubmittedQuantity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxApproved).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ToothNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.RcmClaim)
                    .WithMany(p => p.RcmClaimServicesDetails)
                    .HasForeignKey(d => new { d.ClaimId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ClaimServicesDetail_RCM_Claim");
            });

            modelBuilder.Entity<RcmClaimSubmissionResponse>(entity =>
            {
                entity.ToTable("RCM_ClaimSubmissionResponse");

                entity.HasIndex(e => e.ClaimBundleId, "IX_RCM_ClaimSubmissionResponse_ClaimBundleId");

                entity.HasIndex(e => e.ClaimIdentifier, "IX_RCM_ClaimSubmissionResponse_ClaimIdentifier");

                entity.HasIndex(e => e.ResponseBundleId, "IX_RCM_ClaimSubmissionResponse_ResponseBundleId");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClaimBundleId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClaimIdentifier)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ReceivedAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");

                entity.Property(e => e.ResponseBundleId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubmittedAmount).HasColumnType("decimal(18, 2)");
            });


            modelBuilder.Entity<RcmClaimTimeLine>(entity =>
            {
                entity.ToTable("RCM_ClaimTimeLine");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApprovedOn).HasColumnType("datetime");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.RejectedOn).HasColumnType("datetime");

                entity.Property(e => e.ReviewedOn).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VerifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmClaimVitalSign>(entity =>
            {
                entity.HasKey(e => new { e.ClaimId, e.OrganizationId });

                entity.ToTable("RCM_ClaimVitalSigns");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.BloodPressure)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmergencyIndicator)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyeAxisDistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyeAxisNear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyeBifocal)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyeCylinderDistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyeCylinderNear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyePddistance)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LeftEyePDDistance");

                entity.Property(e => e.LeftEyePdnear)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LeftEyePDNear");

                entity.Property(e => e.LeftEyePrismDistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyePrismNear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyeSphereDistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyeSphereNear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeftEyeVadistance)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LeftEyeVADistance");

                entity.Property(e => e.LeftEyeVanear)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LeftEyeVANear");

                entity.Property(e => e.MainSymptoms).IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PhysicianNotesConditions).IsUnicode(false);

                entity.Property(e => e.RightEyeAxisDistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyeAxisNear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyeBifocal)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyeCylinderDistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyeCylinderNear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyePddistance)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RightEyePDDistance");

                entity.Property(e => e.RightEyePdnear)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RightEyePDNear");

                entity.Property(e => e.RightEyePrismDistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyePrismNear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyeSphereDistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyeSphereNear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RightEyeVadistance)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RightEyeVADistance");

                entity.Property(e => e.RightEyeVanear)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RightEyeVANear");

                entity.Property(e => e.SignificantSigns).IsUnicode(false);

                entity.Property(e => e.Vertex)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VitalSignCreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("VitalSignCreatedON");

                entity.HasOne(d => d.RcmClaim)
                    .WithOne(p => p.RcmClaimVitalSign)
                    .HasForeignKey<RcmClaimVitalSign>(d => new { d.ClaimId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ClaimVitalSigns_RCM_Claim");
            });

            modelBuilder.Entity<RcmClientReport>(entity =>
            {
                entity.HasKey(e => e.ClientReportId)
                    .HasName("PK__RCM_Clie__A3C8EA138142166A");

                entity.ToTable("RCM_ClientReport");

                entity.Property(e => e.ClientReportId).HasColumnName("ClientReportID");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ClinicName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyTaxAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyType)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DoctorName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.GrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PatientCardId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PatientName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PatientShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PatientTaxAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PatientType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PayerName)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmClientReportNew>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RCM_ClientReportNew");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Branch)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ClientReportId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ClientReportID");

                entity.Property(e => e.CompanyGroupName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyShareInclusiveOfVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetRevenueAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PatientName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PatientShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PatientShareInclusiveOfVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PharmacyLocation)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.RefundOrReturnNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefundReturnDate).HasColumnType("datetime");

                entity.Property(e => e.TotalRevenueInclusiveOfVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalVatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VatOnCompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VatOnPatientShare).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<RcmClinic>(entity =>
            {
                entity.HasKey(e => new { e.ClinicId, e.FacilityId, e.FacilityGroupId, e.OrganizationId });

                entity.ToTable("RCM_Clinic");

                entity.HasIndex(e => new { e.ClinicId, e.OrganizationId, e.FacilityId }, "NonClusteredIndex-20220330-152353");

                entity.Property(e => e.ClinicId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ClinicID");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ClinicName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ClinicNameN)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsDental)
                    .HasColumnName("isDental")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.RcmFacility)
                    .WithMany(p => p.RcmClinics)
                    .HasForeignKey(d => new { d.FacilityId, d.OrganizationId, d.FacilityGroupId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_Clinic_RCM_Facility");
            });

            modelBuilder.Entity<RcmDiagnosis>(entity =>
            {
                entity.HasKey(e => e.DiagnosisCode);

                entity.ToTable("RCM_Diagnosis");

                entity.Property(e => e.DiagnosisCode)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisDescription)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<RcmDiagnosisDetailsTemp>(entity =>
            {
                entity.ToTable("RCM_DiagnosisDetailsTemp");

                entity.Property(e => e.DiagnoisInfoCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnoisInfoType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisCodeType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmDischargeDiagnosis>(entity =>
            {
                entity.HasKey(e => new { e.RcmdischargeDiagnosisId, e.CodeId })
                    .HasName("PK_INP_DischargeDiagnosis");

                entity.ToTable("RCM_DischargeDiagnosis");

                entity.Property(e => e.RcmdischargeDiagnosisId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RCMDischargeDiagnosisID");

                entity.Property(e => e.CodeId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("CodeID")
                    .IsFixedLength();

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DiagnosisDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Poastatus)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POAStatus");

                entity.Property(e => e.Remarks).HasMaxLength(80);
            });

            modelBuilder.Entity<RcmDischargeMedicine>(entity =>
            {
                entity.ToTable("RCM_DischargeMedicine");

                entity.Property(e => e.RcmdischargeMedicineId).HasColumnName("RCMDischargeMedicineID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.DoseDailyUnitId).HasColumnName("DoseDailyUnitID");

                entity.Property(e => e.DoseDetail)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DoseTimingId).HasColumnName("DoseTimingID");

                entity.Property(e => e.EditedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.FrequencyId).HasColumnName("FrequencyID");

                entity.Property(e => e.GenericName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MedicineDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PharmacistRemarks)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.RouteId).HasColumnName("RouteID");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<RcmDischargeSummary>(entity =>
            {
                entity.ToTable("RCM_DischargeSummary");

                entity.Property(e => e.RcmdischargeSummaryId).HasColumnName("RCMDischargeSummaryID");

                entity.Property(e => e.AdmissionDate).HasColumnType("smalldatetime");

                entity.Property(e => e.BedId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BedID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.ClinicName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionOnDischarge)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.DischargeDate).HasColumnType("smalldatetime");

                entity.Property(e => e.DischargeInstructions)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.DoctorName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EditedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Ercare)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("ERCare");

                entity.Property(e => e.FinalDiagnosis)
                    .IsRequired()
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.FollowupPlan)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.Investigations)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PastHistory)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Persentation)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PlanOfCare)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PlanedProcedure)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.RoomId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RoomID");

                entity.Property(e => e.SignificantFindings)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmDoctor>(entity =>
            {
                entity.HasKey(e => new { e.DoctorId, e.OrganizationId, e.FacilityGroupId, e.FacilityId })
                    .HasName("PK_RCM_Doctor_1");

                entity.ToTable("RCM_Doctor");

                entity.Property(e => e.DoctorId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DoctorID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DateofBirth).HasColumnType("date");

                entity.Property(e => e.DateofJoining).HasColumnType("smalldatetime");

                entity.Property(e => e.DoctorName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DoctorNameN)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EmailExternal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailInternal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FaxNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NationalityId).HasColumnName("NationalityID");

                entity.Property(e => e.PhoneResidence)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmDoctorLicense>(entity =>
            {
                entity.HasKey(e => new { e.LicenseId, e.DoctorId, e.OrganizationId, e.FacilityGroupId, e.FacilityId });

                entity.ToTable("RCM_DoctorLicenses");

                entity.Property(e => e.LicenseId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("LicenseID");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DoctorLogin)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DoctorPwd)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LicenseExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.RcmDoctor)
                    .WithMany(p => p.RcmDoctorLicenses)
                    .HasForeignKey(d => new { d.DoctorId, d.OrganizationId, d.FacilityGroupId, d.FacilityId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_DoctorLicenses_RCM_Doctor");
            });

            modelBuilder.Entity<RcmDoctorSpeciality>(entity =>
            {
                entity.HasKey(e => new { e.SpecialityId, e.DoctorId, e.OrganizationId, e.FacilityGroupId, e.FacilityId });

                entity.ToTable("RCM_DoctorSpecialities");

                entity.Property(e => e.SpecialityId).HasColumnName("SpecialityID");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.RcmDoctor)
                    .WithMany(p => p.RcmDoctorSpecialities)
                    .HasForeignKey(d => new { d.DoctorId, d.OrganizationId, d.FacilityGroupId, d.FacilityId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_DoctorSpecialities_RCM_Doctor");
            });

            modelBuilder.Entity<RcmEncounterMedicalDetail>(entity =>
            {
                entity.HasKey(e => e.MedicalInfoId);

                entity.ToTable("RCM_EncounterMedicalDetail");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterNo)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityGroupId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FacilityGroupID")
                    .IsFixedLength();

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.MedicalData).IsUnicode(false);

                entity.Property(e => e.PatientMrn)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmExternalServiceConfiguration>(entity =>
            {
                entity.ToTable("RCM_ExternalServiceConfiguration");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConfigKey)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Config_Key");

                entity.Property(e => e.ConfigValue)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Config_Value");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");
            });

            modelBuilder.Entity<RcmExternalServiceDetail>(entity =>
            {
                entity.ToTable("RCM_ExternalServiceDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConstantName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DefaultValue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Module)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmFacility>(entity =>
            {
                entity.HasKey(e => new { e.FacilityId, e.OrganizationId, e.FacilityGroupId })
                    .HasName("PK_RCM_Facility_1");

                entity.ToTable("RCM_Facility");

                entity.Property(e => e.FacilityId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FacilityID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.AddressArea)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AddressStreet)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.ClientName).HasMaxLength(40);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalCode2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityNameAlias)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityNameN)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FaxNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NphiesLicenseNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(264)
                    .IsFixedLength();

                entity.Property(e => e.PhoneOffice1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneOffice2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.TaxRegistrationNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.WebSiteUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.RcmFacilityGroup)
                    .WithMany(p => p.RcmFacilities)
                    .HasForeignKey(d => new { d.FacilityGroupId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_Facility_RCM_Organization");
            });

            modelBuilder.Entity<RcmFacilityGroup>(entity =>
            {
                entity.HasKey(e => new { e.FacilityGroupId, e.OrganizationId });

                entity.ToTable("RCM_FacilityGroup");

                entity.Property(e => e.FacilityGroupId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FacilityGroupID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.AddressArea)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AddressStreet)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityGroupName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityGroupNameN)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FaxNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PhoneOffice1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneOffice2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.TaxRegistrationNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.WebSiteUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.RcmFacilityGroups)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_FacilityGroup_RCM_Organization");
            });

            modelBuilder.Entity<RcmLabDetailsVidum>(entity =>
            {
                entity.HasKey(e => e.LabDetailId)
                    .HasName("PK_RCM_LabDetails");

                entity.ToTable("RCM_LabDetailsVida");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabHigh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabLow)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabProfile)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LabResult).IsUnicode(false);

                entity.Property(e => e.LabResultTextBased).IsUnicode(false);

                entity.Property(e => e.LabSection)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LabTestName).IsUnicode(false);

                entity.Property(e => e.LabUnits)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");

                entity.Property(e => e.ReferenceRange)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceLineItemNo).HasDefaultValueSql("((0))");

                entity.Property(e => e.VisitDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmMappingType>(entity =>
            {
                entity.HasKey(e => e.TransactionTypeId)
                    .HasName("PK_RCM_MappingType_1");

                entity.ToTable("RCM_MappingType");

                entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");

                entity.Property(e => e.TransactionTypeName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmMedicalInfoTemp>(entity =>
            {
                entity.HasKey(e => e.MedicalRecordsId)
                    .HasName("PK_RCM_MedicalInfo");

                entity.ToTable("RCM_MedicalInfoTemp");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterValue).IsUnicode(false);

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");

                entity.Property(e => e.ReferenecId).HasColumnName("ReferenecID");
            });

            modelBuilder.Entity<RcmNphiesclaimCommunicationLog>(entity =>
            {
                entity.HasKey(e => new { e.OrganizationId, e.FacilityId, e.ServiceReferenceNo, e.ClaimIdentifier, e.LineItemNo });

                entity.ToTable("RCM_NphiesclaimCommunicationLog");

                entity.Property(e => e.ClaimIdentifier)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.IdentifierSystem)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdentifierValue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RequestBundleId)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseBundleId)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmNphiesclaimLog>(entity =>
            {
                entity.HasKey(e => e.ClaimLogId);

                entity.ToTable("RCM_NPHIESClaimLog");

                entity.Property(e => e.ApprovedQuantity)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ClaimBundleId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ClaimBundleID");

                entity.Property(e => e.ClaimIdentifier)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.FacilityGroup)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NphiesStatus).HasColumnName("NPHIES_Status");

                entity.Property(e => e.ReasonCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.SubmittedQuantity)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TaxApproved)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmNphiesclaimRejectionAttachmentsReference>(entity =>
            {
                entity.ToTable("RCM_NPHIESClaimRejectionAttachmentsReference");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.ClaimIdentifier)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RcmNphiesclaimRejectionRemarksId).HasColumnName("RCM_NPHIESClaimRejectionRemarks_Id");

                entity.Property(e => e.ServiceReferenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.RcmNphiesclaimRejectionAttachmentsReferences)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_NPHIESClaimRejectionAttachmentsReference_RCM_NPHIESClaimRejectionAttachments");

                entity.HasOne(d => d.RcmNphiesclaimRejectionRemarks)
                    .WithMany(p => p.RcmNphiesclaimRejectionAttachmentsReferences)
                    .HasForeignKey(d => d.RcmNphiesclaimRejectionRemarksId)
                    .HasConstraintName("FK__RCM_NPHIE__RCM_N__32B6742D");
            });

            modelBuilder.Entity<RcmNphiesclaimRejectionRemark>(entity =>
            {
                entity.ToTable("RCM_NPHIESClaimRejectionRemarks");

                entity.Property(e => e.ClaimIdentifier)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.ServiceReferenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmNphiesprocessQueue>(entity =>
            {
                entity.HasKey(e => e.ProcessId);

                entity.ToTable("RCM_NPHIESProcessQueue");

                entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.FromDate).HasColumnType("smalldatetime");

                entity.Property(e => e.IncludePreAuthRef).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsProcessed).HasColumnName("isProcessed");

                entity.Property(e => e.Limit).HasColumnName("limit");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ProcessedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.ToDate).HasColumnType("smalldatetime");

                entity.Property(e => e.VerifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmOrganization>(entity =>
            {
                entity.HasKey(e => e.OrganizationId);

                entity.ToTable("RCM_Organization");

                entity.Property(e => e.OrganizationId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrganizationID");

                entity.Property(e => e.AddressArea)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AddressStreet)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FaxNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationNameN)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.PhoneOffice1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneOffice2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.TaxRegistrationNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.WebSiteUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmParameter>(entity =>
            {
                entity.ToTable("RCM_Parameter");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterCodeDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterCodeDescriptionN).HasMaxLength(200);
            });

            modelBuilder.Entity<RcmParameterGroup>(entity =>
            {
                entity.ToTable("RCM_ParameterGroup");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterGroupName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsFixedLength();

                entity.Property(e => e.ParameterGroupNameN).HasMaxLength(250);
            });

            modelBuilder.Entity<RcmParameterType>(entity =>
            {
                entity.ToTable("RCM_ParameterType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParameterTypeName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterTypeNameN).HasMaxLength(200);
            });

            modelBuilder.Entity<RcmPayer>(entity =>
            {
                entity.HasKey(e => new { e.PayerId, e.OrganizationId })
                    .HasName("PK_RCM_Payer_1");

                entity.ToTable("RCM_Payer");

                entity.Property(e => e.PayerId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PayerID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CardFormat)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CompanyURL");

                entity.Property(e => e.ContactMobile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPersonEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NphiesLicenseNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PayerName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PayerNameN)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.PhoneOffice1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneOffice2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Pobox)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("POBox");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.TaxRegistrationNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.RcmPayers)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_Payer_RCM_Organization");
            });

            modelBuilder.Entity<RcmPayerDetail>(entity =>
            {
                entity.HasKey(e => new { e.PayerId, e.OrganizationId, e.DimensionId });

                entity.ToTable("RCM_PayerDetail");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.DimensionId).HasColumnName("DimensionID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DimReference)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.RcmPayerDetails)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PayerDetail_RCM_ReferenceDimension");

                entity.HasOne(d => d.RcmPayer)
                    .WithMany(p => p.RcmPayerDetails)
                    .HasForeignKey(d => new { d.PayerId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PayerDetail_RCM_Payer");
            });

            modelBuilder.Entity<RcmPayerPolicy>(entity =>
            {
                entity.HasKey(e => new { e.PayerPolicyId, e.OrganizationId, e.PayerId })
                    .HasName("PK_RCM_PayerPolicy_1");

                entity.ToTable("RCM_PayerPolicy");

                entity.HasIndex(e => new { e.OrganizationId, e.PayerId }, "Indx_Org_PID");

                entity.Property(e => e.PayerPolicyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PayerPolicyID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PayerPolicyExpiryDate).HasColumnType("date");

                entity.Property(e => e.PayerPolicyName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PayerPolicyNameN)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PayerPolicyNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.RcmPayer)
                    .WithMany(p => p.RcmPayerPolicies)
                    .HasForeignKey(d => new { d.PayerId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PayerPolicy_RCM_Payer");
            });

            modelBuilder.Entity<RcmPayerPolicyClass>(entity =>
            {
                entity.HasKey(e => new { e.PayerPolicyClassId, e.OrganizationId, e.PayerPolicyId, e.PayerId });

                entity.ToTable("RCM_PayerPolicyClass");

                entity.Property(e => e.PayerPolicyClassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PayerPolicyClassID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PayerPolicyId).HasColumnName("PayerPolicyID");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PayerPolicyClassName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PayerPolicyClassNameN).HasMaxLength(200);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ValidFrom).HasColumnType("smalldatetime");

                entity.Property(e => e.ValidTo).HasColumnType("smalldatetime");

                entity.HasOne(d => d.RcmPayerPolicy)
                    .WithMany(p => p.RcmPayerPolicyClasses)
                    .HasForeignKey(d => new { d.PayerPolicyId, d.OrganizationId, d.PayerId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PayerPolicyClass_RCM_PayerPolicy");
            });

            modelBuilder.Entity<RcmPayerPolicyClassDetail>(entity =>
            {
                entity.HasKey(e => new { e.PayerPolicyClassId, e.OrganizationId, e.PayerPolicyId, e.PayerId, e.DimensionId });

                entity.ToTable("RCM_PayerPolicyClassDetail");

                entity.Property(e => e.PayerPolicyClassId).HasColumnName("PayerPolicyClassID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PayerPolicyId).HasColumnName("PayerPolicyID");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.DimensionId).HasColumnName("DimensionID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DimReference)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.RcmPayerPolicyClassDetails)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PayerPolicyClassDetail_RCM_ReferenceDimension");

                entity.HasOne(d => d.RcmPayerPolicyClass)
                    .WithMany(p => p.RcmPayerPolicyClassDetails)
                    .HasForeignKey(d => new { d.PayerPolicyClassId, d.OrganizationId, d.PayerPolicyId, d.PayerId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PayerPolicyClassDetail_RCM_PayerPolicyClass");
            });

            modelBuilder.Entity<RcmPayerPolicyDetail>(entity =>
            {
                entity.HasKey(e => new { e.PayerPolicyId, e.OrganizationId, e.PayerId, e.DimensionId });

                entity.ToTable("RCM_PayerPolicyDetail");

                entity.Property(e => e.PayerPolicyId).HasColumnName("PayerPolicyID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.DimensionId).HasColumnName("DimensionID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DimReference)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.RcmPayerPolicyDetails)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PayerPolicyDetail_RCM_ReferenceDimension");

                entity.HasOne(d => d.RcmPayerPolicy)
                    .WithMany(p => p.RcmPayerPolicyDetails)
                    .HasForeignKey(d => new { d.PayerPolicyId, d.OrganizationId, d.PayerId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PayerPolicyDetail_RCM_PayerPolicy");
            });

            modelBuilder.Entity<RcmPaymentReconcilation>(entity =>
            {
                entity.HasKey(e => e.PaymentReconcilationId);

                entity.ToTable("RCM_PaymentReconcilation");

                entity.Property(e => e.AdjustmentPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PayeeReference).HasMaxLength(100);

                entity.Property(e => e.PeriodFromDate).HasColumnType("datetime");

                entity.Property(e => e.PeriodToDate).HasColumnType("datetime");

                entity.Property(e => e.ReceivedAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionReference)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UnAppliedAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.VerifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.RcmPaymentReconcilations)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PaymentReconcilation_RCM_Organization");
            });

            modelBuilder.Entity<RcmPaymentReconcilationDetail>(entity =>
            {
                entity.HasKey(e => e.PaymentReconcilationDetailId);

                entity.ToTable("RCM_PaymentReconcilationDetail");

                entity.Property(e => e.ApprovedAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ClaimAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.SettleAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.PaymentReconcilation)
                    .WithMany(p => p.RcmPaymentReconcilationDetails)
                    .HasForeignKey(d => d.PaymentReconcilationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PaymentReconcilationDetail_RCM_PaymentReconcilation");

                entity.HasOne(d => d.RcmClaim)
                    .WithMany(p => p.RcmPaymentReconcilationDetails)
                    .HasForeignKey(d => new { d.ClaimId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_PaymentReconcilationDetail_RCM_PaymentReconcilationDetail");
            });

            modelBuilder.Entity<RcmProcessLog>(entity =>
            {
                entity.HasKey(e => e.ProcessLogId);

                entity.ToTable("RCM_ProcessLog");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.ErrorMessage).IsUnicode(false);

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmRadDetailsTemp>(entity =>
            {
                entity.HasKey(e => e.RadDetailId)
                    .HasName("PK__RCM_RadD__9F0BA4474278A683");

                entity.ToTable("RCM_RadDetailsTemp");

                entity.Property(e => e.RadDetailId).HasColumnName("RadDetailID");

                entity.Property(e => e.ClinicalData)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RadiologyResult)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.RadiologyVisitDate).HasColumnType("datetime");

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmReferenceDimension>(entity =>
            {
                entity.HasKey(e => e.DimensionId);

                entity.ToTable("RCM_ReferenceDimension");

                entity.Property(e => e.DimensionId)
                    .ValueGeneratedNever()
                    .HasColumnName("DimensionID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DimentionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DimentionType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmRejectionReason>(entity =>
            {
                entity.HasKey(e => e.RejectionId)
                    .HasName("PK__RCM_Reje__9CACD2B8176049D9");

                entity.ToTable("RCM_RejectionReason");

                entity.Property(e => e.RejectionId).HasColumnName("RejectionID");

                entity.Property(e => e.RejectionReason)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmRemittanceLog>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.ToTable("RCM_RemittanceLog");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifedOn).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmReportDiscripancy>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__RCM_Repo__D5BD4805CB89AD27");

                entity.ToTable("RCM_ReportDiscripancy");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CyclusCompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CyclusGrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiscripancyType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.RcmCompanyName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.VidaCompanyName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.VidaCompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VidaGrossAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<RcmReportRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.ToTable("RCM_ReportRequest");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmReportTemplate>(entity =>
            {
                entity.ToTable("RCM_ReportTemplate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ServiceUrl)
                    .HasMaxLength(500)
                    .HasColumnName("ServiceURL");
            });

            modelBuilder.Entity<RcmReportTemplateBkp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RCM_ReportTemplate_bkp");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ServiceUrl)
                    .HasMaxLength(500)
                    .HasColumnName("ServiceURL");
            });

            modelBuilder.Entity<RcmReportdiscrepancy>(entity =>
            {
                entity.ToTable("RCM_Reportdiscrepancy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Discrepancy)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("discrepancy");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.RcmcompanyName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("RCMCompanyName");

                entity.Property(e => e.RcmcompanyShare)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RCMCompanyShare");

                entity.Property(e => e.RcmcompanyTax)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RCMCompanyTax");

                entity.Property(e => e.RcmdiscountAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RCMDiscountAmount");

                entity.Property(e => e.RcmgrossAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RCMGrossAmount");

                entity.Property(e => e.RcmnetAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RCMNetAmount");

                entity.Property(e => e.RcmpatientShare)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RCMPatientShare");

                entity.Property(e => e.RcmpatientTax)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RCMPatientTax");

                entity.Property(e => e.RefundReturnDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnRefundReferenceNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VidaCompanyName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VidaCompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VidaCompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VidaDiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VidaGrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VidaNetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VidaPatientShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VidaPatientTax).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<RcmReportsDayWise>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__RCM_Repo__D5BD48E5809E581F");

                entity.ToTable("RCM_ReportsDayWise");

                entity.Property(e => e.ReportId).HasColumnName("ReportID");

                entity.Property(e => e.CompanyGroupName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyShareInclusiveOfVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FacilityName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.GrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PatientName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PatientShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PatientShareInclusiveOfVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PatientTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PolicyName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PolicyNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefundReturnDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnRefundReferenceNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalRevenueInclusiveOfVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalVatAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<RcmReportsDayWiseHeader>(entity =>
            {
                entity.ToTable("RCM_ReportsDayWiseHeader");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<RcmRuleEngineErrorLog>(entity =>
            {
                entity.HasKey(e => e.ErrorLogId);

                entity.ToTable("RCM_RuleEngineErrorLog");

                entity.HasIndex(e => new { e.ClaimId, e.OrganizationId, e.IsActive }, "Indx_RCM_RuleEngineErrorLog_1")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.OrganizationId, e.ClaimId, e.ErrorTypeId, e.IsActive }, "Indx_RCM_RuleEngineErrorLog_2")
                    .HasFillFactor(90);

                entity.Property(e => e.ErrorLogId).HasColumnName("ErrorLogID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ErrorLog).IsUnicode(false);

                entity.Property(e => e.ErrorType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorTypeId).HasColumnName("ErrorTypeID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.RuleGroupName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RuleIds)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SeverityName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SourceType)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmServiceCatalog>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("PK_RCM_ServiceCatalog_1");

                entity.ToTable("RCM_ServiceCatalog");

                entity.HasIndex(e => new { e.OrganizationId, e.FacilityGroupId, e.ExternalCode }, "Indx_FGID_ORGID_EC");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.Gender).HasDefaultValueSql("((3))");

                entity.Property(e => e.IsAutoAccepted)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ServiceCatalogCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCatalogName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCatalogNameN)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.ServiceCategoryId).HasColumnName("ServiceCategoryID");

                entity.Property(e => e.ServiceGroupId).HasColumnName("ServiceGroupID");

                entity.Property(e => e.ServiceSubGroupId).HasColumnName("ServiceSubGroupID");

                entity.Property(e => e.TaxRate).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TaxType).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.RcmFacilityGroup)
                    .WithMany(p => p.RcmServiceCatalogs)
                    .HasForeignKey(d => new { d.FacilityGroupId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ServiceCatalog_RCM_FacilityGroup");

                entity.HasOne(d => d.RcmServiceCategory)
                    .WithMany(p => p.RcmServiceCatalogs)
                    .HasForeignKey(d => new { d.ServiceCategoryId, d.FacilityGroupId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ServiceCatalog_RCM_ServiceCategory");

                entity.HasOne(d => d.RcmServiceGroup)
                    .WithMany(p => p.RcmServiceCatalogs)
                    .HasForeignKey(d => new { d.ServiceGroupId, d.FacilityGroupId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ServiceCatalog_RCM_ServiceGroup");

                entity.HasOne(d => d.RcmServiceSubGroup)
                    .WithMany(p => p.RcmServiceCatalogs)
                    .HasForeignKey(d => new { d.ServiceSubGroupId, d.FacilityGroupId, d.OrganizationId })
                    .HasConstraintName("FK_RCM_ServiceCatalog_RCM_ServiceSubGroup");
            });

            modelBuilder.Entity<RcmServiceCategory>(entity =>
            {
                entity.HasKey(e => new { e.ServiceCategoryId, e.FacilityGroupId, e.OrganizationId });

                entity.ToTable("RCM_ServiceCategory");

                entity.Property(e => e.ServiceCategoryId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ServiceCategoryID");

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceCategoryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCategoryName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCategoryNameN)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.RcmFacilityGroup)
                    .WithMany(p => p.RcmServiceCategories)
                    .HasForeignKey(d => new { d.FacilityGroupId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ServiceCategory_RCM_FacilityGroup");
            });

            modelBuilder.Entity<RcmServiceCharge>(entity =>
            {
                entity.HasKey(e => new { e.ServiceChargesId, e.DimensionId, e.ServiceId, e.OrganizationId });

                entity.ToTable("RCM_ServiceCharge");

                entity.Property(e => e.ServiceChargesId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ServiceChargesID");

                entity.Property(e => e.DimensionId).HasColumnName("DimensionID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DimReference)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.RcmServiceCharges)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ServiceCharge_RCM_ReferenceDimension");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.RcmServiceCharges)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ServiceCharge_RCM_ServiceCatalog");
            });

            modelBuilder.Entity<RcmServiceComponent>(entity =>
            {
                entity.HasKey(e => new { e.ServiceComponentId, e.LineItemNo, e.ServiceId, e.OrganizationId });

                entity.ToTable("RCM_ServiceComponent");

                entity.Property(e => e.ServiceComponentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ServiceComponentID");

                entity.Property(e => e.LineItemNo).HasColumnName("lineItemNo");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmServiceComponentCharge>(entity =>
            {
                entity.HasKey(e => new { e.ServiceChargeId, e.DimensionId, e.ServiceComponentId, e.LineitemNo, e.ServiceId, e.OrganizationId });

                entity.ToTable("RCM_ServiceComponentCharge");

                entity.Property(e => e.ServiceChargeId).HasColumnName("ServiceChargeID");

                entity.Property(e => e.DimensionId).HasColumnName("DimensionID");

                entity.Property(e => e.ServiceComponentId).HasColumnName("ServiceComponentID");

                entity.Property(e => e.LineitemNo).HasColumnName("lineitemNo");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DimReference)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.RcmServiceComponent)
                    .WithMany(p => p.RcmServiceComponentCharges)
                    .HasForeignKey(d => new { d.ServiceComponentId, d.LineitemNo, d.ServiceId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ServiceComponentCharge_RCM_ServiceComponent");
            });

            modelBuilder.Entity<RcmServiceDetail>(entity =>
            {
                entity.HasKey(e => e.ServiceDetailId);

                entity.ToTable("RCM_ServiceDetails");

                entity.Property(e => e.ApprovalNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EncounterEndType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EncounterStartType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EncounterType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ObservationRemarks).IsUnicode(false);

                entity.Property(e => e.ParentServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReceivedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceDeductibleAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDeductiblePct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountPct).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ServiceGrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceNetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceQuantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceReferenceIdentity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceReferenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceReimbursementAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.StandardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToothNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionReferenceDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionReferenceNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmServiceGroup>(entity =>
            {
                entity.HasKey(e => new { e.ServiceGroupId, e.FacilityGroupId, e.OrganizationId });

                entity.ToTable("RCM_ServiceGroup");

                entity.HasIndex(e => e.OrganizationId, "Indx_RCM_OrgID");

                entity.Property(e => e.ServiceGroupId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ServiceGroupID");

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.AliasN)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceGroupCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceGroupName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceGroupNameN)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.RcmFacilityGroup)
                    .WithMany(p => p.RcmServiceGroups)
                    .HasForeignKey(d => new { d.FacilityGroupId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_ServiceGroup_RCM_FacilityGroup");
            });

            modelBuilder.Entity<RcmServiceSubGroup>(entity =>
            {
                entity.HasKey(e => new { e.ServiceSubGroupId, e.FacilityGroupId, e.OrganizationId });

                entity.ToTable("RCM_ServiceSubGroup");

                entity.Property(e => e.ServiceSubGroupId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ServiceSubGroupID");

                entity.Property(e => e.FacilityGroupId).HasColumnName("FacilityGroupID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceSubGroupCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceSubGroupName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceSubGroupNameN).HasMaxLength(200);
            });

            modelBuilder.Entity<RcmStandardCode>(entity =>
            {
                entity.HasKey(e => e.StandardCodeId)
                    .HasName("PK__Rcm_Stan__758A43EC58D40873");

                entity.ToTable("Rcm_StandardCode");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.StandardCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StandardCodeDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StandardCodeShortDesc)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmSubmissionError>(entity =>
            {
                entity.ToTable("RCM_SubmissionError");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BatchReferenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorDetails).IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PolicyNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmSubmissionErrorLogDetail>(entity =>
            {
                entity.HasKey(e => e.ErrorLogId);

                entity.ToTable("RCM_SubmissionErrorLogDetails");

                entity.Property(e => e.ErrorLogId).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EditedOn).HasColumnType("datetime");

                entity.Property(e => e.ErrorType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExternalRefrenceId)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ExternalRefrenceID");

                entity.Property(e => e.Isactive).HasColumnName("ISActive");

                entity.Property(e => e.RequestDetails).IsUnicode(false);

                entity.Property(e => e.ResponseDetails).IsUnicode(false);
            });

            modelBuilder.Entity<RcmTaskAssignment>(entity =>
            {
                entity.ToTable("RCM_TaskAssignment");

                entity.HasIndex(e => e.ClaimId, "Indx_RCM_ClaimID");

                entity.HasIndex(e => new { e.UserId, e.ClaimId }, "RCM_TaskAssignment_IDX01");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssignedOn).HasColumnType("datetime");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CompletedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.TaskDetailId).HasColumnName("TaskDetailID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<RcmTaskAssignmentLog>(entity =>
            {
                entity.ToTable("RCM_TaskAssignmentLogs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.TaskId).HasColumnName("TaskID");
            });

            modelBuilder.Entity<RcmTaskManagementDetail>(entity =>
            {
                entity.ToTable("RCM_TaskManagementDetail");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.TaskPercentage).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.RcmTaskManagementDetails)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_TaskManagementDetail_RCM_TaskManagementMaster");
            });

            modelBuilder.Entity<RcmTaskManagementMaster>(entity =>
            {
                entity.HasKey(e => e.TaskId);

                entity.ToTable("RCM_TaskManagementMaster");

                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Mrmanager).HasColumnName("MRManager");

                entity.Property(e => e.Mrsupervisor).HasColumnName("MRSupervisor");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.Property(e => e.TaskName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.Trmanager).HasColumnName("TRManager");

                entity.Property(e => e.Trsupervisor).HasColumnName("TRSupervisor");
            });

            modelBuilder.Entity<RcmTemplateMapping>(entity =>
            {
                entity.HasKey(e => e.TemplateId)
                    .HasName("PK_RCM_TemplateId");

                entity.ToTable("RCM_TemplateMapping");

                entity.Property(e => e.Controll)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ValidationMessage)
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmTransactionMapingValue>(entity =>
            {
                entity.HasKey(e => e.TransactionMapingValuesId);

                entity.ToTable("RCM_TransactionMapingValues");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MappingValue)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RcmTransactionMapping>(entity =>
            {
                entity.HasKey(e => new { e.MappingTypeId, e.TransactionTypeId })
                    .HasName("PK_RCM_TransactionMapping_1");

                entity.ToTable("RCM_TransactionMapping");

                entity.Property(e => e.MappingTypeId).HasColumnName("MappingTypeID");

                entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");

                entity.HasOne(d => d.MappingType)
                    .WithMany(p => p.RcmTransactionMappings)
                    .HasForeignKey(d => d.MappingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RCM_TransactionMapping_RCM_MappingType");
            });

            modelBuilder.Entity<RcmTransactionReferenceDetail>(entity =>
            {
                entity.HasKey(e => e.TransactionReferenceDetailsId)
                    .HasName("PK__RCM_Tran__18ED809BBAB22A65");

                entity.ToTable("RCM_TransactionReferenceDetails");

                entity.Property(e => e.TransactionReferenceDetailsId).HasColumnName("TransactionReferenceDetailsID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.CompanyTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PatientTax).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceCompanyShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceGrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceNetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServicePatientShare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceReferenceNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionReferenceDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionReferenceId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TransactionReferenceID");
            });

            modelBuilder.Entity<RcmVitalSignDetailsTemp>(entity =>
            {
                entity.HasKey(e => e.VitalSignDetailId);

                entity.ToTable("RCM_VitalSignDetailsTemp");

                entity.Property(e => e.BloodPressure)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BodyMassIndex).IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Height).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MainSymptoms).IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PhysicianNotesConditions).IsUnicode(false);

                entity.Property(e => e.ReceivedOn).HasColumnType("datetime");

                entity.Property(e => e.SignificantSigns).IsUnicode(false);

                entity.Property(e => e.Temperature).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VitalSignCreatedon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ReEntityMaster>(entity =>
            {
                entity.HasKey(e => e.EntityId);

                entity.ToTable("RE_EntityMaster");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReEventMaster>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK_RE_EventMaster_EventId");

                entity.ToTable("RE_EventMaster");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<RePayerEventRuleGroup>(entity =>
            {
                entity.HasKey(e => e.PayerEventsRuleGroupId)
                    .HasName("PK_RE_PayerEventsRuleGroup_PayerEventsRuleGroupId");

                entity.ToTable("RE_PayerEventRuleGroup");

                entity.Property(e => e.EntityName).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.RuleGroup)
                    .WithMany(p => p.RePayerEventRuleGroups)
                    .HasForeignKey(d => d.RuleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RE_PayerEventsRuleGroup_RE_RuleGroup_RuleGroupId");

                entity.HasOne(d => d.RcmPayer)
                    .WithMany(p => p.RePayerEventRuleGroups)
                    .HasForeignKey(d => new { d.PayerId, d.OrganizationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RE_PayerEventsRuleGroup_RCM_Payer_PayerID");
            });

            modelBuilder.Entity<ReResultSeverityLevel>(entity =>
            {
                entity.HasKey(e => e.SeverityId)
                    .HasName("PK__RE_Resul__C618A97193A9B95A");

                entity.ToTable("RE_ResultSeverityLevel");

                entity.Property(e => e.SeverityName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ReRule>(entity =>
            {
                entity.HasKey(e => e.RuleId)
                    .HasName("PK_RuleId");

                entity.ToTable("RE_Rules");

                entity.Property(e => e.RuleId).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RuleDescription).HasMaxLength(100);

                entity.Property(e => e.RuleName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.RuleGroup)
                    .WithMany(p => p.ReRules)
                    .HasForeignKey(d => d.RuleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rules_RuleGroupId");
            });

            modelBuilder.Entity<ReRuleExpression>(entity =>
            {
                entity.HasKey(e => e.RuleExpressionId)
                    .HasName("PK_RuleExpressionId");

                entity.ToTable("RE_RuleExpression");

                entity.Property(e => e.BracketFirst)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BracketLast).HasMaxLength(100);

                entity.Property(e => e.Condition).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Expression).IsUnicode(false);

                entity.Property(e => e.Field)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Operator)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Rule)
                    .WithMany(p => p.ReRuleExpressions)
                    .HasForeignKey(d => d.RuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RuleExpression_RuleId");
            });

            modelBuilder.Entity<ReRuleFieldParameter>(entity =>
            {
                entity.HasKey(e => e.RuleFieldId)
                    .HasName("PK_RuleFieldId");

                entity.ToTable("RE_RuleFieldParameter");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.Property(e => e.RuleFieldName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ReRuleFieldParameterGroup>(entity =>
            {
                entity.HasKey(e => e.RuleFieldParamGroupId)
                    .HasName("PK_RuleFieldParamGroupId");

                entity.ToTable("RE_RuleFieldParameterGroup");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.HasOne(d => d.RuleField)
                    .WithMany(p => p.ReRuleFieldParameterGroups)
                    .HasForeignKey(d => d.RuleFieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RuleFieldParamter_RuleFieldId");

                entity.HasOne(d => d.RuleGroup)
                    .WithMany(p => p.ReRuleFieldParameterGroups)
                    .HasForeignKey(d => d.RuleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RuleGroup_RuleGroupId");
            });

            modelBuilder.Entity<ReRuleGroup>(entity =>
            {
                entity.HasKey(e => e.RuleGroupId)
                    .HasName("PK_RuleGroupId");

                entity.ToTable("RE_RuleGroup");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.Property(e => e.RuleGroupName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ReRuleResult>(entity =>
            {
                entity.HasKey(e => e.RuleResultId)
                    .HasName("PK_RuleResultId");

                entity.ToTable("RE_RuleResult");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ResultField)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ResultName).HasMaxLength(500);

                entity.Property(e => e.ResultValue).HasMaxLength(500);

                entity.Property(e => e.SeverityLevelTypeId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Rule)
                    .WithMany(p => p.ReRuleResults)
                    .HasForeignKey(d => d.RuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RuleResult_RuleId");

                entity.HasOne(d => d.Severity)
                    .WithMany(p => p.ReRuleResults)
                    .HasForeignKey(d => d.SeverityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RuleResult_SeverityLevels");
            });

            modelBuilder.Entity<ReRuleUserFunction>(entity =>
            {
                entity.HasKey(e => e.FunctionId)
                    .HasName("PK_FunctionId");

                entity.ToTable("RE_RuleUserFunction");

                entity.HasIndex(e => e.FunctionName, "UQ__RE_RuleU__7A54611B03C23488")
                    .IsUnique();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FunctionName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<RcmClaimNphiesPostTrail>(entity =>
            {
                entity.ToTable("RCM_ClaimNphiesPostTrail");

                entity.HasIndex(e => new { e.OrganizationId, e.ClaimId }, "IX_ClaimNphiesPostTrail_Org_ClaimId");

                entity.Property(e => e.ClaimBundleId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ClaimIdentifier)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PreviousClaimIdentifier).HasMaxLength(255);

                entity.Property(e => e.PrevoiusClaimBundleId).HasMaxLength(255);

                entity.Property(e => e.Rowversion)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("rowversion");
            });

            modelBuilder.Entity<RcmAddResponseBinding>(entity =>
            {
                
                entity.HasKey(e => e.Id)
                  .HasName("PK_RCM_AddResponseBinding");

                entity.ToTable("RCM_AddResponseBinding");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<RcmLabResult>(entity =>
            {
                entity.ToTable("RCM_LabResults");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("ID")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.FacilityId).IsRequired();
                entity.Property(e => e.ClaimId).IsRequired();
                entity.Property(e => e.ServiceCode).HasMaxLength(100);
                entity.Property(e => e.TestId);
                entity.Property(e => e.LabResult).HasMaxLength(500);
                entity.Property(e => e.CreatedBy);
                entity.Property(e => e.CreatedOn);
                entity.Property(e => e.ModifiedBy);
                entity.Property(e => e.ModifiedOn);
                entity.Property(e => e.IsActive);
            });
            modelBuilder.Entity<Claim_Service_Observation>(entity =>
            {
                entity.HasKey(e => new { e.OrganizationID, e.ClaimID, e.ServiceID, e.ServiceLineitemNo, e.LineItemNo });
                entity.ToTable("RCM_ClaimServices_Detail_Observation");

                entity.Property(e => e.ClaimID).HasColumnName("ClaimID");

                entity.Property(e => e.OrganizationID).HasColumnName("OrganizationID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceID).HasColumnName("ServiceID");

                entity.Property(e => e.ServiceLineitemNo).HasColumnName("ServiceLineitemNo");

                entity.Property(e => e.LineItemNo).HasColumnName("LineItemNo");

                entity.Property(e => e.Type).HasColumnName("Type");
                entity.Property(e => e.Code).HasColumnName("Code");
                entity.Property(e => e.Value).HasColumnName("Value");
                entity.Property(e => e.Valuetype).HasColumnName("Valuetype");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.IsActive).HasColumnName("IsActive");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.RCM_ClaimServicesDetail)
                   .WithMany(p => p.RcmClaimServiceObservation)
                   .HasForeignKey(d => new { d.ClaimID, d.OrganizationID, d.ServiceID, d.ServiceLineitemNo })
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_RCM_ClaimServices_Detail_Observation_RCM_Claim_Services");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
