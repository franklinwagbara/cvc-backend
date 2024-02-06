CREATE TABLE [dbo].[COQSubmittedDocuments]
(
	[Id]                INT            IDENTITY (1, 1) NOT NULL,
    [CoQId]     INT            NOT NULL,
    [FileId]            INT            NOT NULL,
    [DocId]             INT            NOT NULL,
    [DocSource]         NVARCHAR (MAX) NOT NULL,
    [DocType]           NVARCHAR (MAX) NOT NULL,
    [DocName]           NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_COQSubmittedDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
)
