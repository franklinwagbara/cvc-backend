CREATE TABLE [dbo].[Depots] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL DEFAULT null,
    [StateId] INT NOT NULL DEFAULT 0, 
    [Capacity] DECIMAL NOT NULL DEFAULT 0, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [DeletedAt] DATETIME NULL , 
    [DeletedBy] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Depots] PRIMARY KEY CLUSTERED ([Id] ASC)
);