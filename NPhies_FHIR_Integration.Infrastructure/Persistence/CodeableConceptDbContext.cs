using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NPhies_FHIR_Integration.Domain.CodeableConcept.Models;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Domain.CodeableConcept.Services
{
    /// <summary>
    /// DbContext interface for NPHIES CodeableConcept database
    /// Allows for dependency injection and testing
    /// </summary>
    public interface ICodeableConceptDbContext
    {
      DbSet<CodeSystemEntity> CodeSystems { get; }
        DbSet<ConceptEntity> Concepts { get; }
        DbSet<ValueSetEntity> ValueSets { get; }
   DbSet<ValueSetCodeSystemMapEntity> ValueSetCodeSystemMaps { get; }
      DbSet<ProfileElementEntity> ProfileElements { get; }
        DbSet<ConceptCodeFilterEntity> ConceptCodeFilters { get; }
   DbSet<ValidationRuleEntity> ValidationRules { get; }
        DbSet<NphiesMessageTypeEntity> NphiesMessageTypes { get; }
        DbSet<NphiesMessageRequiredElementEntity> NphiesMessageRequiredElements { get; }

      // NPHIES Phase 1 Entities
      DbSet<NphiesMessageHeaderEntity> NphiesMessageHeaders { get; }
    DbSet<NphiesBundleEntity> NphiesBundles { get; }
        DbSet<BundleEntryEntity> BundleEntries { get; }
        DbSet<NphiesExtensionEntity> NphiesExtensions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// DbContext implementation for NPHIES CodeableConcept database
    /// </summary>
    public class CodeableConceptDbContext : DbContext, ICodeableConceptDbContext
{
        public CodeableConceptDbContext(DbContextOptions<CodeableConceptDbContext> options)
 : base(options)
  {
   }

   public DbSet<CodeSystemEntity> CodeSystems { get; set; }
        public DbSet<ConceptEntity> Concepts { get; set; }
        public DbSet<ValueSetEntity> ValueSets { get; set; }
        public DbSet<ValueSetCodeSystemMapEntity> ValueSetCodeSystemMaps { get; set; }
        public DbSet<ProfileElementEntity> ProfileElements { get; set; }
        public DbSet<ConceptCodeFilterEntity> ConceptCodeFilters { get; set; }
        public DbSet<ValidationRuleEntity> ValidationRules { get; set; }
        public DbSet<NphiesMessageTypeEntity> NphiesMessageTypes { get; set; }
        public DbSet<NphiesMessageRequiredElementEntity> NphiesMessageRequiredElements { get; set; }

        // NPHIES Phase 1 DbSets
        public DbSet<NphiesMessageHeaderEntity> NphiesMessageHeaders { get; set; }
        public DbSet<NphiesBundleEntity> NphiesBundles { get; set; }
        public DbSet<BundleEntryEntity> BundleEntries { get; set; }
        public DbSet<NphiesExtensionEntity> NphiesExtensions { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       base.OnModelCreating(modelBuilder);

         // Configure CodeSystem
            modelBuilder.Entity<CodeSystemEntity>(entity =>
            {
 entity.HasKey(e => e.CodeSystemId);
        entity.Property(e => e.Url).IsRequired();
       entity.HasIndex(e => e.Url).IsUnique();
        entity.HasIndex(e => e.Name);
    entity.HasIndex(e => e.IsActive);
      });

 // Configure Concept
     modelBuilder.Entity<ConceptEntity>(entity =>
            {
           entity.HasKey(e => e.ConceptId);
           entity.Property(e => e.Code).IsRequired();
      entity.HasIndex(e => new { e.CodeSystemId, e.Code }).IsUnique();
entity.HasIndex(e => e.Code);
    entity.HasIndex(e => e.IsActive);
                entity.HasOne(e => e.CodeSystem)
     .WithMany(cs => cs.Concepts)
            .HasForeignKey(e => e.CodeSystemId)
        .OnDelete(DeleteBehavior.Cascade);
    });

            // Configure ValueSet
      modelBuilder.Entity<ValueSetEntity>(entity =>
          {
      entity.HasKey(e => e.ValueSetId);
              entity.Property(e => e.Url).IsRequired();
     entity.HasIndex(e => e.Url).IsUnique();
                entity.HasIndex(e => e.Name);
           entity.HasIndex(e => e.IsActive);
      });

   // Configure ValueSetCodeSystemMap
            modelBuilder.Entity<ValueSetCodeSystemMapEntity>(entity =>
   {
  entity.HasKey(e => e.ValueSetCodeSystemMapId);
           entity.HasIndex(e => new { e.ValueSetId, e.CodeSystemId }).IsUnique();
     entity.HasOne(e => e.ValueSet)
     .WithMany(vs => vs.CodeSystemMappings)
         .HasForeignKey(e => e.ValueSetId)
      .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.CodeSystem)
      .WithMany(cs => cs.ValueSetMappings)
 .HasForeignKey(e => e.CodeSystemId)
      .OnDelete(DeleteBehavior.Cascade);
            });

 // Configure ProfileElement
  modelBuilder.Entity<ProfileElementEntity>(entity =>
    {
                entity.HasKey(e => e.ProfileElementId);
   entity.HasIndex(e => e.Path);
       entity.HasIndex(e => e.MessageType);
entity.HasIndex(e => e.ResourceType);
    entity.HasIndex(e => e.ProfileName);
           entity.HasOne(e => e.ValueSet)
  .WithMany(vs => vs.ProfileElements)
       .HasForeignKey(e => e.ValueSetId)
      .OnDelete(DeleteBehavior.SetNull);
     });

   // Configure ConceptCodeFilter
            modelBuilder.Entity<ConceptCodeFilterEntity>(entity =>
       {
          entity.HasKey(e => e.ConceptCodeFilterId);
      entity.HasIndex(e => new { e.ValueSetId, e.Code });
          entity.HasOne(e => e.ValueSet)
     .WithMany(vs => vs.CodeFilters)
       .HasForeignKey(e => e.ValueSetId)
           .OnDelete(DeleteBehavior.Cascade);
         });

      // Configure ValidationRule
     modelBuilder.Entity<ValidationRuleEntity>(entity =>
          {
     entity.HasKey(e => e.ValidationRuleId);
      entity.Property(e => e.ErrorCode).IsRequired();
            entity.HasIndex(e => e.ErrorCode).IsUnique();
      entity.HasIndex(e => e.FieldPath);
          entity.HasIndex(e => e.RuleType);
     });

            // Configure NphiesMessageType
     modelBuilder.Entity<NphiesMessageTypeEntity>(entity =>
          {
      entity.HasKey(e => e.NphiesMessageTypeId);
        entity.Property(e => e.MessageType).IsRequired();
             entity.HasIndex(e => e.MessageType).IsUnique();
                entity.HasIndex(e => e.FhirResourceType);
     });

         // Configure NphiesMessageRequiredElement
            modelBuilder.Entity<NphiesMessageRequiredElementEntity>(entity =>
  {
         entity.HasKey(e => e.NphiesMessageRequiredElementId);
            entity.HasOne(e => e.NphiesMessageType)
           .WithMany(nmt => nmt.RequiredElements)
        .HasForeignKey(e => e.NphiesMessageTypeId)
           .OnDelete(DeleteBehavior.Cascade);
             entity.HasOne(e => e.ValueSet)
             .WithMany()
     .HasForeignKey(e => e.ValueSetId)
    .OnDelete(DeleteBehavior.SetNull);
  });

            // NPHIES Phase 1 Configuration
         // NphiesMessageHeader configuration
       modelBuilder.Entity<NphiesMessageHeaderEntity>(entity =>
      {
   entity.HasKey(e => e.Id);
         entity.HasOne(e => e.ProviderOrganization)
         .WithMany()
          .HasForeignKey(e => e.ProviderOrganizationId)
  .OnDelete(DeleteBehavior.Restrict);
    entity.HasIndex(e => e.MessageId).IsUnique();
              entity.HasIndex(e => e.EventCode);
 entity.HasIndex(e => e.CreatedAt);
     });

            // NphiesBundle configuration
            modelBuilder.Entity<NphiesBundleEntity>(entity =>
            {
 entity.HasKey(e => e.Id);
       entity.HasOne(e => e.MessageHeader)
      .WithMany()
     .HasForeignKey(e => e.MessageHeaderId)
           .OnDelete(DeleteBehavior.SetNull);
     entity.HasIndex(e => e.BundleId).IsUnique();
              entity.HasIndex(e => e.CreatedAt);
            });

            // BundleEntry configuration
            modelBuilder.Entity<BundleEntryEntity>(entity =>
     {
 entity.HasKey(e => e.Id);
           entity.HasOne(e => e.Bundle)
          .WithMany(b => b.Entries)
 .HasForeignKey(e => e.BundleId)
          .OnDelete(DeleteBehavior.Cascade);
      entity.HasIndex(e => e.BundleId);
        entity.HasIndex(e => e.ResourceType);
            });

            // NphiesExtension configuration
            modelBuilder.Entity<NphiesExtensionEntity>(entity =>
  {
       entity.HasKey(e => e.Id);
         entity.HasIndex(e => e.ResourceType);
            entity.HasIndex(e => e.ResourceId);
        entity.HasIndex(e => e.ExtensionType);
            });
  }
    }
}
