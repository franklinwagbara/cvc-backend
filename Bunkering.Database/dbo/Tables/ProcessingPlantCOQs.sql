CREATE TABLE [dbo].[ProcessingPlantCOQs] (
    [ProcessingPlantCOQId]          INT            IDENTITY (1, 1) NOT NULL,
    [PlantId]                       INT            NOT NULL,
    [ProductId]                     INT            NOT NULL,
    [MeasurementSystem]             NVARCHAR (MAX) NOT NULL,
    [MeterTypeId]                   INT            NULL,
    [DipMethodId]                   INT            NULL,
    [StartTime]                     DATETIME2 (7)  NULL,
    [EndTime]                       DATETIME2 (7)  NULL,
    [ConsignorName]                 NVARCHAR (MAX) NULL,
    [Consignee]                     NVARCHAR (MAX) NULL,
    [Terminal]                      NVARCHAR (MAX) NULL,
    [Destination]                   NVARCHAR (MAX) NULL,
    [ShipmentNo]                    NVARCHAR (MAX) NULL,
    [ShoreFigure]                   FLOAT (53)     NULL,
    [ShipFigure]                    FLOAT (53)     NULL,
    [PrevMCubeAt15Degree]           FLOAT (53)     NULL,
    [PrevUsBarrelsAt15Degree]       FLOAT (53)     NULL,
    [PrevMTVac]                     FLOAT (53)     NULL,
    [PrevMTAir]                     FLOAT (53)     NULL,
    [PrevWTAir]                     FLOAT (53)     NULL,
    [PrevLongTonsAir]               FLOAT (53)     NULL,
    [LeftMCubeAt15Degree]           FLOAT (53)     NULL,
    [LeftUsBarrelsAt15Degree]       FLOAT (53)     NULL,
    [LeftMTVac]                     FLOAT (53)     NULL,
    [LeftMTAir]                     FLOAT (53)     NULL,
    [LeftLongTonsAir]               FLOAT (53)     NULL,
    [TotalMCubeAt15Degree]          FLOAT (53)     NULL,
    [TotalUsBarrelsAt15Degree]      FLOAT (53)     NULL,
    [TotalMTVac]                    FLOAT (53)     NULL,
    [TotalMTAir]                    FLOAT (53)     NULL,
    [TotalLongTonsAir]              FLOAT (53)     NULL,
    [CreatedAt]                     DATETIME2 (7)  DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [CreatedBy]                     NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    [DeliveredLongTonsAir]          FLOAT (53)     NULL,
    [DeliveredMCubeAt15Degree]      FLOAT (53)     NULL,
    [DeliveredMTAir]                FLOAT (53)     NULL,
    [DeliveredMTVac]                FLOAT (53)     NULL,
    [DeliveredUsBarrelsAt15Degree]  FLOAT (53)     NULL,
    [Reference]                     NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    [Status]                        NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    [CurrentDeskId]                 NVARCHAR (MAX) NULL,
    [SubmittedDate]                 DATETIME2 (7)  NULL,
    [DateModified]                  DATETIME2 (7)  NULL,
    [Price]                         FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [ApiGravity]                    FLOAT (53)     NULL,
    [AverageBsw]                    FLOAT (53)     NULL,
    [AverageSgAt60]                 FLOAT (53)     NULL,
    [Location]                      NVARCHAR (MAX) NULL,
    [TotalBswBarrelsAt60]           FLOAT (53)     NULL,
    [TotalBswLongTons]              FLOAT (53)     NULL,
    [TotalGrossBarrelsAt60]         FLOAT (53)     NULL,
    [TotalGrossLongTons]            FLOAT (53)     NULL,
    [TotalGrossUsBarrelsAtTankTemp] FLOAT (53)     NULL,
    [TotalNettLongTons]             FLOAT (53)     NULL,
    [TotalNettMetricTons]           FLOAT (53)     NULL,
    [TotalNettUsBarrelsAt60]        FLOAT (53)     NULL,
    CONSTRAINT [PK_ProcessingPlantCOQs] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQId] ASC),
    CONSTRAINT [FK_ProcessingPlantCOQs_Plants_PlantId] FOREIGN KEY ([PlantId]) REFERENCES [dbo].[Plants] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProcessingPlantCOQs_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQs_ProductId]
    ON [dbo].[ProcessingPlantCOQs]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQs_PlantId]
    ON [dbo].[ProcessingPlantCOQs]([PlantId] ASC);

