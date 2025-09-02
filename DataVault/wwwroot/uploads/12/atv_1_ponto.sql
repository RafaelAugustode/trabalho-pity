use  classicmodels;

create table CREDITO_CLIENTE(
cliente varchar(50) primary key not null,
total float not null,
limite_de_credito  float not null,
 analise varchar(50) not null
);
create or replace view view_total_pago(Nome_Cliente, Numero_Cliente,total_pago, limite_credito) as
select customerName,customerNumber,sum(amount), creditLimit 
from payments 
join customers using(customerNumber)
where Year(paymentDate) in (2003, 2005) and creditLimit > 100000
group by customerNumber
order by customerNumber;

delimiter %
create procedure pc_cliente()
begin
declare total_registros int default 0;
declare var_contador int default 0;
declare var_Nome varchar(50);
declare var_total float ;
declare var_limite_credito float ;
declare var_analise varchar(50);
declare var_diferenca float;
select ifnull(count(*),0) into total_registros from  view_total_pago;
repeat
select Nome_Cliente, total_pago, limite_credito
into var_Nome, var_total, var_limite_credito
from view_total_pago
LIMIT 1 OFFSET var_contador;

set var_diferenca = var_limite_credito - var_total;

set var_analise = case
when var_diferenca < 0 then " Entrar em contato com o cliente"
else "Sugerir aumento de crédito"
end;

insert into CREDITO_CLIENTE(cliente, total, limite_de_credito, analise)
values (var_Nome, var_total, var_limite_credito, var_analise);

set var_contador = var_contador +1;
until var_contador >= total_registros
end repeat;

end%
delimiter ;
call pc_cliente();
select * from CREDITO_CLIENTE group by cliente;

-- Questão B

create table Analise_Lucro (
id int primary key auto_increment not null,
produto varchar(50)  not null,
media float not null,
analise varchar(50) not null
);

create or replace view view_total_produto(Produto, media) as
select productName,round(avg(((priceEach/buyPrice)-1)*100),2) 
from products 
join orderdetails using(productCode)
group by buyPrice;

delimiter %
create procedure pc_valor()
begin
declare total_registros int default 0;
declare var_contador int default 0;
declare var_produto varchar(50);
declare var_media float;
declare var_analise varchar(50);

select ifnull(count(productCode),0) into total_registros from  products;
repeat
select Produto, media
into var_produto, var_media
from view_total_produto
LIMIT 1 OFFSET var_contador;


set var_analise = case
when var_media < 30 then "Chamar o representante imediatamente"
when var_media < 50 then "Aumentar margem de lucro"
 when var_media < 100 then "Manter o valor"
 else  " Conceder mais 10% de desconto"
end;

insert into Analise_Lucro(produto, media , analise)
values (var_produto, var_media, var_analise);

set var_contador = var_contador +1;
until var_contador >= total_registros
end repeat;

end%
delimiter ;
drop procedure pc_valor;

call pc_valor();

delete from Analise_Lucro;

select produto, media , analise from Analise_Lucro order by produto desc;


