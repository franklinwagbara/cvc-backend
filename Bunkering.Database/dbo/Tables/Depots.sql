CREATE TABLE [dbo].[Depots] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    [State] NVARCHAR(50) NOT NULL, 
    [Capacity] DECIMAL NOT NULL, 
    [IsDeleted] BIT NOT NULL, 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Depots] PRIMARY KEY CLUSTERED ([Id] ASC)
);



