CREATE TABLE [dbo].[Plants] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (MAX) NOT NULL,
    [Company]       NVARCHAR (MAX) NOT NULL,
    [Email]         NVARCHAR (MAX) NOT NULL,
    [State]         NVARCHAR (MAX) NOT NULL,
    [ElpsPlantId]   BIGINT         NULL,
    [CompanyElpsId] BIGINT         NULL,
    [PlantType]     INT            NOT NULL,
    [IsDeleted]     BIT            NOT NULL,
    CONSTRAINT [PK_Plants] PRIMARY KEY CLUSTERED ([Id] ASC)
);

