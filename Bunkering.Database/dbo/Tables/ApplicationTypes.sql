CREATE TABLE [dbo].[ApplicationTypes] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ApplicationTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

