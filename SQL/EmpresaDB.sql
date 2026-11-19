/*
  Esquema para ProductosAPI.
  Se puede ejecutar más de una vez. Si las tablas ya existen, agrega/ajusta
  las columnas requeridas y conserva las filas actuales.
*/
IF DB_ID(N'EmpresaDB') IS NULL
    CREATE DATABASE EmpresaDB;
GO

USE EmpresaDB;
GO

IF OBJECT_ID(N'dbo.Categorias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categorias (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categorias PRIMARY KEY,
        Nombre NVARCHAR(50) COLLATE Latin1_General_100_CI_AS NOT NULL,
        Descripcion NVARCHAR(200) NULL
    );
END;
GO

IF COL_LENGTH(N'dbo.Categorias', N'Descripcion') IS NULL
    ALTER TABLE dbo.Categorias ADD Descripcion NVARCHAR(200) NULL;
GO

IF EXISTS (SELECT 1 FROM dbo.Categorias WHERE Nombre IS NULL OR LEN(Nombre) = 0 OR LEN(Nombre) > 50)
    THROW 51001, 'Hay nombres de categoría vacíos o de más de 50 caracteres. Corrige esos datos antes de continuar.', 1;
GO
ALTER TABLE dbo.Categorias ALTER COLUMN Nombre NVARCHAR(50) COLLATE Latin1_General_100_CI_AS NOT NULL;
GO

IF OBJECT_ID(N'dbo.Productos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Productos PRIMARY KEY,
        Nombre NVARCHAR(50) COLLATE Latin1_General_100_CI_AS NOT NULL,
        Descripcion NVARCHAR(200) NULL,
        Precio DECIMAL(18,2) NOT NULL,
        Stock INT NOT NULL CONSTRAINT DF_Productos_Stock DEFAULT (0),
        FechaRegistro DATETIME2 NOT NULL CONSTRAINT DF_Productos_FechaRegistro DEFAULT (SYSUTCDATETIME()),
        CategoriaId INT NOT NULL
    );
END;
GO

IF COL_LENGTH(N'dbo.Productos', N'Stock') IS NULL
    ALTER TABLE dbo.Productos ADD Stock INT NOT NULL CONSTRAINT DF_Productos_Stock DEFAULT (0) WITH VALUES;
GO
IF COL_LENGTH(N'dbo.Productos', N'FechaRegistro') IS NULL
    ALTER TABLE dbo.Productos ADD FechaRegistro DATETIME2 NOT NULL CONSTRAINT DF_Productos_FechaRegistro DEFAULT (SYSUTCDATETIME()) WITH VALUES;
GO

IF EXISTS (SELECT 1 FROM dbo.Productos WHERE Nombre IS NULL OR LEN(Nombre) = 0 OR LEN(Nombre) > 50)
    THROW 51002, 'Hay nombres de producto vacíos o de más de 50 caracteres. Corrige esos datos antes de continuar.', 1;
IF EXISTS (SELECT 1 FROM dbo.Productos WHERE LEN(Descripcion) > 200)
    THROW 51003, 'Hay descripciones de producto de más de 200 caracteres. Corrige esos datos antes de continuar.', 1;
IF EXISTS (SELECT 1 FROM dbo.Productos WHERE Precio <= 0 OR Stock < 0)
    THROW 51004, 'Hay productos con precio no positivo o stock negativo. Corrige esos datos antes de continuar.', 1;
IF EXISTS (SELECT 1 FROM dbo.Productos p LEFT JOIN dbo.Categorias c ON c.Id = p.CategoriaId WHERE c.Id IS NULL)
    THROW 51005, 'Hay productos cuya categoría no existe. Corrige esas referencias antes de continuar.', 1;
GO
ALTER TABLE dbo.Productos ALTER COLUMN Nombre NVARCHAR(50) COLLATE Latin1_General_100_CI_AS NOT NULL;
ALTER TABLE dbo.Productos ALTER COLUMN Descripcion NVARCHAR(200) NULL;
GO

IF EXISTS (
    SELECT UPPER(Nombre) COLLATE Latin1_General_100_CI_AS
    FROM dbo.Categorias
    GROUP BY UPPER(Nombre) COLLATE Latin1_General_100_CI_AS
    HAVING COUNT(*) > 1
)
    THROW 51006, 'Hay categorías duplicadas sin distinguir mayúsculas/minúsculas. Resuelve los duplicados antes de continuar.', 1;
IF EXISTS (
    SELECT UPPER(Nombre) COLLATE Latin1_General_100_CI_AS
    FROM dbo.Productos
    GROUP BY UPPER(Nombre) COLLATE Latin1_General_100_CI_AS
    HAVING COUNT(*) > 1
)
    THROW 51007, 'Hay productos duplicados sin distinguir mayúsculas/minúsculas. Resuelve los duplicados antes de continuar.', 1;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Categorias') AND name = N'UX_Categorias_Nombre')
    CREATE UNIQUE INDEX UX_Categorias_Nombre ON dbo.Categorias(Nombre);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Productos') AND name = N'UX_Productos_Nombre')
    CREATE UNIQUE INDEX UX_Productos_Nombre ON dbo.Productos(Nombre);
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'dbo.Productos')
      AND referenced_object_id = OBJECT_ID(N'dbo.Categorias')
)
    ALTER TABLE dbo.Productos WITH CHECK
        ADD CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId)
        REFERENCES dbo.Categorias(Id) ON DELETE NO ACTION;
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.Productos') AND name = N'CK_Productos_Precio_Positive')
    ALTER TABLE dbo.Productos WITH CHECK ADD CONSTRAINT CK_Productos_Precio_Positive CHECK (Precio > 0);
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.Productos') AND name = N'CK_Productos_Stock_NonNegative')
    ALTER TABLE dbo.Productos WITH CHECK ADD CONSTRAINT CK_Productos_Stock_NonNegative CHECK (Stock >= 0);
GO
