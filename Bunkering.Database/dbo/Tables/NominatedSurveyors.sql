CREATE TABLE [dbo].[NominatedSurveyors] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX)  NOT NULL,
    [IsDeleted]       BIT             NOT NULL,
    [DeletedAt]       DATETIME2 (7)   NULL,
    [DeletedBy]       NVARCHAR (MAX)  NULL,
    [NominatedVolume] DECIMAL (18, 2) DEFAULT ((0.0)) NOT NULL,
    [Email]           NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_NominatedSurveyors] PRIMARY KEY CLUSTERED ([Id] ASC)
);





