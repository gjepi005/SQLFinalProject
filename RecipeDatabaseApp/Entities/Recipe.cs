using System;
using System.Collections.Generic;

namespace RecipeDatabaseApp.Entities;

public partial class Recipe
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Ingredient> IngredientsNavigation { get; set; } = new List<Ingredient>();
}
