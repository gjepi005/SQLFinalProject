CREATE TABLE Category (  

ID SERIAL PRIMARY KEY,  

Name VARCHAR(50) UNIQUE  

); 

 

CREATE TABLE Recipe (  

ID SERIAL PRIMARY KEY,  

Name VARCHAR(50),  

Category_ID INT,  

FOREIGN KEY (Category_ID) REFERENCES Category(ID)  

); 

 

 

 

CREATE TABLE Ingredient (  

ID SERIAL PRIMARY KEY,  

Name VARCHAR(50) UNIQUE  

); 

 

CREATE TABLE Recipe_Ingredient (  

Recipe_ID INT,  

Ingredient_ID INT,  

FOREIGN KEY (Recipe_ID) REFERENCES Recipe(ID) ON DELETE CASCADE,  

FOREIGN KEY (Ingredient_ID) REFERENCES Ingredient(ID) ON DELETE CASCADE,  

PRIMARY KEY (Recipe_ID, Ingredient_ID)  

); 