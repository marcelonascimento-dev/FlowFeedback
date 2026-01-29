-- Tabelas do Banco CENTRAL (FlowFeedback_Master)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tenants')
BEGIN
    CREATE TABLE Tenants (
        Id INT IDENTITY(100,1) PRIMARY KEY, -- Começa do 100
        Nome NVARCHAR(200) NOT NULL,
        Documento NVARCHAR(50) NOT NULL,
        DbServer NVARCHAR(255) DEFAULT 'localhost',
        DbName NVARCHAR(255) NOT NULL,
        DbUser NVARCHAR(100) NULL,
        DbPasswordEncrypted NVARCHAR(MAX) NULL,
        Ativo BIT DEFAULT 1,
        DataCriacao DATETIME DEFAULT GETUTCDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        TenantCode INT NOT NULL,
        Nome NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) UNIQUE NOT NULL,
        SenhaHash NVARCHAR(255) NOT NULL,
        Role NVARCHAR(50) DEFAULT 'User',
        Ativo BIT DEFAULT 1,
        DataCriacao DATETIME DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Usuarios_Tenants FOREIGN KEY (TenantCode) REFERENCES Tenants(Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DispositivoKeys')
BEGIN
    CREATE TABLE DispositivoKeys (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ApiKeyHash NVARCHAR(256) NOT NULL,
        TenantCode INT NOT NULL,
        NomeDispositivo NVARCHAR(100),
        HardwareSignature NVARCHAR(255) NULL,
        Ativo BIT DEFAULT 1,
        DataCriacao DATETIME DEFAULT GETUTCDATE()
    );
    CREATE INDEX IX_ApiKeyHash ON DispositivoKeys(ApiKeyHash);
END