create database sinuca;
use sinuca;
create table sacola(
numero_bola int not null primary key auto_increment,
cor_bola varchar(40) not null,
quant_bolas int

) engine = innoDB;


delimiter $
create procedure inserir_bola_sacola(in param_numero int, in param_cor varchar(10), in param_quantidade int)
begin
  declare var__valor_atual int default 0;
  declare var_capacidade_sacola int default 200;
  declare var_quantidade_final_sacola int default 0;
  declare var_bola_existe_sacola int default 0;
  declare var_espaco_disponivel int default 0;
select ifnull(sum(quant_bolas),0) into var__valor_atual from sacola;

 set var_quantidade_final_sacola =var__valor_atual + param_quantidade;
 
 if var_quantidade_final_sacola <= var_capacidade_sacola then
 
 select ifnull(numero_bola,0) into var_bola_existe_sacola from sacola where numero_bola = param_numero;
 
 if var_bola_existe_sacola = 0 then
 
  insert into sacola(numero_bola, cor, qtde) values (param_numero, param_cor, param_quantidade);
  
  else
  
  call alterar_bolas_sacola(param_numero, param_cor, param_quantidade);
  end if;
  
  else
  
  set var_espaco_disponivel = var_capacidade_sacola - param_quantidade;

if var_espaco_disponivel > 0 then
  
  insert into  sacola (numero_bola, cor, qtde) values (param_numero, param_cor, var_espaco_disponivel);
  select concat('Foram inseridas a quantidade de: ', var_espaco_disponivel, ' bolas', ' e sobraram ', (param_quantidade - var_espaco_disponivel), 'bolas.');
  end if;
  
 end if;

end$

create procedure alterar_bolas_sacola(in param_numero int, in param_cor varchar(10), in param_quantidade int)
begin
 declare var__valor_atual int default 0;
  declare var_capacidade_sacola int default 200;
  declare var_quantidade_final_sacola int default 0;
  declare var_bola_existe_sacola int default 0;
  declare var_espaco_disponivel int default 0;
  
  
  
if param_numero = null then
select concat("Insira bolas, este valor: " , param_numero, "é inválido.");
elseif param_quantidade > var_capacidade_sacola then
select concat("Insira a quantidade de bolas corretamente, este valor: ", param_quantidade, "é inválido.");
elseif param_quantidade < 0 then 
  call deletar_bolas_sacola(param_numero, param_cor, param_quantidade);
elseif  param_quantidade > var_espaco_disponivel then
  update  sacola set  cor_bola = param_cor, quant_bolas = var_espaco_disponivel where numero_bola = param_numero;
  select concat("Restaram", param_quentidade - var_espaco_disponivel,"bolas");
else 
update  sacola set  cor_bola = param_cor, quant_bolas = param_quantidade where numero_bola = param_numero;
end if;
end$

create procedure deletar_bolas_sacola(in param_numero int, in param_cor varchar(40), in param_quantidade int)
begin
 declare var__valor_atual int default 0;
  declare var_capacidade_sacola int default 200;
  declare var_quantidade_final_sacola int default 0;
  declare var_bola_existe_sacola int default 0;
  declare var_espaco_disponivel int default 0;
   declare transformar int default 0;
  set transformar = param_quantidade * -1;
  if param_numero = null then
select concat("Insira bolas, este valor: " , param_numero, "é inválido.");
elseif param_quantidade > var_capacidade_sacola then
select concat("Insira a quantidade de bolas corretamente, este valor: ", param_quantidade, "é inválido.");
 elseif trnasformar > var__valor_atual then
 select concat("Números acimas", transformar);
 else
update  sacola set quant_bolas = (var__valor_atual - transformar)  where numero_bola = param_numero and cor_bola = param_cor;
end if;
end$
delimiter ;


 