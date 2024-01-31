CREATE TABLE [dbo].[Messages] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [IsRead]        BIT            NOT NULL,
    [ApplicationId] INT            NULL,
    [Content]       NVARCHAR (MAX) NOT NULL,
    [Date]          DATETIME2 (7)  NOT NULL,
    [Subject]       NVARCHAR (MAX) NOT NULL,
    [UserId]        NVARCHAR (MAX) NOT NULL,
    [COQId]         INT            DEFAULT ((0)) NULL,
    [IsCOQ]         BIT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Applications] ([Id])
);
















GO
CREATE NONCLUSTERED INDEX [IX_Messages_ApplicationId]
    ON [dbo].[Messages]([ApplicationId] ASC);

