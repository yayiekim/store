CREATE TABLE [dbo].[CustomerShippingAddresses] (
    [Id]           NVARCHAR (128) NOT NULL,
    [AspNetUserId] NVARCHAR (128) NOT NULL,
    [IsDefault]    BIT            DEFAULT ((1)) NOT NULL,
    [Line1]        NVARCHAR (250) NOT NULL,
    [Line2]        NVARCHAR (250) NOT NULL,
    [City]         NVARCHAR (250) NOT NULL,
    [State]        NVARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CustomerShippingAddresses_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

