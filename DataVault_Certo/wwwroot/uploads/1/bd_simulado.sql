create database DataCenterInfra;
use DataCenterInfra;

create table Servidores(
id_servidor int primary key auto_increment not null,
nome_servidor varchar(50) not null,
ip_servidor varchar(15) not null,
sistema_operacional varchar(50) not null
);
create table Racks(
id_rack int primary key auto_increment not null,
nome_rack varchar(50) not null,
localizacao varchar(100) not null,
id_servidor int not null,
foreign key (id_servidor) references Servidores (id_servidor)
);
alter table Racks add capacidade_em_terabytes float not null;
alter table Servidores drop sistema_operacional;
ALTER TABLE Racks change nome_rack  identificador_rack varchar(50) not null;
alter table Servidores modify ip_servidor varchar(20) not null;
create table Equipamentos(
id_equipamento int primary key auto_increment not null,
nome_equipamento varchar(50) not null,
tipo_equipamento varchar(50) not null,
id_rack int not null,
foreign key (id_rack) references Racks (id_rack)
);
create table Redes(
id_rede int primary key auto_increment not null,
nome_rede varchar(50) not null,
subnet varchar(50) not null,
gateway varchar(15) not null,
id_servidor int not null,
foreign key (id_servidor) references Servidores (id_servidor)
);
create table Logsatv (
id_log int primary key auto_increment not null,
descricao varchar(255) not null,
data_hora datetime not null,
gateway varchar(15) not null,
id_servidor int not null,
id_rede int not null,
foreign key (id_rede) references Redes (id_rede),
foreign key (id_servidor) references Servidores (id_servidor)
);
