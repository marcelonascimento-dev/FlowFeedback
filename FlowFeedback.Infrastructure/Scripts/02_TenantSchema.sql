-- Tabelas que serão criadas EM CADA CLIENTE (FlowFeedback_100, 101...)

CREATE TABLE Empresas (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nome NVARCHAR(100) NOT NULL,
    Cidade NVARCHAR(100),
    Ativo BIT DEFAULT 1
);

CREATE TABLE Dispositivos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EmpresaId UNIQUEIDENTIFIER NULL,
    Nome NVARCHAR(100) NOT NULL,
    Identificador NVARCHAR(50), -- Código interno visual
    Ativo BIT DEFAULT 1,
    FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id)
);

CREATE TABLE Votos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DispositivoId UNIQUEIDENTIFIER NOT NULL,
    DataHora DATETIME NOT NULL,
    Valor INT NOT NULL, -- 0 a 10 (NPS)
    Comentario NVARCHAR(500) NULL,
    FOREIGN KEY (DispositivoId) REFERENCES Dispositivos(Id)
);
-- Índices para performance de Dashboards
CREATE INDEX IX_Votos_Data ON Votos(DataHora);
CREATE INDEX IX_Votos_Dispositivo ON Votos(DispositivoId);