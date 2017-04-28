CREATE TABLE [dbo].[OrderDetails] (
    [Id]                     NVARCHAR (128) NOT NULL,
    [OrdersId]               NVARCHAR (128) NOT NULL,
    [ProductsId]             NVARCHAR (128) NOT NULL,
    [Quantity]               INT            NOT NULL,
    [Remarks]                NVARCHAR (MAX) NULL,
    [ProductAmount]          MONEY          NOT NULL,
    [OrderDetailsStatus]     NVARCHAR (50)  NULL,
    [DateAdded]              DATETIME       NOT NULL,
    [ShippingAmount]         MONEY          NULL,
    [ShippingTrackingNumber] NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY ([OrdersId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderDetails_Products] FOREIGN KEY ([ProductsId]) REFERENCES [dbo].[Products] ([Id])
);

