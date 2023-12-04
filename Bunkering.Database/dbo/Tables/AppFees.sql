CREATE TABLE [dbo].[AppFees] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [ApplicationTypeId] INT             NOT NULL,
    [VesseltypeId]      INT             NOT NULL,
    [ApplicationFee]    DECIMAL (18, 2) NOT NULL,
    [AccreditationFee]  DECIMAL (18, 2) NOT NULL,
    [VesselLicenseFee]  DECIMAL (18, 2) NOT NULL,
    [AdministrativeFee] DECIMAL (18, 2) NOT NULL,
    [InspectionFee]     DECIMAL (18, 2) NOT NULL,
    [SerciveCharge]     DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_AppFees] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AppFees_ApplicationTypes_ApplicationTypeId] FOREIGN KEY ([ApplicationTypeId]) REFERENCES [dbo].[ApplicationTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AppFees_VesselTypes_VesseltypeId] FOREIGN KEY ([VesseltypeId]) REFERENCES [dbo].[VesselTypes] ([Id]) ON DELETE CASCADE
);












GO
CREATE NONCLUSTERED INDEX [IX_AppFees_ApplicationTypeId]
    ON [dbo].[AppFees]([ApplicationTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AppFees_VesseltypeId]
    ON [dbo].[AppFees]([VesseltypeId] ASC);

