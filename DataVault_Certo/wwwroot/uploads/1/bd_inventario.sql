create database bd_inventario_2b1;
use bd_inventario_2b1;
create table fabricante
(
id_fabricante int not null primary key auto_increment,
nome varchar(45) not null,
sede varchar(45),
email varchar(65) not null
);
create table equipamento(
id_equipamento int not null primary key auto_increment,
modelo varchar(45) not null,
num_serie char(25) not null,
id_fabricante int,
foreign key (id_fabricante) references fabricante (id_fabricante)
);
alter table equipamento modify modelo char(150) not null;
alter table fabricante add endereco varchar(77) not null;
alter table fabricante drop sede;