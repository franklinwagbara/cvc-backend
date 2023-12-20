CREATE TABLE [dbo].[CoQs]
(
	[Id] INT IDENTITY(1, 1) NOT NULL, 
    [AppId] INT NOT NULL, 
    [DepotId] INT NOT NULL, 
    [DateOfVesselArrival] DATETIME2 NOT NULL, 
    [DateOfVesselUllage] DATETIME2 NOT NULL, 
    [DateOfSTAfterDischarge] DATETIME2 NULL, 
    [MT_VAC] DECIMAL NOT NULL, 
    [MT_AIR] DECIMAL NOT NULL, 
    [GOV] DECIMAL NOT NULL, 
    [GSV] DECIMAL NOT NULL, 
    [DepotPrice] DECIMAL NOT NULL, 
    [DateCreated] DATETIME2 NULL, 
    [DateModified] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_CoQs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CoQs_Applications_AppId] FOREIGN KEY ([AppId]) REFERENCES [dbo].[Applications] ([Id])
)
