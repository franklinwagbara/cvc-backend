CREATE TABLE [dbo].[OperatingFacilities] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NOT NULL,
    [CompanyEmail] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_[OperatingFacilities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

