create database empresa;
use empresa;
create table funcionario(
id int not null auto_increment primary key,
nome varchar(100) not null,
telefone varchar(20) not null,
estadoCivil varchar(20) not null,
sexo char(1) not null,
cpf char(14) not null,
naturalidade varchar(45) not null,
endereco varchar(100) not null,
bairro varchar(45) not null,
cidade varchar(45) not null,
cargo varchar(40) not null,
salario double not null,
data_nascimento date not null
);
INSERT INTO funcionario (id, nome, telefone, estadoCivil, sexo, cpf, naturalidade, endereco, bairro, cidade, cargo, salario, data_nascimento) VALUES
('1','Rafael', '(31) 98210-1613', 'Solteiro', 'M', '123.456.789-43', 'Brasileiro', 'Rua Zurick 540','Nova Suíssa', 'Betim', 'Analista de sistemas', 8800.00, '2008-02-03'),
('2','Gabriel', '(31)97890-3452', 'Casado', 'M', '565.787.989-92', 'Brasileiro', 'Rua Afonso 270', 'Padre Eustáquio', 'Contagem', 'Desenvolvedor de software', 9000.00, '2008-02-03'),
('3''Flávio', '(31)93451-9563', 'Divorciada', 'M', '332.457.826-01', 'Brasileiro', 'Rua Júnior 180', 'Coração Eucarístico', 'Belo Horizonte', 'Programador de Banco de Dados', 12000.00, '1972-08-24');
-- Questao A
select *  from funcionario order by cidade asc;
-- Questao B
select nome  from funcionario where salario > 1800 order by nome asc;
-- Questao C
select data_nascimento, nome, estadoCivil  from funcionario order by data_nascimento asc;
-- Questao D
select nome, telefone  from funcionario where estadoCivil = "Casado" and sexo = "M" and salario < 2000;
-- Questao E
select nome, cargo  from funcionario where salario between 6000 and 10000;
-- Questao F
select nome  from funcionario where sexo = "F" and salario > 10000 order by cargo asc;
-- Questao G
select nome, estadoCivil, salario from funcionario where data_nascimento > year("1974");
-- Questao H
select cpf, cargo, salario from funcionario where cidade = "Belo Horizonte" and salario <= 1700;