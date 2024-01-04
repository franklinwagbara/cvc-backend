CREATE TABLE [dbo].[CoQs] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [AppId]                  INT             NOT NULL,
    [DepotId]                INT             NOT NULL,
    [DateOfVesselArrival]    DATETIME2 (7)   NOT NULL,
    [DateOfVesselUllage]     DATETIME2 (7)   NOT NULL,
    [DateOfSTAfterDischarge] DATETIME2 (7)   NOT NULL,
    [MT_VAC]                 DECIMAL (18, 2) NOT NULL,
    [MT_AIR]                 DECIMAL (18, 2) NOT NULL,
    [GOV]                    DECIMAL (18, 2) NOT NULL,
    [GSV]                    DECIMAL (18, 2) NOT NULL,
    [DepotPrice]             DECIMAL (18, 2) NOT NULL,
    [DateCreated]            DATETIME2 (7)   NOT NULL,
    [DateModified]           DATETIME2 (7)   NULL,
    [CreatedBy]              NVARCHAR (MAX)  NOT NULL,
    [Status] NVARCHAR(50) NULL DEFAULT null, 
    [IsDeleted] BIT NULL DEFAULT 0, 
    [Reference] NVARCHAR(100) NULL DEFAULT null, 
    [CurrentDeskId] NVARCHAR(200) NULL DEFAULT null, 
    [FADApproved] BIT NULL DEFAULT 0, 
    [SubmittedDate] DATETIME2 NULL DEFAULT null, 
    CONSTRAINT [PK_CoQs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CoQs_Applications_AppId] FOREIGN KEY ([AppId]) REFERENCES [dbo].[Applications] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CoQs_Depots_DepotId] FOREIGN KEY ([DepotId]) REFERENCES [dbo].[Depots] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CoQs_DepotId]
    ON [dbo].[CoQs]([DepotId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CoQs_AppId]
    ON [dbo].[CoQs]([AppId] ASC);

