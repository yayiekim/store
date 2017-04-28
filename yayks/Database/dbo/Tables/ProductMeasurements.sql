CREATE TABLE [dbo].[ProductMeasurements] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [MeasurementName]   NVARCHAR (50) NOT NULL,
    [ProductCategoryId] INT           NOT NULL,
    [MeasurementValue]  DECIMAL (18)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductMeasurements_ProductCategories] FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[ProductCategories] ([Id])
);

