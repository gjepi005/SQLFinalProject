using System;
using System.Collections.Generic;

namespace RecipeDatabaseApp.Entities;

public partial class Category
{
    public string? Name { get; set; }

    public int Id { get; set; }

    public int? RecipeId { get; set; }

    public virtual Recipe? Recipe { get; set; }
}
