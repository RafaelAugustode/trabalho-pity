create database Apuração_Eleitoral;
use  Apuração_Eleitoral;

create  table candidatos(
id_candidato int not null auto_increment primary key,
nome varchar(55) not null,
partido varchar(55) not null,
telefone char(14) not null,
cidade varchar(50) not null,
cargo  varchar(30)  not null,
total_votos int not null,
id_votos int not null,
sigla_partido int not null,
id_eleicao int not null,
foreign key (id_votos) references votos(id_votos),
foreign key (sigla_partido) references partido(sigla),
foreign key (id_eleicao) references eleicao(id_eleicao)
);

create  table votos(
id_votos int primary key auto_increment not null,
data_votos date not null,
id_candidato int not null,
cpf_eleitor int not null,
foreign key (id_candidato) references candidato(id_candidato),
foreign key (cpf_eleitor) references  eleitor(cpf_eleitor)
);

create  table eleitores(
cpf_eleitor char(14) auto_increment primary key not null,
nome varchar(55) not null,
endereco varchar(65) not null,
telefone char(14) not null,
estado_civil varchar(40) not null,
sexo char(1) not null,
idade int not null,
titulo_eleitor varchar(40) not null,
id_votos int not null,
id_candidato int not null,
foreign key (id_candidato) references candidatos (id_candidato),
foreign key (id_votos) references votos(id_votos)
);

create  table resultados(
id int not null auto_increment primary key,
data_resultado date not null,
quant_votos_total int not null,
eleitos varchar(45) not null,
id_eleicao int not null,
foreign key (id_eleicao) references eleicao(id_eleicao)
);
create table eleicao(
id_eleicao int not null auto_increment primary key,
data_eleicao date not null,
candidato_preferido int not null,
id_candidato int not null,
id_resultados int not null,
foreign key (id_candidato) references candidato(id_candidato),
foreign key (id_resultados) references resultados(id)
);
create table partido(
sigla varchar(5) not null auto_increment primary key,
nome varchar(50) not null,
sede varchar(40) not null,
estado varchar(50) not null,
cidade varchar(40) not null,
rua varchar(40) not null,
id_candidato int not null,
foreign key (id_candidato) references candidato(id_candidato)
);
insert into candidatos(id_candidato, partido, telefone, cidade, cargo , total_votos ) 
values ("1",'PT','(31)93456-9312','bh','prefeito','200'),
("2",'PT','(31)91236-0982','BH','vereador','600'),
("3",'PL','(31)90856-2367','Betim','prefeito','700'),
("4",'PSDB','(31)98076-4326','Contagem','vereador','300'),
("5",'UNIAO','(31)97230-6579','bh','prefeito','400');

insert into votos(id_votos, data_votos) 
values ("1",'2008-03-02'),
("2",'2008-03-03'),
("3",'2008-03-04'),
("4",'2008-03-05'),
("5",'2008-03-06');

insert into eleitores(cpf_eleitor, nome,endereco , telefone, estado_civil, sexo , idade, titulo_eleitor) 
values ("123.567.890-12",'Rafael','Rua Zurick Nova Suíssa','(31)92859-1947','Casado','17','200 399 426'),
("278.529.219-90",'Gabriel','Rua Zurick Nova Suíssa','(31)99973-8943','Casado','23','600 524 336'),
("371.566.829-08",'Cardoso','Rua Cracilândia Coração Eucarístico','(31)98762-0982','Solteira','29','796 326 123'),
("437.819.732-40",'Samuel','Rua Afonso Padilha Nova Suíssa','(31)92675-4512','Casado','69','345 769 159'),
("549.726.619-33",'Caua','Rua Arnold Sh. bairro Barrero','(31)97359-2367','Viúvo','50','400 952 866');



insert into resultados(id, data_resultado, quant_votos_total, eleitos ) 
values ("1",'2024-10-27','400','Engler'),
("2",'2024-10-27','500','Nunes'),
("3",'2024-10-27','200','Gabriel'),
("4",'2024-10-27','150','Fuad'),
("5",'2024-10-27','30000','Marçal');



insert into eleicao(id_eleicao, data_eleicao, candidato_preferido ) 
values ("1",'2024-10-27','Marçal'),
("2",'2024-10-27','Fuad'),
("3",'2024-10-27','Adolfo'),
("4",'2024-10-27','Gabriel'),
("5",'2024-10-27','Nunes');


insert into partido(sigla, nome, sede, cidade, rua ) 
values ('PT','Partido Trabalhista','RJ','RJ','Rua Bardinerio 432'),
('PT','Partido Trabalhista','RJ','RJ','Rua Bardineiro 432'),
('PL','Partido Liberal','SP','SP','Rua Joaneiro 356'),
('PSDB','Partido da Social Democracia Brasileira','SP','SP','Rua Faveiro 540'),
('UNIAO','União Direita','MG','BH','Rua Zurick 860');

select partido, total_votos from candidatos;

select data_votos from votos;

select cpf_eleitor, nome from eleitores;

select eleitos from resultados;

select data_eleicao from eleicao;

select sigla, nome from partido;



select partido, total_votos from candidatos where partido like "P%";

select data_votos from votos where year(data_votos)  like 2008;

select cpf_eleitor, nome from eleitores where nome like "%l";

select eleitos from resultados where quant_votos_total like 30000;

select data_eleicao from eleicao where  candidato_preferido like "Adolfo";

select sigla, nome from partido where sigla like "P%";



select sigla_partido, nome  from candidatos join partido on sigla_partido = "P%";

select data_votos, data_resultado from votos join resultados;

select candidato, nome from eleitores as E join candidato as C on E.id_candidato = C.id_candidato;

select eleitos, nomes from resultados join candidatos ;

select data_eleicao, data_resultado from eleicao join resultados ;
