CREATE TABLE [dbo].[CoQs] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [AppId]                    INT            NULL,
    [DepotId]                  INT            NOT NULL,
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
    [Status]                   NVARCHAR (50)  DEFAULT (NULL) NULL,
    [IsDeleted]                BIT            DEFAULT ((0)) NULL,
    [Reference]                NVARCHAR (100) DEFAULT (NULL) NULL,
    [CurrentDeskId]            NVARCHAR (200) DEFAULT (NULL) NULL,
    [FADApproved]              BIT            DEFAULT ((0)) NULL,
    [SubmittedDate]            DATETIME2 (7)  DEFAULT (NULL) NULL,
    [ArrivalShipFigure]        FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [DischargeShipFigure]      FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [PlantId]                  INT            DEFAULT ((0)) NOT NULL,
    [QuauntityReflectedOnBill] FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [NameConsignee]            NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    CONSTRAINT [PK_CoQs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CoQs_Applications_AppId] FOREIGN KEY ([AppId]) REFERENCES [dbo].[Applications] ([Id]),
    CONSTRAINT [FK_CoQs_Depots_DepotId] FOREIGN KEY ([DepotId]) REFERENCES [dbo].[Depots] ([Id]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_CoQs_DepotId]
    ON [dbo].[CoQs]([DepotId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CoQs_AppId]
    ON [dbo].[CoQs]([AppId] ASC);

