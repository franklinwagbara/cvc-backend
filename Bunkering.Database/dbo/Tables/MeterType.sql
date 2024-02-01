CREATE TABLE [dbo].[MeterType] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [CreatedDate]  DATETIME     NULL,
    [ModifiedDate] DATETIME     NULL,
    [IsDeleted]    BIT          CONSTRAINT [DF_MeterType_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MeterType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

