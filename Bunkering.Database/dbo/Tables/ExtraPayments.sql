CREATE TABLE [dbo].[ExtraPayments] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Reference] NVARCHAR (MAX) NOT NULL,
    [AddedDate] DATETIME2 (7)  NOT NULL,
    [AddedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ExtraPayments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

