create database pim;
use pim;

	-- TABELA SETORES
CREATE TABLE Setores (
    id_setor INT IDENTITY(1,1) PRIMARY KEY,
    nome_setor VARCHAR(50) NOT NULL UNIQUE
);

-- TABELA USUARIOS
CREATE TABLE Usuarios (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    telefone VARCHAR(15) NOT NULL,
    senha VARCHAR(255) NOT NULL,
    id_setor INT NOT NULL,
    perfil VARCHAR(20) NOT NULL CHECK (perfil IN ('Usuario', 'Analista', 'Admin')),
    data_cadastro DATETIME DEFAULT GETDATE(),
    ativo BIT DEFAULT 1,
    FOREIGN KEY (id_setor) REFERENCES Setores(id_setor)
);

-- TABELA CATEGORIAS
CREATE TABLE Categorias (
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    nome_categoria VARCHAR(50) NOT NULL UNIQUE
);

-- TABELA CHAMADOS
CREATE TABLE Chamados (
    id_chamado INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_analista INT NULL,
    titulo VARCHAR(100) NOT NULL,
    descricao VARCHAR(500) NOT NULL,
    id_categoria INT NOT NULL,
    prioridade VARCHAR(10) NOT NULL CHECK (prioridade IN ('Baixa', 'Média', 'Alta')),
    status VARCHAR(20) NOT NULL CHECK (status IN ('Aberto', 'Em analise', 'Aguardando Resposta', 'Encerrado')),
    data_abertura DATETIME DEFAULT GETDATE(),
    data_fechamento DATETIME NULL,
    solucao_informada VARCHAR(500),
    avaliacao_atendimento INT CHECK (avaliacao_atendimento BETWEEN 1 AND 5),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_analista) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_categoria) REFERENCES Categorias(id_categoria)
);

-- TABELA SOLUCOESIA
CREATE TABLE SolucoesIA (
    id_solucao INT IDENTITY(1,1) PRIMARY KEY,
    descricao_problema VARCHAR(500) NOT NULL,
    solucao_sugerida VARCHAR(500) NOT NULL,
    data_criacao DATETIME DEFAULT GETDATE()
);

-- TABELA LOGSAUDITORIA
CREATE TABLE LogsAuditoria (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario_afetado INT NOT NULL,
	id_usuario_executor INT NOT NULL,
    acao VARCHAR(100) NOT NULL,
    tabela_afetada VARCHAR(100),
    data_hora DATETIME DEFAULT GETDATE(),
    ip_origem VARCHAR(50),
    FOREIGN KEY (id_usuario_afetado) REFERENCES Usuarios(id_usuario),
	FOREIGN KEY (id_usuario_executor) REFERENCES Usuarios(id_usuario)
);






SET IDENTITY_INSERT Setores ON;    -- para inserir os ids manualmente,esse e o codigo
-- go;
INSERT INTO Setores (id_setor,nome_setor) VALUES
(1,'TI'),
(2,'RH'),
(3,'Administrativo'),
(4,'Financeiro'),
(5,'Comercial');
-- go;
-- os setores de atendimento
SET IDENTITY_INSERT Setores OFF; -- para fechar o inserir dadosd do setores.
-- go;



-- INSERÇÃO DE USUARIOS, AS SENHAS DEVEM SER CRIPTOGRAFAFDAS ,VOU COLOCAR COMO

SET IDENTITY_INSERT Usuarios ON;
-- go;
INSERT INTO Usuarios (id_usuario,nome, email, telefone, senha, id_setor, perfil, data_cadastro, ativo) VALUES
(1,'Bruno Pereira' , 'bruno.pereira@gmail.com', ' 11986546372' , 'hash_senha', 1, 'Usuario','2025-01-01 10:00:00',1),
(2,'Ana luisa' , 'ana.lulu23@gmail.com', ' 11998765432' , 'hash_senha', 2, 'Usuario','2025-01-01 10:05:00',1),
(3,'Pedro Reis' , 'pedro532@gmail.com', ' 11976548934' , 'hash_senha', 1, 'Analista','2025-01-02 10:30:00',1),
(4,'Fernanda Correa' , 'fefecorrea02@gmail.com', ' 11943210987' , 'hash_senha', 3, 'Admin','2025-01-03 11:00:00',1),
(5,'Carlos Mendes' , 'Mendes319@gmail.com', ' 11976534254' , 'hash_senha', 4, 'Usuario','2025-01-04 11:40:00',1),
(6, 'Fernanda Costa', 'fernanda.costa@gmail.com', '11943210987', 'hash_senha', 1, 'Analista', '2025-01-01 10:00:00', 1);
-- go;
SET IDENTITY_INSERT Usuarios OFF;
-- go;


-- os usuarios cadastrados no sistema   (esse aqui é com as senhas criptografadas ,precisa testar.)
SET IDENTITY_INSERT Usuarios ON;

INSERT INTO Usuarios (id_usuario, nome, email, telefone, senha, id_setor, perfil, data_cadastro, ativo) VALUES
(1,'Bruno Pereira' , 'bruno.pereira@gmail.com', '11986546372' , CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'senha123'), 2), 1, 'Usuario','2025-01-01 10:00:00',1),
(2,'Ana luisa' , 'ana.lulu23@gmail.com', '11998765432' , CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'senha123'), 2), 2, 'Usuario','2025-01-01 10:05:00',1),
(3,'Pedro Reis' , 'pedro532@gmail.com', '11976548934' , CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'senha123'), 2), 1, 'Analista','2025-01-02 10:30:00',1),
(4,'Fernanda Correa' , 'fefecorrea02@gmail.com', '11943210987' , CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'senha123'), 2), 3, 'Admin','2025-01-03 11:00:00',1),
(5,'Carlos Mendes' , 'Mendes319@gmail.com', '11976534254' , CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'senha123'), 2), 4, 'Usuario','2025-01-04 11:40:00',1),
(6, 'Fernanda Costa', 'fernanda.costa@gmail.com', '11943210987', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'senha123'), 2), 1, 'Analista', '2025-01-01 10:00:00', 1);

SET IDENTITY_INSERT Usuarios OFF;




-- inserçaão de categorias
 SET IDENTITY_INSERT Categorias ON;
 -- go;
 INSERT INTO Categorias (id_categoria,nome_categoria) VALUES
 (1,'Acesso'),
 (2,'Sistema'),
 (3,'Rede'),
 (4,'Hardware'),
 (5,'Software');
 -- go;
 SET IDENTITY_INSERT Categorias OFF;
 -- go;

-- categorias do sistema



select * from chamados; -- comando para conferir as tabelas 







-- inserção de chamados
SET IDENTITY_INSERT Chamados ON;
-- go;
INSERT INTO Chamados (id_chamado, id_usuario, id_analista, titulo, descricao, id_categoria, prioridade, status, data_abertura) VALUES
(1,1,3, 'Problema de Login no Sistema', 'Não consigo acessar o sistema.', 1, 'Alta', 'Aberto', '2025-10-23 10:40:00'),
(2,2,3, 'lentidão no sistema do RH', 'Não funciona o comando do sistema do RH', 2, 'Média', 'Em analise', '2025-10-27 14:30:00'),
(3,1, Null, 'Rede sem fio não conecta', 'Meu computador não conecta com a rede da empresa.', 3, 'Alta', 'Aberto', '2025-11-12 08:42:00'),
(4,5,6, 'Impressora não imprime','A impressora do setor financeiro não está respondendo.',4,'Média','Encerrado','2025-10-14 16:36:00');
-- go;
SET IDENTITY_INSERT Chamados OFF;  --  esta com problema esse.
-- go;
-- adicionando oque cada usuario perguntou



-- atualização do chamado encerrrado com soluçaõ e avaliação.
UPDATE Chamados
SET solucao_informada = 'Problema resolvido após reinicialização do servidor de impressão.',
      avaliacao_atendimento = 5,
      data_fechamento = '2025-10-15  10:30:00'
WHERE id_chamado = 4;
-- go


-- Inserção de Soluções IA
SET IDENTITY_INSERT SolucoesIA ON;
INSERT INTO SolucoesIA (id_solucao, descricao_problema, solucao_sugerida)
VALUES
(1, 'Não consigo acessar o sistema', 'Verifique suas credenciais de login e tente novamente. Se o problema persistir, entre em contato com o suporte.'),
(2, 'Sistema lento', 'Verifique a conexão com a internet e reinicie o aplicativo. Se a lentidão persistir, pode ser um problema de servidor.'),
(3, 'Impressora não funciona', 'Verifique se a impressora está ligada e conectada à rede. Tente reiniciar a impressora e o computador.'),
(4, 'Esqueci minha senha', 'Clique em "Esqueci minha senha" na tela de login e siga as instruções enviadas para seu e-mail cadastrado.'),
(5, 'Erro ao salvar arquivo', 'Verifique se há espaço disponível no disco e se você tem permissões de escrita na pasta de destino.');
SET IDENTITY_INSERT SolucoesIA OFF;
-- GO


CREATE PROCEDURE sp_RegistrarLog(
    @p_id_usuario_afetado INT,
    @p_id_usuario_executor INT, -- NOVO PARÂMETRO: Quem fez a alteração
    @p_acao VARCHAR(100),
    @p_tabela_afetada VARCHAR(50),
    @p_ip_origem VARCHAR(45)
)
AS
BEGIN
    INSERT INTO LogsAuditoria (id_usuario_afetado, id_usuario_executor, acao, tabela_afetada, ip_origem)
    VALUES (@p_id_usuario_afetado, @p_id_usuario_executor, @p_acao, @p_tabela_afetada, @p_ip_origem);
END
-- GO

-- Trigger para registrar alterações em chamados (Ajustado para T-SQL e removido o registro incompleto)
-- O trigger original foi removido. A auditoria de UPDATE deve ser feita na aplicação
-- chamando a Stored Procedure sp_RegistrarLog, que é mais segura e correta no T-SQL.
-- O T-SQL não permite a captura do usuário logado de forma simples dentro de um trigger.

-- Exemplo de como a aplicação deve chamar a SP para atualizar um chamado e registrar o log:
/*
BEGIN TRANSACTION;
