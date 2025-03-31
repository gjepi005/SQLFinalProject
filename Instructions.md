# **Project Assignment: Recipe Database Management System**

## **Overview**

In this project, you will:

1. **Design** a relational database schema for managing recipes.
2. **Implement** the database by writing SQL scripts in **two** files:
   - **`Create.sql`** for table creation.
   - **`Insert.sql`** for data insertion.
3. **Enhance** a .NET Core console application, adding methods to interact with your PostgreSQL database via EF Core.

You will submit your deliverables via a **GitHub repository**, including a **comprehensive report** (using the provided template) and all relevant code/scripts.

---

## **Project Steps and Deliverables**

### 1. **Database Design**

1. **Modeling & ER Diagram**

   - Identify and describe your entities (e.g., `Recipe`, `Ingredient`, `Category`, etc.).
   - Define relationships and cardinalities (one-to-many, many-to-many, etc.).
   - Create an **ER Diagram** that shows all entities, attributes, and relationships.

2. **Design Rationale in Report**
   - Discuss normalization strategies (e.g., 3NF) and constraints (`PK`, `FK`, `NOT NULL`, `UNIQUE`, etc.).
   - Justify your approach to many-to-many relationships (e.g., using junction tables).

**Deliverables**:

- **ER Diagram** included in your GitHub repository (e.g., as a PNG or PDF).
- **Report Section**: “Step 1: Database Design” in `ReportTemplate.md`.

---

### 2. **Database Implementation (SQL)**

You are required to provide **two** SQL files in your repository (can be found inside SQL-queries folder):

1. **`Create.sql`**

   - Contains **`CREATE TABLE`** statements for each entity in your design.
   - Must include **primary keys**, **foreign keys**, **constraints**, etc.
   - Each table should precisely reflect the ER diagram.

2. **`Insert.sql`**

   - Contains **`INSERT INTO`** statements to populate the database with **sample data**.
   - Ensure the data demonstrates relationships (e.g., multiple recipes, shared ingredients).
   - Include at least **3–5 recipes** and enough ingredients/categories to show variety.

3. **Testing & Validation**
   - After running `Create.sql` and `Insert.sql`, run a few queries (using `psql` or another tool `pgAdmin`) to confirm everything is working:
     - For example, `SELECT * FROM Recipe;`, `SELECT * FROM Ingredient;`, etc.
   - Verify your foreign key constraints and relationship tables (if any) contain valid references.

**Deliverables**:

- **`Create.sql`**: Table creation scripts.
- **`Insert.sql`**: Data insertion scripts.
- **Report Section**: “Step 2: Database Implementation” in `ReportTemplate.md`, summarizing your table structures and sample data choices.

---

### 3. **.NET Core Console Application Enhancement**

Use the existing **.NET Core console app** implementation inside `RecipeDatabaseApp` that has basic EF Core setup.

1. **Prepare the Console Application**

   - Ensure EF Core packages are installed:
     ```bash
      dotnet add package Microsoft.EntityFrameworkCore
      dotnet add package Microsoft.EntityFrameworkCore.Tools
      dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
     ```

2. **Scaffold Entity Models from the Existing Database**

   - **Run the Scaffold Command**

     - In the **root folder** of your console application (where the `.csproj` file is located), open a terminal or command prompt and execute:

       ```bash
         dotnet ef dbcontext scaffold "Host=...;Port=...;Database=...;Username=...;Password=..." Npgsql.EntityFrameworkCore.PostgreSQL -o Entities --force
       ```

       **Explanation**:

       - `dbcontext scaffold`: The EF Core command for reverse-engineering a database into C# models.

       - `"Host=...;"`: Your **PostgreSQL connection string** (e.g. `localhost`).

       - `"Port=...;"`: Port where your **PostgreSQL database** is running (e.g. `5432`).

       - `"Database=...;"`: Name of your **PostgreSQL database** (e.g. `RecipeDB`).

       - `"Username=...;"`: Your **PostgreSQL database** username (e.g. `postgres`).

       - `"Password=...;"`: Password for your **PostgreSQL database**.

       - `Npgsql.EntityFrameworkCore.PostgreSQL`: The EF Core provider for PostgreSQL.

       - `-o Entities`: Outputs all generated entity classes and `DbContext` into a folder named `Entities`.

       - `--force`: Overwrites existing files if any. Use with caution.

3. **Review the Generated Files**

   - A new `RecipeDbContext` (or similarly named context) file should be generated.

   - Entity classes representing each table in your database (e.g., `Recipe`, `Ingredient`, `Category`) will also be created in the `Entities` folder.

4. **Perform EF Core Migrations After Scaffolding**

Even though your database is already created, you should set up EF Core migrations to track **future changes** in code. Here’s how:

1.  **Add the Initial Migration**

    - If you have never run migrations in this project, create an “initial” migration that matches your **existing** database:

    ```bash
      dotnet ef migrations add InitialCreate --ignore-changes
    ```

    - **Explanation**:
    - `--ignore-changes` tells EF Core to generate a migration without **diffing** the existing schema. This creates a baseline so EF Core knows how to track future changes.

2.  **Apply Migrations**

    - Now run:

    ```bash
      dotnet ef database update
    ```

    - Since your database is already created, EF Core won’t make changes for this initial migration, but you now have a migration history.

    - **Future Schema Changes**: If you alter your models or want to add new tables, you can use:

    ```bash
      dotnet ef migrations add SomeFeature
      dotnet ef database update
    ```

    - This approach keeps your schema **synchronized** between the code and database going forward.

3.  **Implement Required Methods**

    Once your entity classes and `DbContext` are generated, **use them** to implement the following methods or any existing stubs you created in prior steps:

    1. **CRUD Operations**

       - **AddNewRecipe()**: Insert a new `Recipe` entity based on user inputs.
       - **ListAllRecipes()**: Retrieve all `Recipe` entities and display them.
       - **UpdateRecipe()**: Prompt user for a `RecipeId` and update relevant fields.
       - **DeleteRecipe()**: Remove the specified recipe (handle related data in junction tables if needed).

    2. **Advanced Queries**

       - **FetchRecipeByCategory()**: Prompt for a category, then use EF Core & LINQ to filter recipes.
       - **SearchRecipeByIngredients()**: Accept multiple ingredient names, return recipes containing **all** of those ingredients.
       - **AddCategoryToRecipe()** / **RemoveCategoryFromRecipe()**: Manipulate many-to-many or foreign key relationships as needed.

    3. **Menu & Error Handling**

       - Provide a **text-based menu**.
       - Validate user input (e.g., does the specified recipe exist? Is the category valid?).
       - Use appropriate error messages and catch exceptions.

4.  **Testing & Demonstration**

5.  **Test Your Application**

    - Confirm each menu item (e.g., “Add new recipe,” “Fetch recipes by category,” etc.) works as expected.
    - Use the sample data inserted by your `Insert.sql` or add new data dynamically.

6.  **Showcase**

    - (Optional) Provide screenshots or a short video of your console application running the key features.
    - Summarize any LINQ queries in your `report.md` if required.

**Deliverables**:

- **Scaffolded Models**: The `Models/` folder containing automatically generated entity classes and the `DbContext`.
- **Migrations Folder**: Contains migration files (`InitialCreate` and any subsequent migrations).
- **Enhanced Methods**: Implementation of all required CRUD and advanced queries in your **Program.cs** (or separate classes) using EF Core.
- **Report Section**: “Step 3: .NET Core Console Application Enhancement” in `report.md`, detailing:
  - Your **scaffold** process (commands used, any manual refinements).
  - How you set up or updated **migrations**.
  - Explanations or snippets of the **LINQ queries** for your new/updated methods.

---

## **GitHub Repository Requirements**

1. **Version Control**

   - Commit each major step with clear messages (e.g., “Add initial schema creation script,” “Enhance console app with advanced queries”).

2. **Use the Provided Report Template**
   - Find `ReportTemplate.md` (or similarly named file) in the repository (provided by the instructor).
   - Rename it to `FinalReport.md` and **fill it out** as you progress.
   - Commit new sections of the report as you complete each step.

---

## **Submission Guidelines**

1. **GitHub Repository Link**

   - Provide the link to your repository according to the insructions.
   - Make sure your final code, SQL scripts, and `FinalReport.md` are all up to date.

2. **Report**

   - Follow the **report template** exactly.
   - Include references to or snippets of important SQL statements, code blocks, or screenshots.

3. **Optional Demo**
   - You may include a short screen capture or screenshots demonstrating the console app menu and its functionality (e.g., listing recipes, adding new ones).

---

## **Evaluation Criteria**

1. **Database Design (max 6 points)**

   - Quality of ER Diagram and rationale (normalization, keys, constraints).
   - Clarity of relationships.

2. **SQL Implementation (max 5 points)**

   - Correctness and completeness of **`Create.sql`** (table creation).
   - Quality and relevance of **`Insert.sql`** (sample data).
   - Proper relationships and constraints.

3. **.NET Core Application (max 6 points)**

   - Implementation of the required CRUD and advanced features.
   - Correct usage of EF Core and LINQ to interact with PostgreSQL.
   - Menu usability, input validation, and general code quality.

4. **Report & Repository (max 3 points)**
   - Thoroughness and clarity of `FinalReport.md`, following the provided template.
   - Organized and well-documented GitHub repository, with meaningful commit messages.

---

### **Final Notes**

Upon completing this project, you will have:

1. A fully designed and documented relational database schema for food recipes.
2. Two SQL files, **`Create.sql`** and **`Insert.sql`**, that can set up and populate the database.
3. A .NET Core console application that **interacts** with your PostgreSQL database for various recipe-related tasks.

This holistic approach ensures that you gain experience in database design, SQL implementation, and real-world application development with EF Core. Good luck!
