CREATE TABLE [dbo].[PlantFieldOfficers] (
    [ID]        INT              IDENTITY (1, 1) NOT NULL,
    [PlantID]   INT              NOT NULL,
    [OfficerID] UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted] BIT              NOT NULL,
    CONSTRAINT [PK_PlantFieldOfficers] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PlantFieldOfficers_Plants_PlantID] FOREIGN KEY ([PlantID]) REFERENCES [dbo].[Plants] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_PlantFieldOfficers_PlantID]
    ON [dbo].[PlantFieldOfficers]([PlantID] ASC);

