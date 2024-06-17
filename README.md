# app_receitas_api

## Script DB
CREATE TABLE IF NOT EXISTS Users (
    id SERIAL PRIMARY KEY,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    email TEXT NOT NULL,
    password TEXT NOT NULL,
    image_url TEXT
);

CREATE TABLE IF NOT EXISTS Categories (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Recipes (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    description TEXT NOT NULL,
    instructions TEXT NOT NULL,
    image_url TEXT NOT NULL,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Ingredients (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Materials (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Favorites (
    id SERIAL PRIMARY KEY,
    recipe_id INT NOT NULL,
    user_id INT NOT NULL,
    FOREIGN KEY (recipe_id) REFERENCES Recipes(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Ratings (
    id SERIAL PRIMARY KEY,
    value REAL NOT NULL,
    message TEXT,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Ingredients_Recipe (
    id SERIAL PRIMARY KEY,
    ingredient_id INT NOT NULL,
    recipe_id INT NOT NULL,
    FOREIGN KEY ( ingredient_id) REFERENCES Ingredients(id) ON DELETE CASCADE,
    FOREIGN KEY (recipe_id) REFERENCES Recipes(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Materials_Recipe (
    id SERIAL PRIMARY KEY,
    material_id INT NOT NULL,
    recipe_id INT NOT NULL,
    FOREIGN KEY ( material_id) REFERENCES Materials(id) ON DELETE CASCADE,
    FOREIGN KEY (recipe_id) REFERENCES Recipes(id) ON DELETE CASCADE
);


CREATE TABLE IF NOT EXISTS Category_Recipe (
    id SERIAL PRIMARY KEY,
    category_id INT NOT NULL,
    recipe_id INT NOT NULL,
    FOREIGN KEY (category_id) REFERENCES Categories(id) ON DELETE CASCADE,
    FOREIGN KEY (recipe_id) REFERENCES Recipes(id) ON DELETE CASCADE
);



CREATE TABLE IF NOT EXISTS Rating_Recipe (
    id SERIAL PRIMARY KEY,
    rating_id INT NOT NULL,
    recipe_id INT NOT NULL,
    FOREIGN KEY (rating_id) REFERENCES Ratings(id) ON DELETE CASCADE,
    FOREIGN KEY (recipe_id) REFERENCES Recipes(id) ON DELETE CASCADE
);


INSERT INTO categories (name) VALUES 
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

select *from Recipes;

## Categorias 
INSERT INTO categories (name) VALUES 
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
