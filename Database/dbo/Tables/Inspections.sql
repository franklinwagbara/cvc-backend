CREATE TABLE [dbo].[Inspections] (
    [Id]                                   INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId]                        INT            NOT NULL,
    [ScheduledBy]                          NVARCHAR (MAX) NOT NULL,
    [NominatedStaffId]                     NVARCHAR (MAX) NULL,
    [IndicationOfSImilarFacilityWithin2km] NVARCHAR (MAX) NOT NULL,
    [SiteDrainage]                         NVARCHAR (MAX) NOT NULL,
    [SietJettyTopographicSurvey]           NVARCHAR (MAX) NOT NULL,
    [InspectionDocument]                   NVARCHAR (MAX) NOT NULL,
    [Comment]                              NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Inspections] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Inspections_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Applications] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Inspections_ApplicationId]
    ON [dbo].[Inspections]([ApplicationId] ASC);

