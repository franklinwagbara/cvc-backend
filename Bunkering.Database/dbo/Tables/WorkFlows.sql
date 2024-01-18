CREATE TABLE [dbo].[WorkFlows] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [VesselTypeId]      INT            NOT NULL,
    [FromLocationId]    INT            NULL,
    [ToLocationId]      INT            NULL,
    [OfficeId]          INT            NULL,
    [ApplicationTypeId] INT            NULL,
    [TriggeredByRole]   NVARCHAR (MAX) NOT NULL,
    [Action]            NVARCHAR (MAX) NOT NULL,
    [TargetRole]        NVARCHAR (MAX) NOT NULL,
    [Rate]              NVARCHAR (MAX) NOT NULL,
    [Status]            NVARCHAR (MAX) NOT NULL,
    [IsArchived]        BIT            NOT NULL,
    CONSTRAINT [PK_WorkFlows] PRIMARY KEY CLUSTERED ([Id] ASC)
);

