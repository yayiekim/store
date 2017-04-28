CREATE TABLE [dbo].[PaymentDetails] (
    [Id]              NVARCHAR (128)  NOT NULL,
    [PaymentId]       NVARCHAR (128)  NOT NULL,
    [PaymentMethodId] INT             NOT NULL,
    [PaymentRef]      NVARCHAR (150)  NOT NULL,
    [PayerName]       NVARCHAR (50)   NULL,
    [PaymentAmount]   MONEY           NOT NULL,
    [Remarks]         NVARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PaymentDetails_PaymentMethod] FOREIGN KEY ([PaymentMethodId]) REFERENCES [dbo].[PaymentMethods] ([Id]),
    CONSTRAINT [FK_PaymentDetails_Payments] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[Payments] ([Id]) ON DELETE CASCADE
);

