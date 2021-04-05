USE master
GO

--drop database if it exists
IF DB_ID('final_capstone') IS NOT NULL
BEGIN
	ALTER DATABASE final_capstone SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE final_capstone;
END

CREATE DATABASE final_capstone
GO

USE final_capstone
GO

--create tables
CREATE TABLE users (
	user_id int IDENTITY(1,1) NOT NULL,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	user_role varchar(50) NOT NULL
	CONSTRAINT PK_user PRIMARY KEY (user_id)
)

CREATE TABLE ingredients (
	ingredient_id int IDENTITY(1,1) NOT NULL,
	ingredient_name nvarchar(50) NOT NULL
	CONSTRAINT PK_ingredient PRIMARY KEY(ingredient_id)
)

--this is the pantry
CREATE TABLE users_ingredients(
	user_id int NOT NULL,
	ingredient_id int NOT NULL,
	ingredient_qty int NOT NULL,
	ingredient_unit varchar(50) NOT NULL,
	CONSTRAINT PK_ingredients_users PRIMARY KEY(ingredient_id, user_id),
	CONSTRAINT FK_UserIngredients_Ingredients FOREIGN KEY (ingredient_id) REFERENCES ingredients (ingredient_id),
	CONSTRAINT FK_UserIngredients_Users FOREIGN KEY (user_id) REFERENCES users (user_id)
)

CREATE TABLE types(
	type_id int IDENTITY(1,1) NOT NULL,
	type varchar(50) NOT NULL,
	CONSTRAINT PK_types PRIMARY KEY (type_id)
)

CREATE TABLE recipes (
	recipe_id int IDENTITY(1,1) NOT NULL,
	user_id int NOT NULL,
	recipe_name nvarchar(50) NOT NULL,
	instructions nvarchar(1000) NOT NULL,
	type_id int NOT NULL,
	num_servings int NOT NULL,
	--shareable?
	CONSTRAINT PK_recipes PRIMARY KEY (recipe_id),
	CONSTRAINT FK_recipes_userid FOREIGN KEY (user_id) REFERENCES users (user_id),
	CONSTRAINT FK_recipes_type FOREIGN KEY (type_id) REFERENCES types (type_id)
)

CREATE TABLE recipes_ingredients(
	recipe_id int NOT NULL,
	ingredient_id int NOT NULL,
	ingredient_qty int NOT NULL,
	ingredient_unit varchar(50) NOT NULL,
	CONSTRAINT PK_recipes_ingredients PRIMARY KEY (recipe_id, ingredient_id),
	CONSTRAINT FK_recipes_ingredients_ingredients FOREIGN KEY (ingredient_id) REFERENCES ingredients (ingredient_id),
	CONSTRAINT FK_recipes_ingredients_recipes FOREIGN KEY (recipe_id) REFERENCES recipes (recipe_id)
)

CREATE TABLE recipes_users (
	recipe_id int NOT NULL,
	user_id int NOT NULL,
	CONSTRAINT PK_recipes_users PRIMARY KEY (user_id, recipe_id),
	CONSTRAINT FK_recipes_users_recipe FOREIGN KEY (recipe_id) REFERENCES recipes (recipe_id),
	CONSTRAINT FK_recipes_users_users FOREIGN KEY (user_id) REFERENCES users (user_id)
)

CREATE TABLE time_of_day (
	time_of_day_id int IDENTITY(1,1) NOT NULL,
	time_of_day_name varchar(50) NOT NULL,
	CONSTRAINT PK_time_of_day PRIMARY KEY (time_of_day_id),
)

CREATE TABLE meals (
	meal_id int IDENTITY(1,1) NOT NULL,
	meal_name nvarchar(50) NOT NULL,
	time_of_day_id int NOT NULL,
	CONSTRAINT PK_meals PRIMARY KEY (meal_id),
	CONSTRAINT FK_meals_TOD FOREIGN KEY (time_of_day_id) REFERENCES time_of_day (time_of_day_id)
)

CREATE TABLE meals_recipes (
	meal_id int NOT NULL,
	recipe_id int NOT NULL,
	CONSTRAINT PK_meals_recipes PRIMARY KEY (meal_id, recipe_id),
	CONSTRAINT FK_meals_recipes_meal FOREIGN KEY (meal_id) REFERENCES meals (meal_id),
	CONSTRAINT FK_meals_recipes_recipe FOREIGN KEY (recipe_id) REFERENCES recipes (recipe_id)
)

CREATE TABLE meal_plans (
	meal_plan_id int IDENTITY(1,1) NOT NULL,
	name nvarchar(50) NOT NULL,
	time_period varchar(50) NOT NULL,
	user_id int NOT NULL,
	CONSTRAINT PK_meal_plans PRIMARY KEY (meal_plan_id),
	CONSTRAINT FK_meal_plans_user FOREIGN KEY (user_id) REFERENCES users (user_id),
)

CREATE TABLE meal_plans_meals (
	meal_plan_id int NOT NULL,
	meal_id int NOT NULL,
	CONSTRAINT PK_meal_plans_meals PRIMARY KEY (meal_plan_id, meal_id),
	CONSTRAINT FK_meal_plans_meals_meals FOREIGN KEY (meal_id) REFERENCES meals (meal_id),
	CONSTRAINT FK_meal_plans_meals_meal_plans FOREIGN KEY (meal_plan_id) REFERENCES meal_plans (meal_plan_id)
)



--populate default data
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user');
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin');

--INSERT INTO TIME OF DAY BREAKFAST LUNCH DINNER DESERT SNACK, INSERT INTO TYPE MAIN DISH, SIDE DISH, BEVERAGE, DESSERT, APPETIZER,
INSERT INTO types (type) VALUES ('Main Dish'), ('Side Dish'), ('Beverage'), ('Dessert'), ('Appetizer');
INSERT INTO time_of_day (time_of_day_name) VALUES ('Breakfast'), ('Lunch'), ('Dinner'),('Snack');

GO