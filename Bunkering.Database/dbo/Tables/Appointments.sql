CREATE TABLE [dbo].[Appointments] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId]    INT            NOT NULL,
    [ScheduleType]     NVARCHAR (MAX) NOT NULL,
    [ScheduleDate]     DATETIME2 (7)  NOT NULL,
    [AppointmentDate]  DATETIME2 (7)  NOT NULL,
    [ScheduledBy]      NVARCHAR (MAX) NOT NULL,
    [ScheduleMessage]  NVARCHAR (MAX) NOT NULL,
    [IsApproved]       BIT            NOT NULL,
    [ApprovedBy]       NVARCHAR (MAX) NULL,
    [ApprovalMessage]  NVARCHAR (MAX) NULL,
    [IsAccepted]       BIT            NOT NULL,
    [ClientMessage]    NVARCHAR (MAX) NULL,
    [ContactName]      NVARCHAR (MAX) NULL,
    [PhoneNo]          NVARCHAR (MAX) NULL,
    [ExpiryDate]       DATETIME2 (7)  NOT NULL,
    [NominatedStaffId] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Appointments_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Applications] ([Id]) ON DELETE CASCADE
);














GO
CREATE NONCLUSTERED INDEX [IX_Appointments_ApplicationId]
    ON [dbo].[Appointments]([ApplicationId] ASC);

