use sakila;

create table log_modificacao(
idModificacao int not null primary key auto_increment,
modificacao text,
dataModificao datetime default current_timestamp,
tabela varchar(50)
)engine = InnoDB;

use sakila;

delimiter $

create procedure pr_inativos()
begin
    
    declare var_cliente int;
    declare var_filme int;
    declare preco_filme decimal(5,2);
    declare id_inventory int;
    declare id_rental int;
    declare staff_ida int;
    declare done_cliente boolean default false;
    declare done_filme boolean default false;
    declare min_duration int;
    declare duracao_minima int;
    
    declare cursor_cliente cursor for
        select customer_id
        from customer
        join payment using(customer_id)
        where active = 0
        group by customer_id
        having sum(amount) > 50
        limit 5;

  
    declare cursor_filme cursor for
        select f.film_id, f.rental_rate
        from rental r
        join inventory i using(inventory_id)
        join film f using(film_id)
        group by f.film_id
        order by count(r.rental_id) desc
        limit 5;


    declare continue handler for not found set done_cliente = true;

   
    select staff_id into staff_ida from staff limit 1;

 
    start transaction;

    
    open cursor_cliente;

    loop_clientes: loop
        fetch cursor_cliente into var_cliente;
        if done_cliente then 
            leave loop_clientes; 
        end if;

    
        update customer set active = 1 where customer_id = var_cliente;

        set done_filme = false;
        open cursor_filme;
        loop_filmes: loop
            fetch cursor_filme into var_filme, preco_filme;
            if done_filme then 
                leave loop_filmes; 
            end if;
SELECT MIN(rental_duration), MIN(rental_rate)
INTO duracao_minima, preco_filme
FROM film
WHERE film_id = var_filme;

            select inventory_id into id_inventory
            from inventory
            where film_id = var_filme
            and inventory_id not in (select inventory_id from rental where return_date is null)
            limit 1;

        
            if id_inventory is null then
                select inventory_id into id_inventory from inventory where film_id = var_filme limit 1;
            end if;
            
select store_id into @store_id from customer where customer_id = var_cliente;

select staff_id into staff_ida from staff where store_id = @store_id limit 1;
           
            insert into rental (rental_date, inventory_id, customer_id, staff_id)
            values (now(), id_inventory, var_cliente, staff_ida);

            set id_rental = last_insert_id();

           
            insert into payment (customer_id, staff_id, rental_id, amount, payment_date)
            values (var_cliente, staff_ida, id_rental, preco_filme, now());

            
            leave loop_filmes;
        end loop loop_filmes;

        close cursor_filme;
    end loop loop_clientes;

    close cursor_cliente;

    commit;
end$


create trigger trg_cliente after Update on customer for each row
begin
insert into log_modificacao(idModificacao, modificacao, tabela)
values(default, concat("O cliente ", new.customer_id ," saiu de inativo para ativo "), "customer");
end$

create trigger trg_pagamento after Insert on payment for each row
begin
insert into log_modificacao(idModificacao, modificacao, tabela)
values(default, concat("Foi incluso um novo pagamento de código", new.payment_id ,"feito pelo  cliente: ", new.customer_id), "payment");
end$

create trigger trg_aluguel after Insert on rental for each row
begin
insert into log_modificacao(idModificacao, modificacao, tabela)
values(default, concat("Foi incluso o novo aluguel de código", new.rental_id ," feito pelo  cliente: ", new.customer_id), "rental");
end$

delimiter ;


call pr_inativos();



select * from log_modificacao;

