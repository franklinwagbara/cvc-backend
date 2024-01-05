CREATE TABLE [dbo].[ApplicationSurveyors] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [NominatedSurveyorId] INT             NOT NULL,
    [ApplicationId]       INT             NOT NULL,
    [Volume]              DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_ApplicationSurveyors] PRIMARY KEY CLUSTERED ([Id] ASC)
);



