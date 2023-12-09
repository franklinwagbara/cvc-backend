CREATE TABLE [dbo].[vAppUsers] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Email]    NVARCHAR (MAX) NOT NULL,
    [Location] NVARCHAR (MAX) NOT NULL,
    [Office]   NVARCHAR (MAX) NOT NULL,
    [StateId]  INT            NOT NULL,
    [role]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_vAppUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

