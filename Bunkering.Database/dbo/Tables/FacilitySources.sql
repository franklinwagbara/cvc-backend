CREATE TABLE [dbo].[FacilitySources] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [FacilityTypeId] INT            NOT NULL,
    [FacilityId]     INT            NOT NULL,
    [Name]           NVARCHAR (MAX) NOT NULL,
    [Address]        NVARCHAR (MAX) NOT NULL,
    [LicenseNumber]  NVARCHAR (MAX) NOT NULL,
    [LgaId]          INT            NOT NULL,
    CONSTRAINT [PK_FacilitySources] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FacilitySources_FacilitySources] FOREIGN KEY ([Id]) REFERENCES [dbo].[FacilitySources] ([Id]),
    CONSTRAINT [FK_FacilitySources_Lga_LgaId] FOREIGN KEY ([LgaId]) REFERENCES [dbo].[LGAs] ([Id])
);







