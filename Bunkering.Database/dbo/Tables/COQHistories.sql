CREATE TABLE [dbo].[COQHistories] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [COQId]           INT            NOT NULL,
    [TriggeredBy]     NVARCHAR (MAX) NOT NULL,
    [TriggeredByRole] NVARCHAR (MAX) NOT NULL,
    [Action]          NVARCHAR (MAX) NOT NULL,
    [TargetedTo]      NVARCHAR (MAX) NOT NULL,
    [TargetRole]      NVARCHAR (MAX) NOT NULL,
    [Date]            DATETIME2 (7)  NOT NULL,
    [Comment]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_COQHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_COQHistories_CoQs_COQId] FOREIGN KEY ([COQId]) REFERENCES [dbo].[CoQs] ([Id]) ON DELETE CASCADE
);




