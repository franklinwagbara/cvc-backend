CREATE TABLE [dbo].[PPCOQSubmittedDocuments]
(
	[Id]                INT            IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQId]     INT            NOT NULL,
    [FileId]            INT            NOT NULL,
    [DocId]             INT            NOT NULL,
    [DocSource]         NVARCHAR (MAX) NOT NULL,
    [DocType]           NVARCHAR (MAX) NOT NULL,
    [DocName]           NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PPCOQSubmittedDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
)
