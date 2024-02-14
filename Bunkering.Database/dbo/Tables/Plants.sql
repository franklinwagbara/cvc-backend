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
    [IsDefaulter]   BIT            CONSTRAINT [DF_Plants_IsDefaulter] DEFAULT ((0)) NOT NULL,
    [IsCleared]     BIT            CONSTRAINT [DF_Plants_IsCleared] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Plants] PRIMARY KEY CLUSTERED ([Id] ASC)
);





