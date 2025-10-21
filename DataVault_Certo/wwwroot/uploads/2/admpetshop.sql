create database admpetshop;
use admpetshop;
create table cliente(
id int primary key not null auto_increment,
cpf char(14) not null,
nome_completo varchar(70) not null,
endereço varchar(80) not null,
email varchar(80) not null unique,
telefone char(14) not null
);
create table animal
(
tipo varchar(25) primary key auto_increment not null,
nome varchar(50) not null,
id_do_cliente int not null,
foreign key (id_do_cliente) references cliente (id),
dono_do_animal varchar(50) not null,
sexo char(1),
data_de_nascimento date not null
);
create table servicos
(
id int not null auto_increment primary key,
descricao varchar(50) not null,
valor_cobrado float not null
);
alter table animal add raca varchar(30)  not null;
alter table servico add tempo_medio int not null;
alter table cliente change telefone  celular  char(14) not null;
alter table cliente drop endereco;
alter table cliente add rua varchar(50) not null;
alter table cliente add número int not null;
alter table cliente add complemento varchar(50) not null;
alter table cliente add bairro varchar(50) not null;
alter table cliente add cidade varchar(50) not null;
alter table cliente add cep char(9) not null;