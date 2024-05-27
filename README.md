# app_receitas_api

## Script DB


CREATE TABLE IF NOT EXISTS Tbl_User (
id SERIAL PRIMARY KEY,
primeiro_nome TEXT NOT NULL,
ultimo_nome TEXT NOT NULL,
email TEXT NOT NULL,
password TEXT NOT NULL,
image_url TEXT
);

CREATE TABLE IF NOT EXISTS Tbl_Categoria (
id SERIAL PRIMARY KEY,
nome TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Tbl_Receita (
id SERIAL PRIMARY KEY,
title TEXT NOT NULL,
description TEXT NOT NULL,
ingredients TEXT NOT NULL,
materials TEXT NOT NULL,
instructions TEXT NOT NULL,
image_url TEXT NOT NULL,
id_user INT NOT NULL,
FOREIGN KEY (id_user) REFERENCES Tbl_User(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Tbl_Categoria_Receita (
id SERIAL PRIMARY KEY,
id_categoria INT NOT NULL,
id_receita INT NOT NULL,
FOREIGN KEY (id_categoria) REFERENCES Tbl_Categoria(id) ON DELETE CASCADE,
FOREIGN KEY (id_receita) REFERENCES Tbl_Receita(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Tbl_Avaliacao (
id SERIAL PRIMARY KEY,
value real NOT NULL,
message TEXT,
id_user INT NOT NULL,
FOREIGN KEY (id_user) REFERENCES Tbl_User(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Tbl_Avaliacao_Receita (
id SERIAL PRIMARY KEY,
id_avaliacao INT NOT NULL,
id_receita INT NOT NULL,
FOREIGN KEY (id_avaliacao) REFERENCES Tbl_Avaliacao(id) ON DELETE CASCADE,
FOREIGN KEY (id_receita) REFERENCES Tbl_Receita(id) ON DELETE CASCADE
);




## Categorias 
INSERT INTO Tbl_Categoria (nome) VALUES 
('Aperitivos e Entradas'),
('Saladas'),
('Sopas'),
('Pratos Principais'),
('Massas'),
('Risotos'),
('Carnes'),
('Aves'),
('Peixes'),
('Frutos do Mar'),
('Vegetariano'),
('Vegano'),
('Sobremesas'),
('Bebidas'),
('Café da Manhã'),
('Lanches'),
('Pães e Bolos'),
('Molhos e Condimentos'),
('Comida Rápida'),
('Comida Saudável'),
('Comida de Festa'),
('Comida Confortável'),
('Culinária Italiana'),
('Culinária Japonesa'),
('Culinária Mexicana'),
('Culinária Indiana'),
('Culinária Tailandesa'),
('Culinária Francesa'),
('Culinária Chinesa'),
('Culinária Brasileira');


## DotENV COnnection String

CONNECTION_STRING=Host = localhost; Username= postgres;Port= 5432 ;Password = password; Database = dbname
