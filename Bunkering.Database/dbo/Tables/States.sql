CREATE TABLE [dbo].[States] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [CountryId] INT            NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [Code]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_States_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([Id]) ON DELETE CASCADE
);












GO
CREATE NONCLUSTERED INDEX [IX_States_CountryId]
    ON [dbo].[States]([CountryId] ASC);

