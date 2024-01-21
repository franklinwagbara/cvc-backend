CREATE TABLE [dbo].[JettyFieldOfficers] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [JettyId]   INT           NOT NULL,
    [OfficerId] VARCHAR (MAX) NOT NULL,
    [IsDeleted] BIT           CONSTRAINT [DF_JettyFieldOfficers_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JettyFieldOfficers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

