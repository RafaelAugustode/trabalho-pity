create database formula1;
use formula1;
create table gp(
codigo int unsigned not null,
circuito varchar(60) null,
pais smallint unsigned null,
primary key (codigo)
);
create table equipe(
id int unsigned not null primary key,
nome varchar(60) null,
motor  varchar(60) null,
pais smallint unsigned null,
pontuacao mediumint unsigned null
);
create table pais(
id smallint unsigned not null auto_increment primary key,
nome varchar(80) null,
sigla varchar(5) null
);
create table pontuacao (
posicao tinyint unsigned not null primary key,
pontos tinyint unsigned null,
pontos2 tinyint unsigned null
);
create table piloto(
id int unsigned not null auto_increment primary key,
nome varchar(80) null,
pais smallint unsigned null,
carro tinyint unsigned null,
pontuacao mediumint unsigned null
);
create table campeonato (
gp_codigo int unsigned not null,
equipe_id int unsigned not null,
primary key (gp_codigo, equipe_id),
foreign key (gp_codigo) references gp(codigo),
foreign key (equipe_id) references equipe(id)
);
create table classificacao(
gp_codigo int unsigned not null,
piloto_id int unsigned not null,
posicao tinyint unsigned null primary key,
foreign key (gp_codigo) references gp(codigo),
foreign key (piloto_id) references piloto(id)
);
create table contrato(
piloto_id int unsigned not null,
equipe_id int unsigned not null,
primary key(piloto_id, equipe_id),
foreign key(piloto_id) references piloto(id),
foreign key (equipe_id) references equipe(id)
);
create table carros(
equipe_id int unsigned not null,
numero tinyint unsigned not null,
foreign key(equipe_id) references equipe(id)
);
select * from pais;
select nome,pontuacao from piloto;
select nome, numero from equipe join carros on equipe.id = carros.equipe_id;
select * from pilotos where pontuacao < 10;
insert into pais(id, nome, sigla) values (null, 'Brasil', 'BRA');
delete from pontuacao where posicao > 10;
select avg(pontuacao) from piloto;
select count(*) from pais;
delimiter | 
create procedure sp_lista_paises() 
begin 
select * from pais; 
end;
| 
delimiter ; call sp_lista_paises();
delimiter | 
create procedure sp_novo_pais(in nomepais varchar(80),
siglapais varchar(5)) 
begin 
insert into pais(id, nome, sigla)
values (null, nomepais, siglapais);
end;
| 
delimiter ; 
call sp_novo_pais('Brasil', 'BRA');
delimiter | 
create procedure sp_muda_pontos(in pilotonome varchar(80),
pilotopontos mediumint) 
begin 
update piloto set pontuacao = pilotopontos where nome = pilotonome;
end;
| 
delimiter ; 
call sp_muda_pontos('Antonio José', 20);
delimiter | 
create trigger trg_del_piloto before delete on piloto
for each row 
begin 
delete from contrato where piloto_id = OLD.id;
end;
| 
delimiter ; 
delete from piloto where id = 123;

delimiter | 
create trigger trg_ins_equipe after insert on equipe
for each row 
begin 
insert into carros(equipe_id, numero)
values (NEW.id, 0);
end;
| 
delimiter ; 
insert into equipe(id, nome, motor, pais, pontuação)
values (null, 'Williams', 'Mercedes', 5, 0);