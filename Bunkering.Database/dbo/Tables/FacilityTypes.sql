CREATE TABLE [dbo].[FacilityTypes] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    [Code] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_FacilityTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

