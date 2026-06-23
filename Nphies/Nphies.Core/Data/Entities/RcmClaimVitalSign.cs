using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimVitalSign
    {
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public string BloodPressure { get; set; }
        public double? Pulse { get; set; }
        public double? Temperature { get; set; }
        public int? Respiration { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string EmergencyIndicator { get; set; }
        public string RightEyeSphereDistance { get; set; }
        public string RightEyeSphereNear { get; set; }
        public string RightEyeCylinderDistance { get; set; }
        public string RightEyeCylinderNear { get; set; }
        public string RightEyeAxisDistance { get; set; }
        public string RightEyeAxisNear { get; set; }
        public string RightEyePrismDistance { get; set; }
        public string RightEyePrismNear { get; set; }
        public string RightEyeVadistance { get; set; }
        public string RightEyeVanear { get; set; }
        public string RightEyePddistance { get; set; }
        public string RightEyePdnear { get; set; }
        public string RightEyeBifocal { get; set; }
        public string LeftEyeSphereDistance { get; set; }
        public string LeftEyeSphereNear { get; set; }
        public string LeftEyeCylinderDistance { get; set; }
        public string LeftEyeCylinderNear { get; set; }
        public string LeftEyeAxisDistance { get; set; }
        public string LeftEyeAxisNear { get; set; }
        public string LeftEyePrismDistance { get; set; }
        public string LeftEyePrismNear { get; set; }
        public string LeftEyeVadistance { get; set; }
        public string LeftEyeVanear { get; set; }
        public string LeftEyePddistance { get; set; }
        public string LeftEyePdnear { get; set; }
        public string LeftEyeBifocal { get; set; }
        public string Vertex { get; set; }
        public string MainSymptoms { get; set; }
        public string SignificantSigns { get; set; }
        public string PhysicianNotesConditions { get; set; }
        public DateTime? VitalSignCreatedOn { get; set; }
        public double? BodyMassIndex { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string RespiratoryRate { get; set; } = string.Empty;
        public string OxygenSaturation { get; set; } = string.Empty;
        public string InvestigationResult { get; set; } = string.Empty;
        public string TreatmentPlan { get; set; } = string.Empty;
        public string PatientHistory { get; set; } = string.Empty;
        public string PhysicalExamination { get; set; } = string.Empty;
        public string HistoryOfPresentIllness { get; set; } = string.Empty;
        public virtual RcmClaim RcmClaim { get; set; }
    }
}
