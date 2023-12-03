CREATE TABLE [dbo].[ApplicationHistories] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId]   INT            NOT NULL,
    [TriggeredBy]     NVARCHAR (MAX) NOT NULL,
    [TriggeredByRole] NVARCHAR (MAX) NOT NULL,
    [Action]          NVARCHAR (MAX) NOT NULL,
    [TargetedTo]      NVARCHAR (MAX) NOT NULL,
    [TargetRole]      NVARCHAR (MAX) NOT NULL,
    [Date]            DATETIME2 (7)  NOT NULL,
    [Comment]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ApplicationHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ApplicationHistories_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Applications] ([Id]) ON DELETE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_ApplicationHistories_ApplicationId]
    ON [dbo].[ApplicationHistories]([ApplicationId] ASC);

