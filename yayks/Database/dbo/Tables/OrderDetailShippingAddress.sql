CREATE TABLE [dbo].[OrderDetailShippingAddress] (
    [Id]      NVARCHAR (128)  NOT NULL,
    [OdersId] NVARCHAR (128)  NOT NULL,
    [Line1]   NVARCHAR (1000) NOT NULL,
    [Line2]   NVARCHAR (1000) NOT NULL,
    [City]    NVARCHAR (500)  NOT NULL,
    [State]   NVARCHAR (500)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OrderShippingAddress_Oders] FOREIGN KEY ([OdersId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE
);

