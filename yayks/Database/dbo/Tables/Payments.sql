CREATE TABLE [dbo].[Payments] (
    [Id]           NVARCHAR (128) NOT NULL,
    [PaymentDate]  DATETIME       NOT NULL,
    [OrdersId]     NVARCHAR (128) NOT NULL,
    [AspNetUserId] NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Payments_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Payments_Orders] FOREIGN KEY ([OrdersId]) REFERENCES [dbo].[Orders] ([Id])
);

