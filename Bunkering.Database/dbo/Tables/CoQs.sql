CREATE TABLE [dbo].[CoQs] (
    [Id]                              INT            IDENTITY (1, 1) NOT NULL,
    [AppId]                           INT            NOT NULL,
    [DepotId]                         INT            NOT NULL,
    [DateOfVesselArrival]             DATETIME2 (7)  NOT NULL,
    [DateOfVesselUllage]              DATETIME2 (7)  NOT NULL,
    [DateOfSTAfterDischarge]          DATETIME2 (7)  NOT NULL,
    [MT_VAC]                          FLOAT (53)     NOT NULL,
    [MT_AIR]                          FLOAT (53)     NOT NULL,
    [GOV]                             FLOAT (53)     NOT NULL,
    [GSV]                             FLOAT (53)     NOT NULL,
    [DepotPrice]                      FLOAT (53)     NOT NULL,
    [DateCreated]                     DATETIME2 (7)  NOT NULL,
    [DateModified]                    DATETIME2 (7)  NULL,
    [CreatedBy]                       NVARCHAR (MAX) NOT NULL,
    [CurrentDeskId]                   NVARCHAR (MAX) NULL,
    [FADApproved]                     BIT            NULL,
    [IsDeleted]                       BIT            NULL,
    [Reference]                       NVARCHAR (MAX) NULL,
    [Status]                          NVARCHAR (MAX) NULL,
    [SubmittedDate]                   DATETIME2 (7)  NULL,
    [ArrivalShipFigure]               FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [DifferenceBtwShipAndShoreFigure] FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [DischargeShipFigure]             FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [PercentageDifference]            FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [PlantId]                         INT            DEFAULT ((0)) NOT NULL,
    [QuauntityReflectedOnBill]        FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    CONSTRAINT [PK_CoQs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CoQs_Applications_AppId] FOREIGN KEY ([AppId]) REFERENCES [dbo].[Applications] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CoQs_Depots_DepotId] FOREIGN KEY ([DepotId]) REFERENCES [dbo].[Depots] ([Id]) ON DELETE CASCADE
);
















GO



GO
CREATE NONCLUSTERED INDEX [IX_CoQs_AppId]
    ON [dbo].[CoQs]([AppId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CoQs_DepotId]
    ON [dbo].[CoQs]([DepotId] ASC);

