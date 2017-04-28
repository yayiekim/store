CREATE TABLE [dbo].[PurchaseDetails] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [PurchaseId]           NVARCHAR (128) NOT NULL,
    [ProductId]            NVARCHAR (128) NOT NULL,
    [Amount]               MONEY          NOT NULL,
    [Quantity]             INT            NOT NULL,
    [Remarks]              NVARCHAR (500) NULL,
    [PurchaseDetailStatus] NVARCHAR (50)  DEFAULT ('new') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchaseDetails_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_PurchaseDetails_Purchases] FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[Purchases] ([Id]) ON DELETE CASCADE
);

