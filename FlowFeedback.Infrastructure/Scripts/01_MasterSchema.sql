-- Tabelas do Banco CENTRAL (FlowFeedback_Master)

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tenants')
BEGIN
    CREATE TABLE Tenants (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(200) NOT NULL,
        Slug NVARCHAR(200) NOT NULL,
        Status INT NOT NULL DEFAULT 1,
        DbServer NVARCHAR(200) NOT NULL,
        DbName NVARCHAR(200) NOT NULL,
        DbUser NVARCHAR(200) NOT NULL,
        DbPassword VARBINARY(MAX) NOT NULL,
        CreatedAt DATETIME DEFAULT GETUTCDATE()
    );
    CREATE UNIQUE INDEX IX_Tenants_Slug ON Tenants(Slug);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Email NVARCHAR(150) UNIQUE NOT NULL,
        PasswordHash NVARCHAR(255) NOT NULL,
        IsActive BIT DEFAULT 1,
        EmailConfirmed BIT DEFAULT 0,
        CreatedAt DATETIME DEFAULT GETUTCDATE()
    );
    CREATE INDEX IX_Users_Email ON Users(Email);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserTenants')
BEGIN
    CREATE TABLE UserTenants (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UserId UNIQUEIDENTIFIER NOT NULL,
        TenantId UNIQUEIDENTIFIER NOT NULL,
        Role INT NOT NULL, -- EnumUserRole
        IsActive BIT DEFAULT 1,
        CONSTRAINT FK_UserTenants_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_UserTenants_Tenants FOREIGN KEY (TenantId) REFERENCES Tenants(Id)
    );
    CREATE INDEX IX_UserTenants_User ON UserTenants(UserId);
    CREATE INDEX IX_UserTenants_Tenant ON UserTenants(TenantId);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DispositivoKeys')
BEGIN
    CREATE TABLE DispositivoKeys (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ApiKeyHash NVARCHAR(256) NOT NULL,
        TenantId UNIQUEIDENTIFIER NOT NULL,
        NomeDispositivo NVARCHAR(100),
        Ativo BIT DEFAULT 1,
        DataCriacao DATETIME DEFAULT GETUTCDATE(),
        CONSTRAINT FK_DispositivoKeys_Tenants FOREIGN KEY (TenantId) REFERENCES Tenants(Id)
    );
    CREATE INDEX IX_ApiKeyHash ON DispositivoKeys(ApiKeyHash);
END
