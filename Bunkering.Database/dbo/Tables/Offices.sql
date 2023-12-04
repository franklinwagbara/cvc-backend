CREATE TABLE [dbo].[Offices] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
    [StateId] INT           NOT NULL,
    CONSTRAINT [PK_Offices] PRIMARY KEY CLUSTERED ([Id] ASC)
);



