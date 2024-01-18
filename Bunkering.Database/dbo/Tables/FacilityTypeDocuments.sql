﻿CREATE TABLE [dbo].[FacilityTypeDocuments] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [DocumentTypeId]    INT            NOT NULL,
    [VesselTypeId]      INT            NOT NULL,
    [ApplicationTypeId] INT            NOT NULL,
    [IsFADDoc]          BIT            NOT NULL,
    [Name]              NVARCHAR (MAX) NOT NULL,
    [DocType]           NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_FacilityTypeDocuments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FacilityTypeDocuments_ApplicationTypes_ApplicationTypeId] FOREIGN KEY ([ApplicationTypeId]) REFERENCES [dbo].[ApplicationTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FacilityTypeDocuments_VesselTypes_VesselTypeId] FOREIGN KEY ([VesselTypeId]) REFERENCES [dbo].[VesselTypes] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FacilityTypeDocuments_ApplicationTypeId]
    ON [dbo].[FacilityTypeDocuments]([ApplicationTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FacilityTypeDocuments_VesselTypeId]
    ON [dbo].[FacilityTypeDocuments]([VesselTypeId] ASC);

