CREATE TABLE [dbo].[CoQs] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [AppId]                  INT            NOT NULL,
    [DepotId]                INT            NOT NULL,
    [DateOfVesselArrival]    DATETIME2 (7)  NOT NULL,
    [DateOfVesselUllage]     DATETIME2 (7)  NOT NULL,
    [DateOfSTAfterDischarge] DATETIME2 (7)  NULL,
    [MT_VAC]                 DECIMAL (18)   NOT NULL,
    [MT_AIR]                 DECIMAL (18)   NOT NULL,
    [GOV]                    DECIMAL (18)   NOT NULL,
    [GSV]                    DECIMAL (18)   NOT NULL,
    [DepotPrice]             DECIMAL (18)   NOT NULL,
    [DateCreated]            DATETIME2 (7)  NULL,
    [DateModified]           DATETIME2 (7)  NOT NULL,
    [CreatedBy]              NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_CoQs] PRIMARY KEY CLUSTERED ([Id] ASC)
);


