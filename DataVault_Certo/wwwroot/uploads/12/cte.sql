 select * from media;
 select * from Compras;
 update Compras set msrp = 1000 where productCode = 'S12_1108';
 
 /*create table copia
 select * from media;
 */
 select 
 customerName,
 customerNumber,
 phone,
 addressLine1
 from customers;
 
 with cte_cliente (Nome, Codigo, Phone, Endereco) as( 
 select 
 customerName,
 customerNumber,
 phone,
 addressLine1
 from customers
 where creditLimit > 50000
 ),
 CTE_pagamento(codigo, valor, datapagamento) as(
 
 select customernumber,
 amount,
 paymentDate
 from payments
 where year(paymentDate) in (2003)
 )
 
select * from cte_cliente join CTE_pagamento using(codigo);