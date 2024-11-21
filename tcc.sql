create database db;

create table usuarios
(
	id integer identity primary key,
	nome varchar(20),
	senha varchar(60),
	permissao integer
);

create table estados
(
	id integer identity primary key,
	nome varchar(25),
	sigla varchar(2)
);

create table cidades
(
	id integer identity primary key,
	nome varchar(35),
	fk_estado integer not null,
	foreign key (fk_estado) references estados(id)
);

create table entregadores
(
	id integer identity primary key,
	nome varchar(20),
	sobrenome varchar(20),
	pix varchar(100),
	situacao bit,
	celular varchar(15)
);

create table entregas
(
	id integer identity primary key,
	nome varchar(20),
	valor money,
	situacao bit
);

create table entregador_entrega
(
	id integer identity primary key,
	fk_entregador integer not null,
	foreign key (fk_entregador) references entregadores(id),
	fk_entrega integer not null,
	foreign key (fk_entrega) references entregas(id)
);

create table pagamentos
(
	id integer identity primary key,
	adicional money,
	desconto money,
	adiantamento money,
	pago bit,
	notaFiscal bit,
	fk_cidade integer not null,
	foreign key (fk_cidade) references cidades(id),
	fk_entregador integer not null,
	foreign key (fk_entregador) references entregadores(id),
	periodo date,
	situacao bit
);

create table pagamento_entrega
(
	id integer identity primary key,
	quantidade integer,
	fk_entrega integer not null,
	foreign key (fk_entrega) references entregas(id),
	fk_pagamento integer not null,
	foreign key (fk_pagamento) references pagamentos(id),
	periodo date
);

