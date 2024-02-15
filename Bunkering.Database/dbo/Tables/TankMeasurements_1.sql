CREATE TABLE [dbo].[TankMeasurements] (
    [Id]                      INT             IDENTITY (1, 1) NOT NULL,
    [COQTankId]               INT             NOT NULL,
    [DIP]                     FLOAT (53)      NOT NULL,
    [WaterDIP]                FLOAT (53)      NOT NULL,
    [TOV]                     FLOAT (53)      NOT NULL,
    [WaterVolume]             FLOAT (53)      NOT NULL,
    [FloatRoofCorr]           FLOAT (53)      NOT NULL,
    [GOV]                     FLOAT (53)      NOT NULL,
    [Tempearture]             DECIMAL (18, 2) NOT NULL,
    [Density]                 FLOAT (53)      NOT NULL,
    [VCF]                     FLOAT (53)      NOT NULL,
    [GSV]                     FLOAT (53)      NOT NULL,
    [MTVAC]                   FLOAT (53)      NOT NULL,
    [ObservedSounding]        FLOAT (53)      NOT NULL,
    [TapeCorrection]          FLOAT (53)      NOT NULL,
    [ObservedLiquidVolume]    FLOAT (53)      NOT NULL,
    [ShrinkageFactorLiquid]   FLOAT (53)      NOT NULL,
    [TankVolume]              FLOAT (53)      NOT NULL,
    [ShrinkageFactorVapour]   FLOAT (53)      NOT NULL,
    [VapourTemperature]       FLOAT (53)      NOT NULL,
    [VapourPressure]          FLOAT (53)      NOT NULL,
    [MolecularWeight]         FLOAT (53)      NOT NULL,
    [VapourFactor]            FLOAT (53)      NOT NULL,
    [LiquidDensityVac]        FLOAT (53)      NOT NULL,
    [MeasurementType]         NVARCHAR (MAX)  DEFAULT (N'') NOT NULL,
    [CorrectedLiquidLevel]    FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [CorrectedLiquidVolumeM3] FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [CorrectedVapourVolume]   FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [GrossStandardVolumeGas]  FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [LiquidDensityAir]        FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [LiquidWeightAir]         FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [LiquidWeightVAC]         FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [TotalGasWeightAir]       FLOAT (53)      NULL,
    [TotalGasWeightVAC]       FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [VapourVolume]            FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [VapourWeightAir]         FLOAT (53)      NULL,
    [VapourWeightVAC]         FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    CONSTRAINT [PK_TankMeasurements] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TankMeasurements_COQTanks_COQTankId] FOREIGN KEY ([COQTankId]) REFERENCES [dbo].[COQTanks] ([Id]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_TankMeasurements_COQTankId]
    ON [dbo].[TankMeasurements]([COQTankId] ASC);

