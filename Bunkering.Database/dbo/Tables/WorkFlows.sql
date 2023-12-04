CREATE TABLE [dbo].[WorkFlows] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [VesselTypeId]      INT            NOT NULL,
    [TriggeredByRole]   NVARCHAR (MAX) NOT NULL,
    [Action]            NVARCHAR (MAX) NOT NULL,
    [TargetRole]        NVARCHAR (MAX) NOT NULL,
    [Rate]              NVARCHAR (MAX) NOT NULL,
    [Status]            NVARCHAR (MAX) NOT NULL,
    [ApplicationTypeId] INT            NULL,
    [IsArchived]        BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [FromLocationId]    INT            NULL,
    [OfficeId]          INT            NULL,
    [ToLocationId]      INT            NULL,
    CONSTRAINT [PK_WorkFlows] PRIMARY KEY CLUSTERED ([Id] ASC)
);

















