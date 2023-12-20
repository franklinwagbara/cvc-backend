CREATE TABLE [dbo].[Jetty] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [IsDeleted] BIT            NOT NULL,
    [DeletedAt] DATETIME       NULL,
    [DeletedBy] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Jetty] PRIMARY KEY CLUSTERED ([Id] ASC)
);

