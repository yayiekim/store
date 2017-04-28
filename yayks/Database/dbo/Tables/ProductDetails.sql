CREATE TABLE [dbo].[ProductDetails] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [ProductId]            NVARCHAR (128) NOT NULL,
    [ProductColorId]       INT            NULL,
    [ProductMeasurementId] INT            NULL,
    [Length]               FLOAT (53)     NOT NULL,
    [Width]                FLOAT (53)     NOT NULL,
    [Height]               FLOAT (53)     NOT NULL,
    [Weight]               FLOAT (53)     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductDetails_ProductColors] FOREIGN KEY ([ProductColorId]) REFERENCES [dbo].[ProductColors] ([Id]),
    CONSTRAINT [FK_ProductDetails_ProductMeasurements] FOREIGN KEY ([ProductMeasurementId]) REFERENCES [dbo].[ProductMeasurements] ([Id]),
    CONSTRAINT [FK_ProductDetails_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);

