-- RECIPE
INSERT INTO Recipe (Name) VALUES ('Makaroonilaatikko');
INSERT INTO Recipe (Name) VALUES ('Mustikkapiirakka');
INSERT INTO Recipe (Name) VALUES ('Smoothie');

-- INGREDIENTS
INSERT INTO Ingredients (Name) VALUES 
('Makarooni'), 
('Jauheliha'), 
('Kananmuna'), 
('Maito'), 
('Mustikka'), 
('Vehnäjauho'), 
('Voi'), 
('Sokeri'), 
('Banaani');


-- RECIPE_INGREDIENTS
-- Makaroonilaatikko
INSERT INTO Recipe_ingredients (Recipe_ID, Ingredient_ID) VALUES
(1, 1), -- Makaroonilaatikko, Makarooni
(1,2), -- Makaroonilaatikko, Jauheliha
(1,3), -- Makaroonilaatikko, Kananmuna
(1,4), -- Makaroonilaatikko, Maito
(2,5), -- Mustikkapiirakka, Mustikka
(2,6), -- Mustikkapiirakka, Vehnäjauho
(2,3), -- Mustikkapiirakka, Kananmuna
(2,7), -- Mustikkapiirakka, Voi
(2,8), -- Mustikkapiirakka, Sokeri
(3,8), -- Smoothie, Sokeri
(3,9), -- Smoothie, Banaani
(3,4) -- Smoothie, Maito;

-- CATEGORY
INSERT INTO Category (Name, Recipe_ID) VALUES 
('Pääruoka', 1),
('Jälkiruoka', 2),
('Välipala', 3);
