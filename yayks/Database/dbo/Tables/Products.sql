CREATE TABLE [dbo].[Products] (
    [Id]              NVARCHAR (128)  NOT NULL,
    [ProductName]     NVARCHAR (1000) NOT NULL,
    [Amount]          MONEY           NOT NULL,
    [Description]     NVARCHAR (MAX)  NOT NULL,
    [IsEnabled]       BIT             DEFAULT ((1)) NOT NULL,
    [ProductBrandId]  INT             NOT NULL,
    [CreatedByUserId] NVARCHAR (128)  NOT NULL,
    [DateCreated]     DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Products_AspNetUser] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Products_ProductBrand] FOREIGN KEY ([ProductBrandId]) REFERENCES [dbo].[ProductBrands] ([Id])
);

