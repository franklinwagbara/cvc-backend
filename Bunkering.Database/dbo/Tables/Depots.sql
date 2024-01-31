CREATE TABLE [dbo].[Depots] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX)  NOT NULL,
    [IsDeleted]    BIT             NOT NULL,
    [DeletedAt]    DATETIME2 (7)   NULL,
    [DeletedBy]    NVARCHAR (MAX)  NULL,
    [Capacity]     DECIMAL (18, 2) DEFAULT ((0.0)) NOT NULL,
    [State]        NVARCHAR (MAX)  DEFAULT (N'') NOT NULL,
    [MarketerName] NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Depots] PRIMARY KEY CLUSTERED ([Id] ASC)
);

