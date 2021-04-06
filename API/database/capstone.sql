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
	instructions nvarchar(4000) NOT NULL,
	type_id int NOT NULL,
	num_servings int NOT NULL,
	is_shared bit DEFAULT 0 NOT NULL --CHECK (is_shared = 0 or is_shared = 1),
	--shareable?
	CONSTRAINT PK_recipes PRIMARY KEY (recipe_id),
	CONSTRAINT FK_recipes_userid FOREIGN KEY (user_id) REFERENCES users (user_id),
	CONSTRAINT FK_recipes_type FOREIGN KEY (type_id) REFERENCES types (type_id),
	
)

CREATE TABLE recipes_ingredients(
	recipe_id int NOT NULL,
	ingredient_id int NOT NULL,
	ingredient_qty decimal NOT NULL,
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

--Insert Shared Recipes / Ingredients
INSERT INTO recipes (user_id, recipe_name, instructions, type_id, num_servings, is_shared) VALUES 
	(1, 'Peanut Butter and Jelly', 'Open peanut butter jar. |||Put peanut butter on one slice of bread. |||Open jelly jar, put jelly on another slice of bread. |||Put them together.', 3, 2, 1),
	(1, 'Brown Sugar Oatmeal Cookies', 'Preheat the oven to 350 degrees F. |||In the bowl of an electric mixer (or using a hand mixer), beat together the brown sugar and butter until fluffy. Beat in the vanilla. Add the eggs one at a time, scraping the bowl after each one. |||Mix together the flour, salt and baking soda in a medium bowl. Add it into the creamed mixture in 2 to 3 batches, mixing until just combined. Mix in the oats until just combined. |||Use your preferred size cookie scoop (or a regular spoon) to drop portions of dough onto baking sheets, spacing them a couple inches apart. Bake until dark and chewy, 12 to 13 minutes. If you''d like a crispier cookie, just cook a little longer! |||Let the cookies cool slightly on the baking sheets, then transfer onto a plate for serving.  |||Cook''s Note: Add 1/2 cup finely chopped nuts to the flour mixture if you''d like a nutty flavor and crunch.', 4, 24, 1),
	(1, 'Cinnamon Baked French Toast', 'For the French toast: Grease the baking pan with butter. Tear the bread into chunks, or cut into cubes, and evenly distribute in the pan. Crack the eggs in a big bowl. Whisk together the eggs, milk, cream, granulated sugar, first 1/2 cup of brown sugar, and vanilla. Pour evenly over the bread. Cover the pan tightly and store in the fridge until needed (overnight preferably). |||For the topping: Mix the flour, brown sugar, cinnamon, salt and some nutmeg in a separate bowl. Stir together using a fork. Add the butter and with a pastry cutter, and mix it all together until the mixture resembles fine pebbles. Store in a re-sealable plastic bag in the fridge. |||When you''re ready to bake the casserole, preheat the oven to 350 degrees F. Remove the casserole from the fridge and sprinkle the topping over the top. Bake for 45 minutes for a softer, more bread pudding texture or for 1 hour or more for a firmer, crisper texture. |||Scoop out individual portions. Top with butter and drizzle with warm pancake syrup and sprinkle with blueberries.', 1, 12, 1),
	(1, 'Wafflemaker Hash Browns', 'Preheat a waffle iron on the regular setting and spray both sides with cooking spray. |||Squeeze out any excess moisture from the hash browns and put in a bowl. Pour the melted butter over the hash browns, sprinkle with the salt and pepper and stir. Scoop a heaping 1/2 cup of the seasoned hash browns into each waffle section, then top with a generous 2 tablespoons Cheddar followed by a sprinkling of chopped ham. Top the cheese and ham in each section with another 1/4 cup hash browns. Close the waffle iron and cook for 15 minutes on the regular setting. |||Repeat with the remaining hash browns, cheese and ham, filling one section of the waffle iron.', 1, 5, 1),
	(1, 'Pan Fried Pork Chops', 'Salt and pepper both sides of the pork chops. |||Combine the flour and some cayenne, salt and black pepper. Dredge each side of the pork chops in the flour mixture, and then set aside on a plate. |||Heat the canola oil over medium to medium-high heat. Add the butter. When the butter is melted and the butter/oil mixture is hot, cook 3 pork chops at a time, 2 to 3 minutes on the first side. Flip and cook until the chops are golden brown on the other side, 1 to 2 minutes (make sure no pink juices remain). Remove to a plate and repeat with the remaining pork chops. |||Delicious and simple!', 1, 8, 1),
	(1, 'Chocolate Peanut Butter Pie', 'For the crust: Preheat the oven to 350 degrees F. Crush the cookies until they''re fine crumbs. Pour the melted butter over the top and stir with a fork to combine. Press into a pie pan and bake until set, 5 to 7 minutes. Remove from the oven and allow to cool completely. |||For the filling: Beat the peanut butter with the cream cheese until smooth. Add the powdered sugar and beat until smooth. Add in the thawed whipped topping and beat until smooth, scraping the sides as needed. |||Pour the filling into the crust, evening out the top with a knife or spatula. Chill for at least an hour before serving. |||Cook’s Note Warning: This is ultra, ultra-rich. Cut small slivers-your guests will thank you!',4,8,1)

INSERT INTO ingredients (ingredient_name)
	VALUES ('mozzerella cheese'),
    ('tomato'),
	('pepperoni'),
	('dough'),
	('peanut butter'),
	('jelly'),
	('bread'),
	('all-purpose flour'),--8
	('butter'),
	('sourdough'),
	('eggs'),
	('milk'),
	('whipping (heavy) cream'),
	('granulated sugar'),
	('brown sugar'),
	('vanilla extract'),
	('salt'),
	('freshly grated nutmeg'),
	('pancake syrup'),
	('blueberries'),
	('cinnamon'),
	('dark brown sugar'),
	('baking soda'),
	('old-fashioned oats'),
	('olive oil cooking spray'),
	('shredded hash browns'),
	('kosher salt'),
	('ground black pepper'),
	('cheddar cheese'),
	('chopped ham'),
	('pork chops'),
	('cayenne pepper'),
	('canola oil'),
	('oreos'),
	('cream cheese'),
	('powdered sugar'),
	('cool whip')

INSERT INTO recipes_ingredients (recipe_id, ingredient_id, ingredient_qty, ingredient_unit)
	VALUES 
	(6, (select ingredient_id from ingredients where ingredient_name = 'oreos'),25, ''),
	(6, (select ingredient_id from ingredients where ingredient_name = 'butter'),4, 'tablespoons'),
	(6, (select ingredient_id from ingredients where ingredient_name = 'peanut butter'),1, 'cup'),
	(6, (select ingredient_id from ingredients where ingredient_name = 'cream cheese'),1, '8-ounce package'),
	(6, (select ingredient_id from ingredients where ingredient_name = 'powdered sugar'),1.25, 'cups'),
	(6, (select ingredient_id from ingredients where ingredient_name = 'cool whip'),1, '8 ounces'),

	(5, (select ingredient_id from ingredients where ingredient_name = 'salt'),1, 'teaspoon'),
	(5, (select ingredient_id from ingredients where ingredient_name = 'ground black pepper'),1, 'teaspoon'),
	(5, (select ingredient_id from ingredients where ingredient_name = 'pork chops'),8, ''),
	(5, (select ingredient_id from ingredients where ingredient_name = 'all-purpose flour'),1, 'cup'),
	(5, (select ingredient_id from ingredients where ingredient_name = 'cayenne pepper'),1, 'pinch'),
	(5, (select ingredient_id from ingredients where ingredient_name = 'canola oil'),0.5, 'cups'),
	(5, (select ingredient_id from ingredients where ingredient_name = 'butter'),1, 'tablespoon'),
	(4, (select ingredient_id from ingredients where ingredient_name = 'olive oil cooking spray'),1, 'bottle'),
	(4, (select ingredient_id from ingredients where ingredient_name = 'shredded hash browns'),1, '30-ounce bag frozen'),
	(4, (select ingredient_id from ingredients where ingredient_name = 'butter'),0.5, 'sticks'),
	(4, (select ingredient_id from ingredients where ingredient_name = 'kosher salt'),1, 'teaspoon'),
	(4, (select ingredient_id from ingredients where ingredient_name = 'ground black pepper'),0.5, 'teaspoons'),
	(4, (select ingredient_id from ingredients where ingredient_name = 'cheddar cheese'),0.75, 'cups'),
	(4, (select ingredient_id from ingredients where ingredient_name = 'chopped ham'),0.75, 'cups'),
	(1, (select ingredient_id from ingredients where ingredient_name = 'dark brown sugar'),2, 'cups'),
	(1, (select ingredient_id from ingredients where ingredient_name = 'butter'),2, 'sticks'),
	(1, (select ingredient_id from ingredients where ingredient_name = 'vanilla extract'),2, 'teaspoons'),
	(1, (select ingredient_id from ingredients where ingredient_name = 'eggs'),2, 'eggs'),
	(1, (select ingredient_id from ingredients where ingredient_name = 'all-purpose flour'),1.5, 'cups'),
	(1, (select ingredient_id from ingredients where ingredient_name = 'salt'),1, 'teaspoon'),
	(1, (select ingredient_id from ingredients where ingredient_name = 'baking soda'),0.5, 'teaspoon'),
	(1, (select ingredient_id from ingredients where ingredient_name = 'old-fashioned oats'),3, 'cups'),
	(2, 5, 2, 'Oz'),
	(2, 6, 2, 'Oz'),
	(2,7, 2, 'Slices'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'butter'),1.1, 'sticks'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'sourdough'),1,'loaf'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'eggs'),8,'whole eggs'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'milk'),2,'cups'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'whipping (heavy) cream'),0.5,'cup'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'granulated sugar'),0.5,'cup'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'brown sugar'),1,'cups'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'vanilla extract'),2,'tablespoons'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'all-purpose flour'),0.5,'cup'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'cinnamon'),1,'teaspoon'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'salt'),0.25,'teaspoon'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'freshly grated nutmeg'),1,'pinch'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'pancake syrup'),1,'cup for serving'),
	(3,(select ingredient_id from ingredients where ingredient_name = 'blueberries'),1,'cup for serving')

GO