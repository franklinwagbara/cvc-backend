CREATE TABLE [dbo].[EmailConfigurations] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (MAX) NOT NULL,
    [Email]    NVARCHAR (MAX) NOT NULL,
    [IsActive] BIT            NOT NULL,
    CONSTRAINT [PK_EmailConfigurations] PRIMARY KEY CLUSTERED ([Id] ASC)
);

