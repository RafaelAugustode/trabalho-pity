use classicmodels;

create or replace view view_faturamento(Faturamento, Quantidade_Empregados, Office)
as
select sum(amount),count(employeeNumber), officeCode
from offices join employees using(officeCode)
join customers on(salesRepEmployeeNumber = employeeNumber)
join payments using(customerNumber)
where offices.country = "USA" and Month(paymentDate) between 4 and 6  and Year(paymentDate) = 2004
group by officeCode;
select * from view_faturamento;

create or replace view view_faturamento_letra_B(Codigo_Cliente, Faturamento, Quantidade_Empregados, Office)
as
select customerNumber, sum(amount),count(employeeNumber), officeCode
from offices join employees using(officeCode)
join customers on(salesRepEmployeeNumber = employeeNumber)
join payments using(customerNumber)
where offices.country = "USA" and Month(paymentDate) between 4 and 6  and Year(paymentDate) = 2004
group by officeCode;

with cte_total(Total_recebido, Total_pedidos) 
as(
select Faturamento, count(orderNumber)
from view_faturamento_letra_B join orders on(Codigo_Cliente = customerNumber)
group by Office
)
select * from cte_total;

-- Questao C
create or replace view view_questaoC(cidade, codigo_produto, media_do_preco_unitario, preco_sugerido_pela_Fabrica, percentual)
as(
select offices.city, productCode, avg(priceEach), MSRP, FORMAT((1-FORMAT(AVG(PRICEEACH),2)/MSRP) * 100,2)
from productlines join products using(productLine)
join orderdetails using(productCode)
join orders using(orderNumber)
join customers using(customerNumber)
join employees on(salesRepEmployeeNumber = employeeNumber)
join offices using(officeCode)
where productLine = "Motorcycles"
group by offices.city
);
select * from view_questaoC;

create table Analise_Perda(
id int primary key auto_increment not null,
escritorio varchar(30) not null,
produto varchar(10) not null,
OBSERVACAO VARCHAR(200) not null
);

delimiter %
create procedure pr_percentual()
begin 
declare total_registros int default 0;
declare var_contador int default 0;
declare var_calculo float;
declare var_produto varchar(50);
declare var_cidade varchar(50);
declare var_observacao varchar(50);

select ifnull(count(productCode),0) into total_registros from  products;
repeat

select percentual, cidade, codigo_produto  into var_calculo, var_cidade, var_produto 
from view_questaoC join products on(codigo_produto = productCode)
join orderdetails using(productCode)
group by cidade
limit 1 offset var_contador;

set var_observacao = Case
when var_calculo < 5 then "Aceitável"
when var_calculo >= 5 and  var_calculo <= 10 then "Atenção produtos com risco"
else "Solicitar reunião com os vendedores!"
end;

insert into Analise_Perda(escritorio, produto, OBSERVACAO)
values( var_cidade, var_produto , var_observacao);

set var_contador = var_contador +1;
until var_contador >= total_registros
end repeat;
end %
delimiter ;
call pr_percentual();
drop procedure  pr_percentual;
delete from Analise_Perda;
select * from Analise_Perda group by produto;

use sakila;

delimiter %
create procedure pr_loja(in store int)
begin
select country, sum(payment.amount) 
from  payment
join rental using(rental_id)
join customer on(customer.customer_id = rental.customer_id)
join address using(address_id)
join city using(city_id)
join country using(country_id)
where customer.store_id = store
group by country_id;
end %
delimiter ;
drop procedure pr_loja;
call pr_loja(1);

use world;
create table table_world
select sum(Percentage * Population)/100
from countrylanguage 
join country using(CountryCode)
where Region = "Polynesia"
group by country.Code;
select * from table_world;


