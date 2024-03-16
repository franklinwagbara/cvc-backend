CREATE TABLE [dbo].[Jetties] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [IsDeleted] BIT            NOT NULL,
    [DeletedAt] DATETIME2 (7)  NULL,
    [DeletedBy] NVARCHAR (MAX) NULL,
    [StateId]   INT            NULL,
    [Location]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Jetties] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Jetties_States_Stateid] FOREIGN KEY ([StateId]) REFERENCES [dbo].[States] ([Id])
);





