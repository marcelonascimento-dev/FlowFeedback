-- Tabelas do Banco CENTRAL (FlowFeedback_Master)

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tenants')
BEGIN
    CREATE TABLE Tenants (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Codigo BIGINT IDENTITY(100,1) NOT NULL,
        Nome NVARCHAR(200) NOT NULL,
        Slug NVARCHAR(200) NOT NULL,
        Status INT NOT NULL DEFAULT 1,
        TipoAmbiente INT NOT NULL DEFAULT 0,
        ConnectionSecretKey NVARCHAR(MAX) NULL,
        DataCriacao DATETIME DEFAULT GETUTCDATE()
    );
    CREATE UNIQUE INDEX IX_Tenants_Codigo ON Tenants(Codigo);
    CREATE INDEX IX_Tenants_Slug ON Tenants(Slug);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        TenantCode BIGINT NULL,
        Nome NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) UNIQUE NOT NULL,
        SenhaHash NVARCHAR(255) NOT NULL,
        Role NVARCHAR(50) DEFAULT 'User',
        Ativo BIT DEFAULT 1,
        DataCriacao DATETIME DEFAULT GETUTCDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DispositivoKeys')
BEGIN
    CREATE TABLE DispositivoKeys (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ApiKeyHash NVARCHAR(256) NOT NULL,
        TenantCode BIGINT NOT NULL,
        NomeDispositivo NVARCHAR(100),
        HardwareSignature NVARCHAR(255) NULL,
        Ativo BIT DEFAULT 1,
        DataCriacao DATETIME DEFAULT GETUTCDATE()
    CREATE INDEX IX_ApiKeyHash ON DispositivoKeys(ApiKeyHash);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TenantUserIndex')
BEGIN
    CREATE TABLE TenantUserIndex (
        Email NVARCHAR(150) NOT NULL PRIMARY KEY,
        UserId UNIQUEIDENTIFIER NOT NULL,
        TenantCodigo BIGINT NOT NULL,
        Ativo BIT DEFAULT 1,
        DataCriacao DATETIME DEFAULT GETUTCDATE()
    );
    CREATE INDEX IX_TenantUserIndex_UserId ON TenantUserIndex(UserId);
END
