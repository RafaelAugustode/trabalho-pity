 use classicmodels;

create or replace view pag51_a
as
select productCode,productName, quantityOrdered 
from products 
join orderdetails using(productCode) 
join orders using(orderNumber)
 where Year(orderDate) = 2003 
 order by quantityOrdered * priceEach 
 desc limit 20;
 
 select * from pag51_a;

create or replace view pag51_b
as
select avg(quantityOrdered * priceEach), max(quantityOrdered * priceEach), min(quantityOrdered * priceEach)  as minimo
from products 
join orderdetails using(productCode) 
join orders using(orderNumber) 
join customers using(customerNumber) 
join employees on(salesRepEmployeeNumber = employeeNumber) 
join offices using(officeCode) 
where Year(orderDate) = 2004 and 
offices.city = "Paris" 
group by Month(orderDate); 

create or replace view pag51_c
as
select quantityOrdered, requiredDate 
from orderdetails 
join orders using(orderNumber)  
where Month(requiredDate) between 9 and 12 
order by requiredDate;

create or replace view pag51_d
as
select count(customerNumber) 
from customers 
join employees on(salesRepEmployeeNumber = employeeNumber) 
join offices using(officeCode)
 group by officeCode 
 order by count(customerNumber) desc;

create or replace view pag51_e
as
select productsName 
from products 
join orderdetails using(productCode) 
join orders using(orderNumber) 
where Year(orderDate) = 2004 and 
Month(orderDate) between 1 and 6 
order by quantityOrdered * priceEach 
limit 15 ;

create or replace view pag51_f
as
select avg(quantityOrdered * priceEach), max(quantityOrdered * priceEach), min(quantityOrdered * priceEach) 
from products 
join orderdetails using(productCode) 
join orders using(orderNumber) 
join customers using(customerNumber) 
join employees on(salesRepEmployeeNumber = employeeNumber) 
join offices using(officeCode) 
where Year(orderDate) = 2003 and offices.city = "Tokyo"; 

create or replace view pag51_g
as
select min(quantityOrdered) as minimo
from products 
join orderdetails using(productCode) 
join orders using(orderNumber)
 where month(requiredDate) between 9 and 12  
 group by productCode 
 order by requiredDate asc;

create or replace view pag51_h
as
select count(customerNumber)
 from customers 
 join employees on(salesRepEmployeeNumber = employeeNumber)
 join offices using(officeCode) 
 group by officeCode 
 order by count(customerNumber) desc;
 
  select * from pag51_a;
 update pag51_a set productName = '2001 Ferrari Enzos' where productCode = 'S12_1108';
 delete from pag51_a where productCode = 'S12_1108';
 
  select * from pag51_b;
  update pag51_b set minimo = 643.90 where minimo = 643.80;
 delete from pag51_b where minimo = 643.90;
 
  select * from pag51_c;
  update pag51_c set quantityOrdered = 30 where orderNumber = 1;
 delete from pag51_c where quantityOrdered = 30;
 
  select * from pag51_d;
  update pag51_d set customerNumber = '8000' where customerNumber = 1;
 delete from pag51_d where customerNumber = 1;
 
  select * from pag51_e;
  update pag51_e set productName = '2001 Ferrari Endor' where productName = '2001 Ferrari Enzos';
 delete from pag51_e where productName = '2001 Ferrari Endor';
 
  select * from pag51_f;
  update pag51_f set minimo = 643.90 where minimo = 643.80;
 delete from pag51_f where minimo = 643.90;
 
  select * from pag51_g;
  update pag51_g set minimo = 643.90  where productCode = 'S12_1108';
 delete from pag51_g where minimo = 643.90;
 
  select * from pag51_h;
   update pag51_h set customerNumber = '9000' where customerNumber = 1;
 delete from pag51_h where customerNumber = 1;
 
 /*
 O erro se deve ao uso de funções de agrupação  como count, sum e entre outros pois não
 teria como subtituir valores de resultados que tem mais de uma coluna envolvida ou contas que somam valores independentes;
 
 */

