using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutantes.Data;

using Mutantes.Models;
using MiniValidation;
using Biblioteca.Helper;
using Mutantes.DTO;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDirectoryBrowser();
builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseMySql("server=localhost;port=3306;database=mutantes;uid=root;password=''", new MySqlServerVersion(new Version(10,4,24))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("User", async (AppDbContext db, [FromBody]UserDTO userDTO) =>
{
    try
    {
        
        if (!MiniValidator.TryValidate(userDTO, out var errors))
        {
            return Results.BadRequest(errors);
        }
        var u = new User();
        u.Email = userDTO.Email;
        u.Password = userDTO.Password;
      
        db.Users.Add(u);
        Console.WriteLine("ok");
        db.SaveChanges();
        return Results.Ok();

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
});

app.MapGet("User", async (AppDbContext db, [FromQuery] string Email, string Password) =>
{
    try
    {
        var userDTO = new UserDTO();
        userDTO.Email = Email;
        userDTO.Password = Password;
        if (!MiniValidator.TryValidate(userDTO, out var errors))
        {
            return Results.BadRequest(errors);
        }
       
        User u = db.Users.Where(u => u.Email == userDTO.Email && u.Password == userDTO.Password).FirstOrDefault();
        if (null != u)
        {
            return Results.Ok(u);
        }
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapPost("Mutant", async (AppDbContext db, MutantDTO mutantDTO) =>
{
    try
    {
        
        if (!MiniValidator.TryValidate(mutantDTO, out var errors))
        {
            return Results.BadRequest(errors);
        }
        ImageHelper image = new ImageHelper();
        var m = new Mutant();
        m.Name =  mutantDTO.Name;
        m.Abilities_one = mutantDTO.Abilities_one;
        m.Abilities_two = mutantDTO.Abilities_two;
        m.Abilities_tree = mutantDTO.Abilities_tree;
        
        Console.WriteLine(mutantDTO.ProfessorId);
        User U = db.Users.Where(p => p.Id == mutantDTO.ProfessorId).First();
        Console.WriteLine(U.Id);
        m.Professor = U;
        string path = image.UploadedFile(mutantDTO.Photo);
        m.Photo = path;
        
        db.Mutants.Add(m);
        Console.WriteLine("ok");
        db.SaveChanges();
        return Results.Ok("Registrado com sucesso");

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
}).Accepts<MutantDTO>("multipart/form-data").Produces(200);

app.MapPut("Mutant", async (AppDbContext db, MUpdateDTO mutantDTO, int id) =>
{
    try
    {
        
       
        if (id.Equals("") || id.Equals(null))
        {
            return Results.BadRequest();
        }
         ImageHelper image = new ImageHelper();
        var m = db.Mutants.Where(m => m.Id == id).First();
        string path = image.UploadedFile(mutantDTO.Photo);

        if(path != null){
            image.DestroyFile(m.Photo);
             m.Photo = path;
        }
        if(mutantDTO.Name == ""){
            return Results.NotFound("Nome n??o pode ser vazio");
        }
        if(mutantDTO.Abilities_one == ""){
              return Results.NotFound("O mutante deve possuir no minimo uma abilidade");
        }
        User U;
          if(mutantDTO.ProfessorId == 0 ){
             return Results.NotFound("O mutante deve possuir um responsavel");
        }
        m.Name =  mutantDTO.Name;
        m.Abilities_one = mutantDTO.Abilities_one;
        m.Abilities_two = mutantDTO.Abilities_two;
        m.Abilities_tree = mutantDTO.Abilities_tree;
        U = db.Users.Where(p => p.Id == mutantDTO.ProfessorId).First();
        db.Mutants.Update(m);
        db.SaveChanges();
        return Results.Ok();

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
}).Accepts<MUpdateDTO>("multipart/form-data").Produces(200);

app.MapGet("Mutant", async (AppDbContext db, [FromQuery] int id) =>
{
    try
    {
        
        if (id.Equals("") || id.Equals(null))
        {
            return Results.BadRequest();
        }
     
        var m = db.Mutants.Where(m => m.Id == id).Include(p => p.Professor).First();
        char[] delimiterChars = { ';' };
        m.Abilits = new List<string>();
        m.Photo = "Images/" + m.Photo;
        m.Abilits.Add(m.Abilities_one);
        if(m.Abilities_two != null)
        m.Abilits.Add(m.Abilities_two);
        if(m.Abilities_tree != null)
        m.Abilits.Add(m.Abilities_tree);
        return Results.Ok(m);

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
});
app.MapGet("Mutants", async (AppDbContext db, [FromQuery] int id) =>
{
    try
    {
        
        if (id.Equals("") || id.Equals(null))
        {
            return Results.BadRequest();
        }
        var p = db.Users.Find(id);
        List<Mutant>mutants = db.Mutants.Where(m => m.Professor == p).ToList();
        char[] delimiterChars = { ';' };
            mutants.ForEach( m => {
            m.Abilits = new List<string>();
            m.Professor = null;
            m.Photo = "Images/" + m.Photo;
            
            m.Abilits.Add(m.Abilities_one);
            if(m.Abilities_two != null)
            m.Abilits.Add(m.Abilities_two);
            if(m.Abilities_tree != null)
            m.Abilits.Add(m.Abilities_tree);
        });
        
        return Results.Ok(mutants);

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
});

app.MapGet("Mutants/Dashboard", async (AppDbContext db, [FromQuery] int id) =>
{
    try
    {
        
        if (id.Equals("") || id.Equals(null))
        {
            return Results.BadRequest();
        }
        var dash = new Dashboard();
        using (var command = db.Database.GetDbConnection().CreateCommand())
        {
        db.Database.OpenConnection();
        command.CommandText =
        "SELECT q.id_criador, q.nome_habilidade, SUM(qtd) quantidade_total" +
        " FROM (SELECT m.Abilities_one nome_habilidade, m.Name, m.ProfessorId id_criador, COUNT(DISTINCT m.Abilities_one) qtd FROM mutants m WHERE m.Abilities_one <> + \"\" GROUP BY m.Abilities_one, m.Name, m.ProfessorId " +
        " UNION ALL" +
        " SELECT m.Abilities_two nome_habilidade, m.Name, m.ProfessorId id_criador, COUNT(DISTINCT m.Abilities_two) qtd FROM mutants m WHERE m.Abilities_two <> \"\" GROUP BY m.Abilities_two, m.Name, m.ProfessorId" +
        " UNION ALL" +
        " SELECT m.Abilities_tree nome_habilidade, m.Name, m.ProfessorId id_criador, COUNT(DISTINCT m.Abilities_tree) qtd FROM mutants m WHERE m.Abilities_tree <> \"\" GROUP BY m.Abilities_tree,m.Name, m.ProfessorId" +
        " ) q" +
        $" WHERE q.id_criador = {id}" +
        " GROUP BY q.nome_habilidade, q.id_criador" +
        " ORDER BY 3 DESC, 2 ASC" +
        " LIMIT 3;";
        using (var result = command.ExecuteReader())
        {
            dash.top3 = new List<string>();
           while (result.Read())
            {
                dash.top3.Add(result["nome_habilidade"].ToString());
            }
        }
         command.CommandText = $"SELECT m.ProfessorId nome_criador, count(DISTINCT m.Name) quantidade_mutantes FROM mutants m WHERE m.ProfessorId = {id} GROUP BY m.ProfessorId";
         using (var result = command.ExecuteReader())
            {
            while (result.Read())
                {
                    dash.num = Int32.Parse(result["quantidade_mutantes"].ToString());
                }
            }
        }
    
        return Results.Ok(dash);

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
});

app.MapGet("Mutants/search", async (AppDbContext db, [FromQuery] int id, string hab) =>
{
    try
    {
        
        if (id.Equals("") || id.Equals(null))
        {
            return Results.BadRequest();
        }
        List<Mutant> mutants = new List<Mutant>();
        using (var command = db.Database.GetDbConnection().CreateCommand())
        {
        db.Database.OpenConnection();
        command.CommandText =
         $"SELECT * FROM mutants m WHERE m.ProfessorId = {id} AND ( " +
         $" LOWER(m.Abilities_one) LIKE  '%{hab}%' OR " +
         $" LOWER(m.Abilities_two) LIKE  '%{hab}%' OR " +
         $" LOWER(m.Abilities_tree) LIKE '%{hab}%');";
      
        using (var result = command.ExecuteReader())
        {
           while (result.Read())
            {
              var m = new Mutant();
               m.Id = Int32.Parse(result["id"].ToString());
               m.Name = result["name"].ToString();
               m.Abilities_one = result["abilities_one"].ToString();
               m.Abilities_two = result["abilities_two"].ToString();
               m.Abilities_tree = result["abilities_tree"].ToString();
               m.Photo = "Images/" + result["photo"].ToString();

               mutants.Add(m);
            }
         }
        }
    
        return Results.Ok(mutants);

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
});
app.MapDelete("Mutant", async (AppDbContext db, [FromQuery] int id) =>
{
    try
    {
        
        if (id.Equals("") || id.Equals(null))
        {
            return Results.BadRequest();
        }
     
        var m = db.Mutants.Find(id);
        ImageHelper image = new ImageHelper();
        image.DestroyFile(m.Photo);
        db.Mutants.Remove(m);
        db.SaveChanges();
        return Results.Ok();

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
});
app.Run();

