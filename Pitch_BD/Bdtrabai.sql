create table usuario(
id int not null auto_increment primary key,
nome_cliente varchar(45) not null,
senha varchar(45) not null,
email varchar(45) not null
);

create table feedback(
id int not null auto_increment primary key,
mensagem varchar(45) not null,
selecionar_nivel varchar(45) not null,
id_usuario int not null,
foreign key (id_usuario) references usuario(id)
);

create table pagamento(
id int not null auto_increment primary key,
pix varchar(45) not null,
extrato varchar(45) not null,
catao_debito bigint not null,
cartao_credito bigint not null,
id_usuario int not null,
foreign key (id_usuario) references usuario(id)
);

create table perfil(
id int not null auto_increment primary key,
modo_site varchar(45) not null,
lingua varchar(45) not null,
historico_mensagens varchar(45) not null,
id_usuario int not null,
foreign key (id_usuario) references usuario(id)
);

create table file(
id int  not null auto_increment primary key,
arquivo_file bool not null auto_increment,
nome_arquivo varchar(45) not null,
favorito bool not null,
compartilhado bool not null,
excluido bool not null,
tipo_arquivo varchar(45) not null,
tamanho_arquivo bigint not null,
nome_pasta varchar(45) not null,
id_usuario int not null,
foreign key (id_usuario) references usuario(id)
)