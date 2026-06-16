-- ============================================================================
-- NPHIES RCM Phase 1 Database Migration
-- Creates tables for: MessageHeader, Bundle, BundleEntry, Extensions
-- ============================================================================

-- Create NphiesMessageHeader table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NphiesMessageHeader')
BEGIN
    CREATE TABLE [NphiesMessageHeader] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [MessageId] NVARCHAR(50) NOT NULL UNIQUE,
        [EventCode] NVARCHAR(100) NOT NULL,
        [TimeSent] DATETIME2 NOT NULL,
        [MessageVersion] NVARCHAR(20),
        [ProviderOrganizationId] INT NOT NULL,
     [PayerIdentifier] NVARCHAR(100) NOT NULL,
        [PrimaryResourceType] NVARCHAR(100) NOT NULL,
        [PrimaryResourceId] NVARCHAR(100) NOT NULL,
        [Reason] NVARCHAR(500),
        [ProcessingStatus] NVARCHAR(50),
     [ResponseMessageId] NVARCHAR(50),
        [ProcessingResult] NVARCHAR(500),
        [MetadataJson] NVARCHAR(MAX),
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100),
 [UpdatedAt] DATETIME2,
        [UpdatedBy] NVARCHAR(100),
        CONSTRAINT [FK_NphiesMessageHeader_Organization] FOREIGN KEY ([ProviderOrganizationId])
            REFERENCES [Organization] ([Id]) ON DELETE RESTRICT
    );
    
    CREATE INDEX [IDX_MessageHeader_MessageId] ON [NphiesMessageHeader]([MessageId]);
    CREATE INDEX [IDX_MessageHeader_EventCode] ON [NphiesMessageHeader]([EventCode]);
    CREATE INDEX [IDX_MessageHeader_CreatedAt] ON [NphiesMessageHeader]([CreatedAt]);
    
    PRINT '? Table NphiesMessageHeader created';
END
ELSE
BEGIN
    PRINT '??  Table NphiesMessageHeader already exists';
END;

-- Create NphiesBundle table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NphiesBundle')
BEGIN
    CREATE TABLE [NphiesBundle] (
     [Id] INT PRIMARY KEY IDENTITY(1,1),
        [BundleId] NVARCHAR(50) NOT NULL UNIQUE,
        [BundleType] NVARCHAR(50) NOT NULL DEFAULT 'message',
        [Timestamp] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  [MessageHeaderId] INT,
        [TotalEntries] INT,
        [ProcessingStatus] NVARCHAR(50),
        [HasErrors] BIT DEFAULT 0,
        [ErrorMessage] NVARCHAR(500),
        [BundleJson] NVARCHAR(MAX),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100),
  CONSTRAINT [FK_NphiesBundle_MessageHeader] FOREIGN KEY ([MessageHeaderId])
   REFERENCES [NphiesMessageHeader] ([Id]) ON DELETE SET NULL
    );
    
    CREATE INDEX [IDX_Bundle_BundleId] ON [NphiesBundle]([BundleId]);
    CREATE INDEX [IDX_Bundle_MessageHeaderId] ON [NphiesBundle]([MessageHeaderId]);
    CREATE INDEX [IDX_Bundle_CreatedAt] ON [NphiesBundle]([CreatedAt]);
    
    PRINT '? Table NphiesBundle created';
END
ELSE
BEGIN
    PRINT '??  Table NphiesBundle already exists';
END;

-- Create BundleEntry table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BundleEntry')
BEGIN
    CREATE TABLE [BundleEntry] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
     [BundleId] INT NOT NULL,
      [FullUrl] NVARCHAR(200),
        [ResourceType] NVARCHAR(100) NOT NULL,
  [ResourceId] NVARCHAR(100) NOT NULL,
        [ResourceJson] NVARCHAR(MAX),
        [SequenceNumber] INT,
 [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [FK_BundleEntry_Bundle] FOREIGN KEY ([BundleId])
            REFERENCES [NphiesBundle] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IDX_BundleEntry_BundleId] ON [BundleEntry]([BundleId]);
    CREATE INDEX [IDX_BundleEntry_ResourceType] ON [BundleEntry]([ResourceType]);
    
    PRINT '? Table BundleEntry created';
END
ELSE
BEGIN
    PRINT '??  Table BundleEntry already exists';
END;

-- Create NphiesExtension table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NphiesExtension')
BEGIN
    CREATE TABLE [NphiesExtension] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
      [ExtensionUrl] NVARCHAR(300) NOT NULL,
  [ExtensionType] NVARCHAR(100) NOT NULL,
        [ResourceType] NVARCHAR(100) NOT NULL,
        [ResourceId] NVARCHAR(100) NOT NULL,
        [ValueType] NVARCHAR(50) NOT NULL,
[Value] NVARCHAR(MAX) NOT NULL,
        [Context] NVARCHAR(500),
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  [CreatedBy] NVARCHAR(100),
        [UpdatedAt] DATETIME2,
        [UpdatedBy] NVARCHAR(100)
    );
    
    CREATE INDEX [IDX_Extension_ResourceType] ON [NphiesExtension]([ResourceType]);
    CREATE INDEX [IDX_Extension_ResourceId] ON [NphiesExtension]([ResourceId]);
    CREATE INDEX [IDX_Extension_ExtensionType] ON [NphiesExtension]([ExtensionType]);
    
    PRINT '? Table NphiesExtension created';
END
ELSE
BEGIN
    PRINT '??Table NphiesExtension already exists';
END;

-- Display summary
PRINT '========================================';
PRINT '? NPHIES Phase 1 Migration Complete';
PRINT '========================================';
PRINT 'New Tables Created:';
PRINT '  1. NphiesMessageHeader - Message envelope';
PRINT '  2. NphiesBundle - FHIR Bundle container';
PRINT '  3. BundleEntry - Individual resources in bundle';
PRINT '  4. NphiesExtension - NPHIES-specific extensions';
PRINT '========================================';
