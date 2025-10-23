create database LojaBancoDados2;
use LojaBancoDados2;
create table Produto(
CodigoProduto int primary key not null auto_increment,
nome varchar(50) not null,
descricao varchar(500),
qtde_estoque int not null
);


create table Cliente (
CodigoCliente int not null primary key auto_increment,
nome varchar(50) not null,
email varchar(60) not null,
cpf varchar(15)
);

create table Pedido(
CodigoPedido int not null primary key auto_increment,
dataPedido datetime,
status varchar(50)
);


create table ItemPedido (
CodigoPedido int,
CodigoProduto int,
precoVenda float,
Qtde int,
foreign key (CodigoPedido) references pedido(CodigoPedido),
foreign key (CodigoProduto) references produto(CodigoProduto)
);


create table funcionario (
CodigoFuncionario int not null primary key auto_increment,
nome varchar(50),
funcao varchar(50),
cidade varchar(50)
);

create table Auditoria (
DataModificao datetime not null, 
nometabela varchar(50) not null,
historico varchar(50) not null 
);

delimiter $

create trigger Produto_insert after insert on Produto for each row 
begin 
insert into Auditoria values (now(),'Produto', concat('O produto ', new.nome ,' foi adicionado')); 
end $
create trigger Produto_update after update on Produto for
 each row begin 
 insert into Auditoria values (now(),'Produto', concat('O produto ', old.nome ,' foi alterado para ', new.nome)); 
 end $
create trigger Produto_delete before delete on Produto for each row
 begin 
 insert into Auditoria values (now(),'Produto', concat('O produto ', old.nome ,' foi deletado'));
 end $
create trigger Cliente_insert after insert on Cliente for each row 
begin 
insert into Auditoria values (now(),'Cliente', concat('O cliente ', new.nome ,' foi adicionado')); 
end $
create trigger Cliente_update after update on Cliente for each row 
begin 
insert into Auditoria values (now(),'Cliente', concat('O cliente ', old.nome ,' foi alterado para ', new.nome));
 end $
create trigger Cliente_delete before delete on Cliente for each row 
begin 
insert into Auditoria values (now(),'Cliente', concat('O cliente ', old.nome ,' foi deletado')); 
end $
create trigger Pedido_insert after insert on Pedido for each row 
begin 
insert into Auditoria values (now(),'Pedido', concat('Um novo pedido foi criado com status ', new.status));
 end $
create trigger Pedido_update after update on Pedido for each row
 begin 
 insert into Auditoria values (now(),'Pedido', concat('O pedido teve o status alterado de ', old.status ,' para ', new.status)); 
 end $
create trigger Pedido_delete before delete on Pedido for each row
 begin 
 insert into Auditoria values (now(),'Pedido', concat('O pedido com status ', old.status ,' foi deletado')); 
 end $
create trigger ItemPedido_insert after insert on ItemPedido for each row 
begin
 insert into Auditoria values (now(),'ItemPedido', concat('Foi adicionado um item com preço de venda ', new.PrecoVenda)); 
 end $
create trigger ItemPedido_update after update on ItemPedido for each row 
begin 
insert into Auditoria values (now(),'ItemPedido', concat('O item teve o preço alterado de ', old.PrecoVenda ,' para ', new.PrecoVenda)); 
end $
create trigger ItemPedido_delete before delete on ItemPedido for each row 
begin 
insert into Auditoria values (now(),'ItemPedido', concat('Um item com preço de venda ', old.PrecoVenda ,' foi deletado')); 
end $
create trigger Funcionario_insert after insert on Funcionario for each row
 begin insert into Auditoria values (now(),'Funcionario', concat('O funcionário ', new.nome ,' foi adicionado')); 
 end$
create trigger Funcionario_update after update on Funcionario for each row 
begin 
insert into Auditoria values (now(),'Funcionario', concat('O funcionário ', old.nome ,' foi alterado para ', new.nome)); 
end $
create trigger Funcionario_delete before delete on Funcionario for each row
 begin insert into Auditoria values (now(),'Funcionario', concat('O funcionário ', old.nome ,' foi deletado')); 
 end $

delimiter ;

delimiter $
create procedure proc_produto ( in var_nome varchar(50), in var_descricao varchar(50), in var_quant int)
begin
insert into produto(nome, descricao, qtde_estoque) values (var_nome, var_descricao, var_quant);
end$
delimiter ;


call proc_produto('Ronaldinho soccer', 'ronaldinho', 9); 
select * from produto;

delimiter $ 
create procedure proc_Cliente (in nome varchar(50), in email varchar(50), in cpf varchar(15))
begin 
insert into cliente(nome, email, cpf) values (nome,email,cpf);
end$
delimiter ;
call proc_Cliente('satoro_gojo','partidoaomeio@gmail','meio');
select * from cliente;

delimiter $ 
create procedure proc_Funcionario( in nome_var varchar(50), in funcao varchar(50), in cidade varchar(50))
begin 
insert into Funcionario(nome, funcao, cidade) values (nome_var,funcao,cidade);
end$
delimiter ;
call Proc_funcionario('Hornet','Saudade_da_Hornet','Eu_quero_jogar_silk_song');

select * from Funcionario;


delimiter $
create procedure fazer_pedido (in codigo_prod int, in codigo_client int, in CodigoFuncionario int ,in quantidade int, in preco_produto float)
begin
declare resposta varchar(50);
  declare done bool default false;
declare json_pedido_carrinho json;
declare codigo_pedido int;
declare var_prod int;
declare var_preco float;
declare var_nome varchar(50);
declare var_descricao varchar(50);
declare var_estoque int;
declare realidade int;
declare car_codigo int;
declare car_qtde int;
declare preco_venda float;

 declare cursor_produto cursor for select * from produto where CodigoProduto = codigo_prod;
 declare continue handler for not found  set done = true;
   start transaction;
  set json_pedido_carrinho = json_object(
    'Carrinho', json_object(
      'CodigoProduto', codigo_prod,
      'CodigoCliente', codigo_client,
      'CodigoFuncionario',CodigoFuncionario,
      'Qtde', quantidade  
    )
  );
   insert into Pedido( dataPedido, status) values(now(), "Pronto");
set car_codigo = cast(JSON_UNQUOTE(JSON_EXTRACT(json_pedido_carrinho, '$.Carrinho.CodigoProduto')) as unsigned);
set car_qtde = cast(JSON_UNQUOTE(JSON_EXTRACT(json_pedido_carrinho, '$.Carrinho.Qtde')) as unsigned);
 set codigo_pedido = last_insert_id();
 open cursor_produto;
 abrir_loop: loop
   fetch cursor_produto into var_prod, var_nome, var_descricao, var_estoque;
    if done then 
    leave abrir_loop;
    end if;
    set realidade = var_estoque - quantidade;
if  realidade >= 0 then 
update produto set qtde_estoque  = realidade where CodigoProduto = codigo_prod;
set preco_venda = preco_produto * quantidade ;
insert into ItemPedido values(codigo_pedido ,var_prod,preco_venda, quantidade );
set resposta = 'Compra efetuada com sucesso';
select resposta;
else
rollback;
set resposta = 'da n moco';
select resposta;
leave abrir_loop;
end if;
end loop;
commit;
end$
delimiter ;

select * from produto;
call fazer_pedido(2, 1, 1, 1,10);

select * from ItemPedido;
drop procedure fazer_pedido;



create or replace view prod_chefe as 
select produto.nome, produto.CodigoProduto, sum(itempedido.Qtde) as totalvendido, 
sum(itempedido.Qtde * itempedido.precoVenda) as Receitatotal from produto
inner join itempedido using (CodigoProduto)
group by produto.CodigoProduto
order by totalvendido desc;

select * from prod_chefe;
desc prod_chefe;


show tables;
show procedure status where Db ="lojabancodados2";
show triggers;
select * from prod_chefe;

SHOW CREATE PROCEDURE proc_produto;
SHOW CREATE PROCEDURE fazer_pedido;
SELECT * 
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_SCHEMA = 'lojabancodados2' and SPECIFIC_NAME = "proc_funcionario";

desc ItemPedido;

