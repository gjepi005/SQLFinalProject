using Microsoft.EntityFrameworkCore;
using RecipeDatabaseApp.Entities;
using System.Xml.XPath;
using static System.Formats.Asn1.AsnWriter;

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
            // Ask user for category name
            Console.Write("Give category name: ");
            string categoryName = Console.ReadLine();

            // Find max ID and add 1
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

           /// <summary>
           /// Allows the user to add a new Ingredient to the database,
           /// specifying properties such as Name, Type, and any optional
           /// nutritional details. Should use EF Core to create and
           /// save the new Ingredient entity.
           /// </summary>
           internal async Task AddNewIngredient()
           {
            _dbContext.Ingredients.Add(new Ingredient
            {
                Id = 100,
                Name = "Mutsis :D",
            });

            _dbContext.SaveChanges();

            Console.Write("XD");
           }

        internal async Task AddNewIngredient(string ing)
        {
            _dbContext.Ingredients.Add(new Ingredient
            {
                Name = ing
            });
            Console.WriteLine("TOIMII");
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Creates a new Recipe entry by prompting the user for
        /// recipe details (name, description, etc.). 
        /// Implementation should add a new Recipe entity via EF Core
        /// and save changes to the database.
        /// </summary>
        internal async Task AddNewRecipe()
        {
            var ingredients = await _dbContext.Ingredients.ToListAsync();
            List<string> Inputingredients = new List<string>();

            Console.WriteLine("Recipe Name:");
            string name = Console.ReadLine();
            while (true)
            {
                Console.WriteLine("Press 0 to continue");
                Console.WriteLine("Give ingredient");
                string input = Console.ReadLine();
                if (input == "0")
                {
                    break;
                }
                Inputingredients.Add(input);

            }

            var existingNames = _dbContext.Ingredients
                .Select(i => i.Name)
                .ToHashSet();

            var newIngredients = Inputingredients
                .Where(i => !existingNames.Contains(i.ToString()))
                .ToList();

            foreach (string ing in newIngredients)
            {
                Console.WriteLine("ADDED NEW INGREDIENT");
                await AddNewIngredient(ing);
            }

            //_dbContext.Recipes.Add(new Recipe
            //{
            //    Id = 100,
            //    Name = name,

            //});
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

            while (true)
            {
                Console.WriteLine("Press 0 to go back to the menu");
                Console.WriteLine("Give recipe ID:");
                int.TryParse(Console.ReadLine(), out int result);

                if(result == 0)
                {
                    break;
                }
                var deleteRecipe = recipeList.Where(x => x.Id == result).FirstOrDefault();
              
                 _dbContext.Recipes.Remove(deleteRecipe);
                 _dbContext.SaveChanges();
            }
           await ListAllRecipes();
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
                string category = Console.ReadLine();

                var fetchedRecipes = await _dbContext.Recipes
                    .Include(r => r.Category)
                    .Where(r => r.Category.Name == category)
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
            ListAllCategories();    

                Console.Write("Kirjoita haluamasi ID: ");
                int id = int.Parse(Console.ReadLine());

                var itemToDelete = _dbContext.Categories.Where(x => x.Id == id).FirstOrDefault();

                _dbContext.Categories.Remove(itemToDelete);
                _dbContext.SaveChanges();
           }

        internal async Task ListAllCategories()
        {
            var categories = await _dbContext.Categories
                .ToListAsync();

            if (categories == null || categories.Count == 0)
            {
                Console.WriteLine("No recipes were found in this category.");
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
               throw new NotImplementedException();
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

            string userInput = Console.ReadLine();

            throw new NotImplementedException();
           }
     
    }
}