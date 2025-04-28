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
               // Print out all Recipes
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
               throw new NotImplementedException();
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
                RecipeId = null
            });

            _dbContext.SaveChanges();

            Console.Write("XD");
           }

           /// <summary>
           /// Creates a new Recipe entry by prompting the user for
           /// recipe details (name, description, etc.). 
           /// Implementation should add a new Recipe entity via EF Core
           /// and save changes to the database.
           /// </summary>
           internal async Task AddNewRecipe()
           {
               throw new NotImplementedException();
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
               throw new NotImplementedException();
           }

           /// <summary>
           /// Removes a given Category association from a Recipe.
           /// The method should confirm both entities exist, then remove
           /// their relationship in the junction table or foreign key.
           /// </summary>
           internal async Task RemoveCategoryFromRecipe()
           {
               throw new NotImplementedException();
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

            Console.WriteLine("\nEnter Recipe ID: ");
            int userInput = int.Parse(Console.ReadLine());
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



            throw new NotImplementedException();


            throw new NotImplementedException();
           }
    }
}