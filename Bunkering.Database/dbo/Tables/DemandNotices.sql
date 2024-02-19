CREATE TABLE [dbo].[DemandNotices] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Reference]   NVARCHAR (MAX) NULL,
    [AddedDate]   DATETIME2 (7)  NOT NULL,
    [AddedBy]     NVARCHAR (MAX) NOT NULL,
    [DebitNoteId] INT            NOT NULL,
    [Amount]      FLOAT (53)     NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Paid]        BIT            NOT NULL,
    CONSTRAINT [PK_DemandNotices] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DemandNotices_Payments] FOREIGN KEY ([DebitNoteId]) REFERENCES [dbo].[Payments] ([Id])
);





