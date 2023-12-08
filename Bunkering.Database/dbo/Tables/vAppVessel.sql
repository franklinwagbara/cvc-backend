CREATE TABLE [dbo].[vAppVessel] (
    [id]                INT             IDENTITY (1, 1) NOT NULL,
    [Status]            NVARCHAR (MAX)  NOT NULL,
    [VesselName]        NVARCHAR (MAX)  NOT NULL,
    [Reference]         NVARCHAR (MAX)  NOT NULL,
    [CreatedDate]       DATETIME2 (7)   NOT NULL,
    [SubmittedDate]     DATETIME2 (7)   NOT NULL,
    [ModifiedDate]      DATETIME2 (7)   NOT NULL,
    [IsDeleted]         BIT             NOT NULL,
    [AppTypeName]       NVARCHAR (MAX)  NOT NULL,
    [Email]             NVARCHAR (MAX)  NOT NULL,
    [CompanyName]       NVARCHAR (MAX)  NOT NULL,
    [IsLicensed]        BIT             NOT NULL,
    [VesselTypes]       NVARCHAR (MAX)  NOT NULL,
    [NoOfTanks]         INT             NOT NULL,
    [Capacity]          DECIMAL (18, 2) NOT NULL,
    [ApplicationTypeId] INT             NOT NULL,
    CONSTRAINT [PK_vAppVessel] PRIMARY KEY CLUSTERED ([id] ASC)
);

