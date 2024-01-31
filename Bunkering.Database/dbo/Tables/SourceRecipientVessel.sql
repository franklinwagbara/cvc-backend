CREATE TABLE [dbo].[SourceRecipientVessel] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [SourceVesselId]      INT NOT NULL,
    [DestinationVesselId] INT NOT NULL,
    [IsDeleted]           BIT CONSTRAINT [DF_SourceRecipientVessel_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SourceRecipientVessel] PRIMARY KEY CLUSTERED ([Id] ASC)
);

