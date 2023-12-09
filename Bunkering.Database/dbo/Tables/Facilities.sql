CREATE TABLE [dbo].[Facilities] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [CompanyId]    INT             NOT NULL,
    [ElpsId]       INT             NOT NULL,
    [VesselTypeId] INT             NOT NULL,
    [Name]         NVARCHAR (MAX)  NOT NULL,
    [IMONumber]    NVARCHAR (MAX)  NOT NULL,
    [CallSIgn]     NVARCHAR (MAX)  NULL,
    [Flag]         NVARCHAR (MAX)  NULL,
    [YearOfBuild]  INT             NULL,
    [PlaceOfBuild] NVARCHAR (MAX)  NULL,
    [IsLicensed]   BIT             NOT NULL,
    [DeadWeight]   DECIMAL (18, 2) NULL,
    [Capacity]     DECIMAL (18, 2) NULL,
    [Operator]     NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Facilities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Facilities_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Facilities_VesselTypes_VesselTypeId] FOREIGN KEY ([VesselTypeId]) REFERENCES [dbo].[VesselTypes] ([Id]) ON DELETE CASCADE
);






















GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_Facilities_CompanyId]
    ON [dbo].[Facilities]([CompanyId] ASC);


GO



GO
CREATE NONCLUSTERED INDEX [IX_Facilities_VesselTypeId]
    ON [dbo].[Facilities]([VesselTypeId] ASC);

