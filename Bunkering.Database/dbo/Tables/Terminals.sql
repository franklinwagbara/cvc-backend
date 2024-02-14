CREATE TABLE [dbo].[Terminals] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [CreatedBy]   VARCHAR (MAX) NOT NULL,
    [CreatedDate] DATETIME      CONSTRAINT [DF_Terminals_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]   BIT           CONSTRAINT [DF_Terminals_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Terminals] PRIMARY KEY CLUSTERED ([Id] ASC)
);

