using System;
using System.Collections.Generic;

namespace RecipeDatabaseApp.Entities;

public partial class Recipe
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}
