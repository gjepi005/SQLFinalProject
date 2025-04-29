using Microsoft.EntityFrameworkCore;
using RecipeDatabaseApp.Entities;


namespace RecipeDatabaseApp.Controllers
{
    public class RecipeController
    {
        // Update the DbContext to match your dbContext, e.g. WebStoreContext
        private readonly LopputehtäväContext _dbContext;

        // Update the DbContext to match your dbContext, e.g. WebStoreContext
        public RecipeController(LopputehtäväContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Retrieves all recipes from the database and prints them to the console.
        /// Implementation should use EF Core to fetch Recipe entities
        /// and display relevant fields (e.g., ID, Name).
        /// </summary>
        public async Task ListAllRecipes()
        {
            Console.WriteLine("======= PRINT ALL RECIPES =======\n");
            // Gather all recipes from Database
            var recipes = await _dbContext.Recipes.ToListAsync();

            if (recipes == null || recipes.Count == 0)
            {
                Console.WriteLine("No recipes were found.");
            }

            foreach (var recipe in recipes)
            {
                Console.WriteLine($"ID: {recipe.Id}, Name: {recipe.Name}");
            }
        }

        /// <summary>
        /// Associates an existing Category with a specified Recipe,
        /// based on user input (e.g., recipe ID/name and category name).
        /// The method should validate that both Recipe and Category
        /// exist, then create the necessary relationship in the database.
        /// </summary>
        internal async Task AddCategoryToRecipe()
        {
            Console.Clear();
            // List all categories
            await ListAllCategories();

            // Ask user for category name
            Console.Write("Give category name: ");
            string categoryName = Console.ReadLine();
            if (!string.IsNullOrEmpty(categoryName))
            { // Find max ID and add 1
                int id = _dbContext.Categories.Max(x => x.Id) + 1;

                // Create new entity
                _dbContext.Categories.Add(new Category
                {
                    Id = id,
                    Name = categoryName,
                });

                // Save the changes to the database
                _dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("Category name cannot be empty!");
            }
        }

        /// <summary>
        /// Allows the user to add a new Ingredient to the database,
        /// specifying properties such as Name, Type, and any optional
        /// nutritional details. Should use EF Core to create and
        /// save the new Ingredient entity.
        /// </summary>

        internal async Task AddNewIngredient(string ing)
        {
            bool exists = _dbContext.Ingredients
    .Any(x => x.Name.ToLower() == ing.ToLower());
            if (!exists)
            {
                _dbContext.Ingredients.Add(new Ingredient
                {
                    Name = ing
                });
                Console.WriteLine("Ingredient Added!");
                _dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("Ingredient already exists!");
            }
        }

        /// <summary>
        /// Creates a new Recipe entry by prompting the user for
        /// recipe details (name, description, etc.). 
        /// Implementation should add a new Recipe entity via EF Core
        /// and save changes to the database.
        /// </summary>
        internal async Task AddNewRecipe()
        {
            Console.Write("Enter recipe name: ");
            string recipeName = Console.ReadLine();

            // Pyydetään kategoria
            Console.Write("Enter category name (or leave blank): ");
            string categoryName = Console.ReadLine();

            // Hakee olemassa olevan kategorian (tai luo uuden jos ei löydy)
            Category category = null;
            if (!string.IsNullOrEmpty(categoryName))
            {
                category = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

                if (category == null)
                {
                    // Jos kategoriaa ei ole, voidaan luoda uusi
                    category = new Category { Name = categoryName };
                    await _dbContext.Categories.AddAsync(category);
                }
            }

            // Pyydetään ainesosien nimet (eroteltu pilkulla)
            Console.Write("Enter ingredients (comma separated): ");
            string ingredientsInput = Console.ReadLine();
            var ingredientNames = ingredientsInput.Split(',').Select(i => i.Trim().ToLower()).ToList();

            // Haetaan olemassa olevat ainesosat tietokannasta
            var existingIngredients = await _dbContext.Ingredients
                .Where(i => ingredientNames.Contains(i.Name.ToLower()))
                .ToListAsync();

            // Lisätään uudet ainesosat, joita ei löydy tietokannasta
            var newIngredients = ingredientNames
                .Where(name => !existingIngredients.Any(i => i.Name.ToLower() == name))
                .Select(name => new Ingredient { Name = name })
                .ToList();

            if (newIngredients.Any())
            {
                // Lisätään uudet ainesosat tietokantaan
                await _dbContext.Ingredients.AddRangeAsync(newIngredients);
                await _dbContext.SaveChangesAsync();

                existingIngredients = await _dbContext.Ingredients
                    .Where(i => ingredientNames.Contains(i.Name.ToLower()))
                    .ToListAsync();
            }

            // 7. Luodaan uusi resepti
            var newRecipe = new Recipe
            {
                Name = recipeName,
                Category = category  // Liitetään kategoria, jos valittiin
            };

            // 8. Liitetään ainesosat reseptiin
            newRecipe.Ingredients = existingIngredients;

            // 9. Lisätään resepti tietokantaan
            await _dbContext.Recipes.AddAsync(newRecipe);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine($"Recipe '{recipeName}' has been added successfully!");
        }


        /// <summary>
        /// Removes an existing Recipe from the database by prompting
        /// the user for a Recipe identifier (e.g., ID or name).
        /// Should handle deletion of any related data (e.g., from
        /// RecipeIngredient junction tables) if cascades are not enabled.
        /// </summary>
        internal async Task DeleteRecipe()
        {
            Console.Clear();
            var recipeList = await _dbContext.Recipes.ToListAsync();
            await ListAllRecipes();

            Console.WriteLine("Give recipe ID:");

            int.TryParse(Console.ReadLine(), out int result);
            var deleteRecipe = recipeList.Where(x => x.Id == result).FirstOrDefault();

            if (deleteRecipe != null)
            {
                _dbContext.Recipes.Remove(deleteRecipe);
                _dbContext.SaveChanges();
                Console.WriteLine("Recipe deleted successfully!");
            }
            else
            {
                Console.WriteLine("ID not found!");
            }

        }

        /// <summary>
        /// Fetches all recipes under a specified category by prompting
        /// the user for the category name. Uses EF Core and LINQ
        /// to filter recipes belonging to that category, then prints 
        /// them to the console.
        /// </summary>
        internal async Task FetchRecipeByCategory()
        {
            Console.Write("Kirjoita kategoria: ");
            string category = Console.ReadLine().ToLower(); ;

            var fetchedRecipes = await _dbContext.Recipes
                .Include(r => r.Category)
                .Where(r => r.Category.Name.ToLower() == category)
                .ToListAsync();

            if (fetchedRecipes == null || fetchedRecipes.Count == 0)
            {
                Console.WriteLine("No recipes were found in this category.");
                return;
            }
            else
            {
                foreach (var item in fetchedRecipes)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Category: {item.Category.Name}");
                }
            }
        }

        /// <summary>
        /// Removes a given Category association from a Recipe.
        /// The method should confirm both entities exist, then remove
        /// their relationship in the junction table or foreign key.
        /// </summary>
        internal async Task RemoveCategoryFromRecipe()
        {

            // List all categories
            await ListAllCategories();

            // Ask for id
            Console.Write("Kirjoita haluamasi ID: ");
            int id = int.Parse(Console.ReadLine());
            var itemToDelete = _dbContext.Categories.Where(x => x.Id == id).FirstOrDefault();

            if (itemToDelete == null)
            {
                Console.WriteLine("ID not found!");
                return;
            }
            else
            {
                _dbContext.Categories.Remove(itemToDelete);
                _dbContext.SaveChanges();
                Console.WriteLine("Category deleted successfully!");
            }
        }

        internal async Task ListAllCategories()
        {

            var categories = await _dbContext.Categories
                .ToListAsync();

            if (categories == null || categories.Count == 0)
            {
                Console.WriteLine("No categories were found.");
                return;
            }
            else
            {
                foreach (var item in categories)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.Name}");
                }
            }
        }

        /// <summary>
        /// Searches for recipes containing all of the user-specified
        /// ingredients. The user can input multiple ingredient names;
        /// the method should return only recipes that include
        /// all those ingredients.
        /// </summary>
        internal async Task SearchRecipeByIngredients()

        {
            Console.Clear();
            Console.WriteLine("Syötä halutut ainesosat, erottele useat ainesosat pilkuilla.");
            var recipes = await _dbContext.Recipes.ToListAsync();
            string wantedIngredientsInput = Console.ReadLine();

            var wantedIngredients = wantedIngredientsInput
      .Split(',', StringSplitOptions.RemoveEmptyEntries)
      .Select(i => i.Trim().ToLower()) // kirjaimet muutetaan pieneksi.
      .ToList();

            recipes = await _dbContext.Recipes
    .Include(r => r.Ingredients)
    .ToListAsync();

            var matchingRecipes = recipes
    .Where(recipe => wantedIngredients
        .All(wanted => recipe.Ingredients
            .Any(ingredient => ingredient.Name.ToLower().Contains(wanted))))
    .ToList();

            if (matchingRecipes.Any())
            {
                Console.WriteLine("\nFound Recipes:");
                foreach (var recipe in matchingRecipes)
                {
                    Console.WriteLine($"- {recipe.Name}");
                }
            }
            else
            {
                Console.WriteLine("\nNo recipes found with the given ingredients.");
            }
        }

        /// <summary>
        /// Updates fields of an existing Recipe, e.g., Name, Description,
        /// or other metadata. Prompts the user for a Recipe identifier,
        /// retrieves the entity from the database, modifies fields,
        /// and saves changes back to the database.
        /// </summary>
        internal async Task UpdateRecipe()
        {

            Console.Clear();
            Console.WriteLine("What recipe do you want to update?\n");

            var recipes = await _dbContext.Recipes.ToListAsync();

            if (recipes == null || recipes.Count == 0)
            {
                Console.WriteLine("No recipes were found.");
            }
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"Id: {recipe.Id}, Name: {recipe.Name}");
            }

            Console.WriteLine("\nEnter Recipe ID: ");
            if(!int.TryParse(Console.ReadLine(), out int userInput))
            {
                Console.WriteLine("Invalid ID entered.");
                return;
            }
            var existingRecipes = await _dbContext.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == userInput);

            if (existingRecipes == null)
            {
                Console.WriteLine("Recipe not found");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Updating Recipe: {existingRecipes.Name}");

            Console.WriteLine($"\nEnter new name (leave blank to keep current): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                existingRecipes.Name = newName;

            //TODO Category
            Console.WriteLine("\nAvailable categories:");
            var categories = await _dbContext.Categories.ToListAsync();
            foreach (var category in categories)
            {
                Console.WriteLine($"Id: {category.Id}, Name: {category.Name}");
            }
            Console.WriteLine("\nEnter new category ID (leave blank to keep current): ");
            var newCategoryInput = Console.ReadLine();

            if(!string.IsNullOrWhiteSpace(newCategoryInput) && int.TryParse(newCategoryInput, out int newCategoryId))
            {
                var newCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == newCategoryId);
                if(newCategory != null)
                {
                    existingRecipes.Category = newCategory;
                }
                else
                {
                    Console.WriteLine("Category not found. Keeping the old category.");
                }
            }


            //TODO Ingredients
            Console.WriteLine("\nDo you want to update ingredients? (y/n)");
            var updateIngredients = Console.ReadLine()?.ToLower();

            if(updateIngredients == "y")
            {
                foreach(var ingredient in existingRecipes.Ingredients)
                {
                    Console.WriteLine($"\n Current Ingredient: {ingredient.Name}");

        }

    }
}
