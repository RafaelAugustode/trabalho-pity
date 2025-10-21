
use sakila;
select 
concat(first_name, "" ,last_name) as NomeCompleto, 'Ativo' as Tipo_Cliente
from costumer
where active= 1
union
select
concat(first_name, "" ,last_name) as NomeCompleto, 'Inativo' as Tipo_Cliente, 0 
from costumer 
where
 active = 0;


use classicmodels;
select 
customerName as Nome_Cliente,employeeNumber
from customers
left join employees on (salesRepEmployeeNumber = EmployeeNumber)
where employeeNumber is null;

select productName as Nome_Produto from products  left join orderdetails using(productCode) where orderNumber is null;
select productName from orderdetails right join products using(productCode) where orderNumber is null;

use sakila;
select title as Titulo from film left join inventory using(film_id) where inventory_id is null;

use classicmodels;
select concat(firstName, " " ,lastName) as NomeCompleto, jobTitle from employees left join customers on (salesRepEmployeeNumber = EmployeeNumber) where jobTitle = 'Sales Rep' and salesRepEmployeeNumber is null;

