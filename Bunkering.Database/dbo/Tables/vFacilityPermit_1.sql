CREATE TABLE [dbo].[vFacilityPermit] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT            NOT NULL,
    [VesselName]    NVARCHAR (MAX) NOT NULL,
    [VesselType]    NVARCHAR (MAX) NOT NULL,
    [PermitNo]      NVARCHAR (MAX) NOT NULL,
    [IssuedDate]    DATETIME2 (7)  NOT NULL,
    [ExpireDate]    DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_vFacilityPermit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

