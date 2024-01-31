CREATE TABLE [dbo].[VesselDischargeClearances] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [AppId]             INT            NOT NULL,
    [DepotId]           INT            NOT NULL,
    [DischargeId]       NVARCHAR (MAX) NULL,
    [VesselName]        NVARCHAR (MAX) NOT NULL,
    [VesselPort]        NVARCHAR (MAX) NOT NULL,
    [Product]           NVARCHAR (MAX) NOT NULL,
    [Density]           FLOAT (53)     NOT NULL,
    [RON]               NVARCHAR (MAX) NOT NULL,
    [FlashPoint]        FLOAT (53)     NOT NULL,
    [FinalBoilingPoint] FLOAT (53)     NOT NULL,
    [Color]             NVARCHAR (MAX) NOT NULL,
    [Odour]             NVARCHAR (MAX) NOT NULL,
    [Oxygenate]         NVARCHAR (MAX) NOT NULL,
    [Others]            NVARCHAR (MAX) NOT NULL,
    [Comment]           NVARCHAR (MAX) NOT NULL,
    [IsAllowed]         BIT            NOT NULL,
    CONSTRAINT [PK_VesselDischargeClearances] PRIMARY KEY CLUSTERED ([Id] ASC)
);

