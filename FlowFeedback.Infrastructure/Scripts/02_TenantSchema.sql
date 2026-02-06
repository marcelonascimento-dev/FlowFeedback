-- Tabelas que serão criadas EM CADA CLIENTE (FlowFeedback_100, 101...)

CREATE TABLE Empresas (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nome NVARCHAR(200) NOT NULL,
    CNPJ NVARCHAR(20) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Telefone NVARCHAR(20) NOT NULL,
    CEP NVARCHAR(10) NOT NULL,
    Logradouro NVARCHAR(255) NOT NULL,
    Numero NVARCHAR(20) NOT NULL,
    Complemento NVARCHAR(100) NULL,
    Bairro NVARCHAR(100) NOT NULL,
    Cidade NVARCHAR(100) NOT NULL,
    UF NVARCHAR(2) NOT NULL,
    LogoUrlOverride NVARCHAR(500) NULL,
    CorPrimariaOverride NVARCHAR(20) NULL,
    CorSecundariaOverride NVARCHAR(20) NULL,
    Ativo BIT DEFAULT 1
);

CREATE TABLE AlvosAvaliacao (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EmpresaId UNIQUEIDENTIFIER NOT NULL,
    Nome NVARCHAR(200) NOT NULL,
    Subtitulo NVARCHAR(200) NULL,
    ImagemUrl NVARCHAR(500) NULL,
    Ordem INT DEFAULT 0,
    Tipo INT NOT NULL,
    Ativo BIT DEFAULT 1,
    FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id)
);

CREATE TABLE Dispositivos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EmpresaId UNIQUEIDENTIFIER NOT NULL,
    Nome NVARCHAR(100) NOT NULL,
    Identificador NVARCHAR(100) NOT NULL,
    Ativo BIT DEFAULT 1,
    DataCriacao DATETIME DEFAULT GETUTCDATE(),
    FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id)
);
CREATE INDEX IX_Dispositivos_Identificador ON Dispositivos(Identificador);

CREATE TABLE DispositivoAlvos (
    DispositivoId UNIQUEIDENTIFIER NOT NULL,
    AlvoAvaliacaoId UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (DispositivoId, AlvoAvaliacaoId),
    FOREIGN KEY (DispositivoId) REFERENCES Dispositivos(Id),
    FOREIGN KEY (AlvoAvaliacaoId) REFERENCES AlvosAvaliacao(Id)
);

CREATE TABLE Votos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EmpresaId UNIQUEIDENTIFIER NOT NULL,
    DispositivoId UNIQUEIDENTIFIER NOT NULL,
    Valor INT NOT NULL,
    Comentario NVARCHAR(1000) NULL,
    DataHora DATETIME NOT NULL,
    DataProcessamento DATETIME NULL,
    FOREIGN KEY (DispositivoId) REFERENCES Dispositivos(Id),
    FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id)
);
CREATE INDEX IX_Votos_DataHora ON Votos(DataHora);
