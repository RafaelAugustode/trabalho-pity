use sakila;
select * from country inner join city using(Country_id);
-- Pag 40 exercicio 2
select * from city inner join address using(city_id);
select * from address join customer using(address_id);
select * from customer join payment using(customer_id);
select * from payment join customer using(customer_id);
select * from film join language using(language_id);

-- Pag 41 exercicio 3
select count(*) from film join language using(language_id) group by language.language_id;
select count(*), sum(amount) from payment join staff using (staff_id) join store using(store_id) group by store_id;
select count(*) from customer join store using(store_id) group by store_id;
select count(*), sum(amount), avg(amount), max(amount), min(amount) from payment join staff using (staff_id) join store using(store_id) group by store_id;
select count(*) from payment join customer using(customer_id) group by customer_id;
select count(*) from film join language using(language_id) where film.length between 100 and 150 group by language_id ;
select count(*) from payment join staff using (staff_id) join store using(store_id) where month(payment_date) between 8 and 9 group by store_id;
select count(*) from customer join store using(store_id) where last_name like "R%" group by store_id;

-- Pag 42 exercicio 4
select * from city join address using(city_id) join customer using(address_id);
select * from customer join payment using(customer_id) join rental using(rental_id);
select * from film join film_category using(film_id) join category using(category_id);
select * from actor join film_actor using(actor_id) join film using(film_id);
select count(*) from customer join address using(address_id) join city using(city_id) where month(create_date) = 2 group by city_id;
select count(*) from film join film_actor using(film_id) join actor using(actor_id) where rental_duration in(3,7) and length between 60 and 150 and replacement_cost > 12.00;
select sum(replacement_cost) from film join film_category using(film_id) join category using(category_id) where rental_duration in(3,7) and length between 60 and 150 and replacement_cost > 12.00;

-- Pag 43 ex5
select * from city join address using(city_id) join customer using(address_id) join payment using(customer_id);
select * from store join staff using (store_id) join payment using(staff_id) join rental using(rental_id) join inventory using(inventory_id) join film using(film_id);
select *  from actor join film_actor using(actor_id) join film using(film_id) join inventory using(film_id) join rental using(inventory_id) join payment using(rental_id) join customer using (customer_id) join address using(address_id) join city using(city_id) join country using(country_id);
-- Pag 43 ex8
select * from category join film_category using(category_id) join film using(film_id) join inventory using(film_id) join store using(store_id) join staff using(store_id) join payment using(staff_id);
select * from country join city using(country_id) join address using(city_id) join customer using(address_id) join payment using(customer_id);