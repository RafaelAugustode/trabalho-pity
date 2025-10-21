use classicmodels;



delimiter $
create procedure p_produto()
begin
declare var_contador int default 0;



repeat 
if var_contador = 0 then
create table t_produto
select  productName
from products
limit 1
offset var_contador;
else
 insert into t_produto
select  productName
from products
limit 1
offset var_contador;
end if;


set var_contador = var_contador +1;

if var_contador = 10 then
select * from t_produto;
drop table t_produto;
end if;
until var_contador = 10
end repeat;



end $
delimiter ;

drop procedure p_produto;

call p_produto();