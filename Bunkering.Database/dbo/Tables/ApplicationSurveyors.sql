CREATE TABLE [dbo].[ApplicationSurveyors] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [NominatedSurveyorId] INT NOT NULL,
    [ApplicationId]       INT NOT NULL,
    [Volume]              INT NOT NULL,
    CONSTRAINT [PK_ApplicationSurveyors] PRIMARY KEY CLUSTERED ([Id] ASC)
);

