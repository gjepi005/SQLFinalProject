using System;
using System.Collections.Generic;

namespace RecipeDatabaseApp.Entities;

public partial class Ingredient
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? RecipeId { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
