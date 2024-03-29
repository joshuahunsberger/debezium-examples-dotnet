CREATE DATABASE Orders
GO

USE Orders;
GO
EXEC sys.sp_cdc_enable_db;
GO

CREATE SCHEMA Inventory;
GO

CREATE TABLE Inventory.Customers (
  CustomerId INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
  FirstName VARCHAR(255) NOT NULL,
  LastName VARCHAR(255) NOT NULL,
  Email VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Inventory.PurchaseOrders (
    PurchaseOrderID BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME2(7) NOT NULL,
    PurchaseOrderKey UNIQUEIDENTIFIER NOT NULL
);

CREATE TABLE Inventory.OrderLineItems (
    OrderLineItemId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Quantity INTEGER NOT NULL,
    TotalPrice DECIMAL(19,2) NULL,
    OrderId BIGINT NULL,
    Item NVARCHAR(255) NULL,
    Status NVARCHAR(255) NULL,
    OrderLineItemKey UNIQUEIDENTIFIER NOT NULL
);

CREATE TABLE Inventory.OutboxEvents (
    Id uniqueidentifier NOT NULL,
    AggregateType NVARCHAR(255) NOT NULL,
    AggregateId NVARCHAR(255) NOT NULL,
    Type NVARCHAR(255) NOT NULL,
    Payload NVARCHAR(4000) NULL,
    TraceParent NVARCHAR(255) NULL
);

EXEC sys.sp_cdc_enable_table @source_schema = 'Inventory', @source_name = 'OutboxEvents', @role_name = NULL, @supports_net_changes = 0;

GO

CREATE DATABASE Shipments;
GO

USE Shipments;
GO

CREATE SCHEMA Inventory;
GO

CREATE TABLE Inventory.Customers (
  CustomerId INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
  FirstName VARCHAR(255) NOT NULL,
  LastName VARCHAR(255) NOT NULL,
  Email VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Inventory.PurchaseOrders (
    PurchaseOrderID BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME2(7) NOT NULL,
    PurchaseOrderKey UNIQUEIDENTIFIER NOT NULL
);

CREATE TABLE Inventory.OrderLineItems (
    OrderLineItemId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Quantity INTEGER NOT NULL,
    TotalPrice DECIMAL(19,2) NULL,
    OrderId BIGINT NULL,
    Item NVARCHAR(255) NULL,
    Status NVARCHAR(255) NULL,
    OrderLineItemKey UNIQUEIDENTIFIER NOT NULL
);

CREATE TABLE Inventory.Shipment (
    ShipmentId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME2(7) NOT NULL,
    PurchaseOrderKey UNIQUEIDENTIFIER NOT NULL
);

CREATE TABLE Inventory.ConsumedMessages (
    EventId uniqueidentifier NOT NULL,
    TimeOfReceiving DATETIMEOFFSET NOT NULL
);

GO
