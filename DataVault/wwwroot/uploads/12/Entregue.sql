use sakila;
select * from country inner join city using(Country_id);

select * from city inner join address using(city_id);
select * from address join customer using(address_id);
select * from customer join payment using(customer_id);
select * from payment join customer using(customer_id);
select * from film join language using(language_id);
select count(*) from film join language using(language_id) group by language.name;
select count(*), sum(amount) from payment join staff using (staff_id) join store using(store_id) group by store_id;
select count(*) from customer join store using(store_id) group by store_id;
select count(*), sum(amount), avg(amount), max(amount), min(amount) from payment join staff using (staff_id) join store using(store_id) group by store_id;
select count(*) from payment join customer using(customer_id) group by customer_id;
select count(*) from film join language using(language_id) where film.length between 100 and 150 group by language_id ;
select count(*) from payment join staff using (staff_id) join store using(store_id) where month(payment_date) between 8 and 9 group by store_id;
select count(*) from customer join store using(store_id) where last_name like "R%" group by store_id;
select * from city join address using(city_id) join customer using(customer_id);
select * from customer join payment using(customer_id) join rental using(customer_id);
select * from film join film_category using(film_id) join category using(category_id);
select * from actor join film_actor using(actor_id) join film using(film_id);
select count(*) from customer join city using(city_id) where month(create_date) = 2 group by city_id;
select count(*) from film join film_actor using(film_id) join actor using(actor_id) where rental_duration in(3,7) and length between 60 and 150 and replacement_cost > 12.00;
select sum(Film.repleacement_cost) from film join film_category using(film_id) join category using(category_id) where rental_duration in(3,7) and length between 60 and 150 and replacement_cost > 12.00;

