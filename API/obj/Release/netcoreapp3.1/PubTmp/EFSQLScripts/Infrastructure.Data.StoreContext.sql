IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [CustomerNames] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_CustomerNames] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [DeliveryMethods] (
        [Id] int NOT NULL IDENTITY,
        [ShortName] nvarchar(max) NULL,
        [DeliveryTime] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [Price] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_DeliveryMethods] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [FinaceStorges] (
        [Id] int NOT NULL IDENTITY,
        [BuyerEmail] nvarchar(max) NULL,
        [PhotoFileName] nvarchar(max) NULL,
        CONSTRAINT [PK_FinaceStorges] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [inventoryTransaction] (
        [Id] int NOT NULL IDENTITY,
        [DesQuantity] int NOT NULL,
        [PhysicalQuantity] int NOT NULL,
        [ActionTime] datetime2 NOT NULL,
        CONSTRAINT [PK_inventoryTransaction] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [ProductBrands] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_ProductBrands] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [ProductTypes] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_ProductTypes] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [Orders] (
        [Id] int NOT NULL IDENTITY,
        [BuyerEmail] nvarchar(max) NULL,
        [OrderDate] datetimeoffset NOT NULL,
        [ShipToAddress_FirstName] nvarchar(max) NULL,
        [ShipToAddress_LastName] nvarchar(max) NULL,
        [ShipToAddress_PhoneNumber] nvarchar(max) NULL,
        [ShipToAddress_Street] nvarchar(max) NULL,
        [ShipToAddress_City] nvarchar(max) NULL,
        [ShipToAddress_State] nvarchar(max) NULL,
        [ShipToAddress_Country] nvarchar(max) NULL,
        [ShipToAddress_Pincode] nvarchar(max) NULL,
        [DeliveryMethodId] int NULL,
        [SubTotal] decimal(18,2) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [PaymentIntentId] nvarchar(max) NULL,
        [PictureUrl] nvarchar(max) NULL,
        [FailMessage] nvarchar(max) NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_DeliveryMethods_DeliveryMethodId] FOREIGN KEY ([DeliveryMethodId]) REFERENCES [DeliveryMethods] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [Inventories] (
        [Id] int NOT NULL IDENTITY,
        [Supplier] nvarchar(max) NULL,
        [OrderName] nvarchar(max) NULL,
        [TotalQuantity] int NOT NULL,
        [TimeStamp] datetime2 NOT NULL,
        [InventoryTransactionId] int NULL,
        [InventoryId] int NOT NULL,
        CONSTRAINT [PK_Inventories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Inventories_inventoryTransaction_InventoryTransactionId] FOREIGN KEY ([InventoryTransactionId]) REFERENCES [inventoryTransaction] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Supplier] nvarchar(max) NULL,
        [Description] nvarchar(180) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Rating] decimal(18,2) NOT NULL,
        [AvailableQuantity] int NOT NULL,
        [Limit] int NOT NULL,
        [PictureUrl] nvarchar(max) NOT NULL,
        [ProductTypeId] int NOT NULL,
        [ProductBrandId] int NOT NULL,
        [QuAccept] int NOT NULL,
        [QuReject] int NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_ProductBrands_ProductBrandId] FOREIGN KEY ([ProductBrandId]) REFERENCES [ProductBrands] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Products_ProductTypes_ProductTypeId] FOREIGN KEY ([ProductTypeId]) REFERENCES [ProductTypes] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] int NOT NULL IDENTITY,
        [ItemOrdered_ProductItemId] int NULL,
        [ItemOrdered_ProductName] nvarchar(max) NULL,
        [ItemOrdered_PictureUrl] nvarchar(max) NULL,
        [ItemOrdered_Description] nvarchar(max) NULL,
        [ItemOrdered_Supplier] nvarchar(max) NULL,
        [Price] decimal(18,2) NOT NULL,
        [Quantity] int NOT NULL,
        [PictureUrl] nvarchar(max) NULL,
        [OrderId] int NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE TABLE [Despatches] (
        [Id] int NOT NULL IDENTITY,
        [CustomerNameId] int NULL,
        [OrderItemId] int NULL,
        [Technition] nvarchar(max) NULL,
        [ApprovelName] nvarchar(max) NULL,
        [Date] datetime2 NOT NULL,
        CONSTRAINT [PK_Despatches] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Despatches_CustomerNames_CustomerNameId] FOREIGN KEY ([CustomerNameId]) REFERENCES [CustomerNames] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Despatches_OrderItems_OrderItemId] FOREIGN KEY ([OrderItemId]) REFERENCES [OrderItems] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE INDEX [IX_Despatches_CustomerNameId] ON [Despatches] ([CustomerNameId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE INDEX [IX_Despatches_OrderItemId] ON [Despatches] ([OrderItemId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE INDEX [IX_Inventories_InventoryTransactionId] ON [Inventories] ([InventoryTransactionId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE INDEX [IX_Orders_DeliveryMethodId] ON [Orders] ([DeliveryMethodId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE INDEX [IX_Products_ProductBrandId] ON [Products] ([ProductBrandId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    CREATE INDEX [IX_Products_ProductTypeId] ON [Products] ([ProductTypeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220202122817_inits')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220202122817_inits', N'3.1.21');
END;

GO

