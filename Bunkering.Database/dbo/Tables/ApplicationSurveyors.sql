CREATE TABLE [dbo].[ApplicationSurveyors] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [NominatedSurveyorId] INT             NOT NULL,
    [ApplicationId]       INT             NOT NULL,
    [Volume]              DECIMAL (18, 2) NOT NULL,
    [CreatedAt]           DATETIME2 (7)   DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    CONSTRAINT [PK_ApplicationSurveyors] PRIMARY KEY CLUSTERED ([Id] ASC)
);





