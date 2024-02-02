CREATE TABLE [dbo].[Depots] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) DEFAULT (NULL) NOT NULL,
    [StateId]      INT            DEFAULT ((0)) NOT NULL,
    [Capacity]     DECIMAL (18)   DEFAULT ((0)) NOT NULL,
    [IsDeleted]    BIT            DEFAULT ((0)) NOT NULL,
    [DeletedAt]    DATETIME       NULL,
    [DeletedBy]    NVARCHAR (MAX) NULL,
    [MarketerName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Depots] PRIMARY KEY CLUSTERED ([Id] ASC)
);



