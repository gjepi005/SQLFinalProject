INSERT INTO Category (Name) VALUES ('Pääruoka');  

INSERT INTO Category (Name) VALUES ('Jälkiruoka');  

INSERT INTO Category (Name) VALUES ('Välipala'); 

 

-- RECIPE 

INSERT INTO Recipe (Name, Category_ID) VALUES ('Makaroonilaatikko', 1);  

INSERT INTO Recipe (Name, Category_ID) VALUES ('Mustikkapiirakka', 2);  

INSERT INTO Recipe (Name, Category_ID) VALUES ('Smoothie', 3); 

 

-- INGREDIENTS  

INSERT INTO Ingredient (Name) VALUES ( 

'Makarooni'),  

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

INSERT INTO Recipe_Ingredient (Recipe_ID, Ingredient_ID) VALUES  

(1, 1), -- Makarooni  

(1, 2), -- Jauheliha  

(1, 3), -- Kananmuna  

(1, 4); -- Maito 

 

-- Mustikkapiirakka  

INSERT INTO Recipe_Ingredient (Recipe_ID, Ingredient_ID) VALUES  

(2, 5), -- Mustikka  

(2, 6), -- Vehnäjauho  

(2, 3), -- Kananmuna  

(2, 7), -- Voi  

(2, 8); -- Sokeri 

-- Smoothie  

INSERT INTO Recipe_Ingredient (Recipe_ID, Ingredient_ID) VALUES  

(3, 8), -- Sokeri  

(3, 9), -- Banaani  

(3, 4); -- Maito 