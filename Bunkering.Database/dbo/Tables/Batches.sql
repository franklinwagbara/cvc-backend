CREATE TABLE [dbo].[Batches] (
    [BatchId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [DeletedAt] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Batches] PRIMARY KEY CLUSTERED ([BatchId] ASC)
);

