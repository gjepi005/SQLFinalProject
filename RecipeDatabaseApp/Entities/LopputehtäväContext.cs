using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipeDatabaseApp.Entities;

public partial class LopputehtäväContext : DbContext
{
    public LopputehtäväContext()
    {
    }

    public LopputehtäväContext(DbContextOptions<LopputehtäväContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=LoppuTehtävä;Username=postgres;Password=");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.ToTable("category");

            entity.HasIndex(e => e.Name, "category_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ingredient_pkey");

            entity.ToTable("ingredient");

            entity.HasIndex(e => e.Name, "ingredient_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recipe_pkey");

            entity.ToTable("recipe");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("recipe_category_id_fkey");

            entity.HasMany(d => d.Ingredients).WithMany(p => p.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipeIngredient",
                    r => r.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("IngredientId")
                        .HasConstraintName("recipe_ingredient_ingredient_id_fkey"),
                    l => l.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeId")
                        .HasConstraintName("recipe_ingredient_recipe_id_fkey"),
                    j =>
                    {
                        j.HasKey("RecipeId", "IngredientId").HasName("recipe_ingredient_pkey");
                        j.ToTable("recipe_ingredient");
                        j.IndexerProperty<int>("RecipeId").HasColumnName("recipe_id");
                        j.IndexerProperty<int>("IngredientId").HasColumnName("ingredient_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
