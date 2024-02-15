CREATE TABLE [dbo].[CoQs] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [AppId]                    INT            NULL,
    [DateOfVesselArrival]      DATETIME2 (7)  NOT NULL,
    [DateOfVesselUllage]       DATETIME2 (7)  NOT NULL,
    [DateOfSTAfterDischarge]   DATETIME2 (7)  NOT NULL,
    [MT_VAC]                   FLOAT (53)     NOT NULL,
    [MT_AIR]                   FLOAT (53)     NOT NULL,
    [GOV]                      FLOAT (53)     NOT NULL,
    [GSV]                      FLOAT (53)     NOT NULL,
    [DepotPrice]               FLOAT (53)     NOT NULL,
    [DateCreated]              DATETIME2 (7)  NOT NULL,
    [DateModified]             DATETIME2 (7)  NULL,
    [CreatedBy]                NVARCHAR (MAX) NOT NULL,
    [Status]                   NVARCHAR (50)  NULL,
    [IsDeleted]                BIT            NULL,
    [Reference]                NVARCHAR (100) NULL,
    [CurrentDeskId]            NVARCHAR (200) NULL,
    [FADApproved]              BIT            NULL,
    [SubmittedDate]            DATETIME2 (7)  NULL,
    [ArrivalShipFigure]        FLOAT (53)     NOT NULL,
    [DischargeShipFigure]      FLOAT (53)     NOT NULL,
    [PlantId]                  INT            NOT NULL,
    [QuauntityReflectedOnBill] FLOAT (53)     NOT NULL,
    [NameConsignee]            NVARCHAR (MAX) NULL,
    [ProductId]                INT            NULL,
    [GrandTotalWeightKg]       FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [ShoreFigureMTAirGas]      FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    CONSTRAINT [PK_CoQs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CoQs_Applications_AppId] FOREIGN KEY ([AppId]) REFERENCES [dbo].[Applications] ([Id]),
    CONSTRAINT [FK_CoQs_Plants_PlantId] FOREIGN KEY ([PlantId]) REFERENCES [dbo].[Plants] ([Id]) ON DELETE CASCADE
);




















GO



GO
CREATE NONCLUSTERED INDEX [IX_CoQs_AppId]
    ON [dbo].[CoQs]([AppId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CoQs_PlantId]
    ON [dbo].[CoQs]([PlantId] ASC);

