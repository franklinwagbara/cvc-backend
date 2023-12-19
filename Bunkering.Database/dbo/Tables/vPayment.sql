CREATE TABLE [dbo].[vPayment] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    
    [CompanyEmail] NVARCHAR(MAX) NOT NULL, 
    [CompanyName] NVARCHAR(50) NOT NULL, 
    [VesselName] NVARCHAR(50) NOT NULL, 
    [RRR] NVARCHAR(MAX) NOT NULL, 
    [Amount] DECIMAL NOT NULL, 
    [PaymentStatus] NVARCHAR(MAX) NOT NULL, 
    [AppReference] NVARCHAR(MAX) NOT NULL, 
    [ExtraPaymentReference] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_vPayment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

