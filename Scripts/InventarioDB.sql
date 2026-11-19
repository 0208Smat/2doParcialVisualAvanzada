-- ============================================================================
-- Script SQL Server para reconstruir la base de datos InventarioDB
-- Compatible con Entity Framework Core 8 (.NET 8) - InventarioApiVB
-- ============================================================================

IF DB_ID(N'InventarioDB') IS NULL
BEGIN
    CREATE DATABASE [InventarioDB];
END
GO

USE [InventarioDB];
GO

-- Tabla de historial de migraciones de Entity Framework Core
IF OBJECT_ID(N'[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
GO

-- Tabla Categorias
IF OBJECT_ID(N'[Categorias]', N'U') IS NULL
BEGIN
    CREATE TABLE [Categorias] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Nombre] NVARCHAR(50) NOT NULL,
        [Descripcion] NVARCHAR(200) NULL,
        CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
    );

    CREATE UNIQUE INDEX [IX_Categorias_Nombre] ON [Categorias] ([Nombre]);
END
GO

-- Tabla Productos (Relación 1:N con Categorias, ON DELETE NO ACTION / RESTRICT)
IF OBJECT_ID(N'[Productos]', N'U') IS NULL
BEGIN
    CREATE TABLE [Productos] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Nombre] NVARCHAR(50) NOT NULL,
        [Descripcion] NVARCHAR(200) NULL,
        [Precio] DECIMAL(18,2) NOT NULL,
        [Stock] INT NOT NULL,
        [FechaRegistro] DATETIME2 NOT NULL,
        [CategoriaId] INT NOT NULL,
        CONSTRAINT [PK_Productos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Productos_Categorias_CategoriaId]
            FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id])
            ON DELETE NO ACTION
    );

    CREATE INDEX [IX_Productos_CategoriaId] ON [Productos] ([CategoriaId]);
    CREATE UNIQUE INDEX [IX_Productos_Nombre] ON [Productos] ([Nombre]);
END
GO

-- Registrar la migración inicial en el historial de EF Core
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20261118000000_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261118000000_InitialCreate', N'8.0.11');
END
GO
