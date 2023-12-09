CREATE TABLE [dbo].[Applications] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationTypeId] INT            NULL,
    [UserId]            NVARCHAR (450) NOT NULL,
    [FacilityId]        INT            NOT NULL,
    [Reference]         NVARCHAR (MAX) NOT NULL,
    [CurrentDeskId]     NVARCHAR (MAX) NOT NULL,
    [FADStaffId]        NVARCHAR (MAX) NULL,
    [FADApproved]       BIT            NOT NULL,
    [CreatedDate]       DATETIME2 (7)  NOT NULL,
    [SubmittedDate]     DATETIME2 (7)  NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [Status]            NVARCHAR (MAX) NOT NULL,
    [IsDeleted]         BIT            NOT NULL,
    [FlowId]            INT            NULL,
    [DischargePort]     NVARCHAR (MAX) CONSTRAINT [DF__Applicati__Disch__236943A5] DEFAULT (N'') NOT NULL,
    [IMONumber]         NVARCHAR (MAX) CONSTRAINT [DF__Applicati__IMONu__245D67DE] DEFAULT (N'') NOT NULL,
    [LoadingPort]       NVARCHAR (MAX) CONSTRAINT [DF__Applicati__Loadi__25518C17] DEFAULT (N'') NOT NULL,
    [MarketerName]      NVARCHAR (MAX) CONSTRAINT [DF__Applicati__Marke__2645B050] DEFAULT (N'') NOT NULL,
    [ProductId]         INT            NULL,
    [VesselName]        NVARCHAR (MAX) CONSTRAINT [DF__Applicati__Vesse__282DF8C2] DEFAULT (N'') NOT NULL,
    [DeportStateId]     INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Applications_ApplicationTypes_ApplicationTypeId] FOREIGN KEY ([ApplicationTypeId]) REFERENCES [dbo].[ApplicationTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Applications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Applications_Facilities_FacilityId] FOREIGN KEY ([FacilityId]) REFERENCES [dbo].[Facilities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Applications_WorkFlows_FlowId] FOREIGN KEY ([FlowId]) REFERENCES [dbo].[WorkFlows] ([Id])
);






















GO
CREATE NONCLUSTERED INDEX [IX_Applications_FacilityId]
    ON [dbo].[Applications]([FacilityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Applications_UserId]
    ON [dbo].[Applications]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Applications_ApplicationTypeId]
    ON [dbo].[Applications]([ApplicationTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Applications_FlowId]
    ON [dbo].[Applications]([FlowId] ASC);

