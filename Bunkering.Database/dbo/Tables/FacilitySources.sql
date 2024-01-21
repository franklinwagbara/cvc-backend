CREATE TABLE [dbo].[FacilitySources] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [FacilityId]     INT            NOT NULL,
    [FacilityTypeId] INT            NOT NULL,
    [Name]           NVARCHAR (MAX) NOT NULL,
    [Address]        NVARCHAR (MAX) NOT NULL,
    [LicenseNumber]  NVARCHAR (MAX) NOT NULL,
    [LgaId]          INT            NOT NULL,
    CONSTRAINT [PK_FacilitySources] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FacilitySources_Facilities_FacilityId] FOREIGN KEY ([FacilityId]) REFERENCES [dbo].[Facilities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FacilitySources_LGAs_LgaId] FOREIGN KEY ([LgaId]) REFERENCES [dbo].[LGAs] ([Id]) ON DELETE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_FacilitySources_LgaId]
    ON [dbo].[FacilitySources]([LgaId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FacilitySources_FacilityId]
    ON [dbo].[FacilitySources]([FacilityId] ASC);

