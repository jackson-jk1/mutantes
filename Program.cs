using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutantes.Data;

using Mutantes.Models;
using MiniValidation;
using Biblioteca.Helper;
using Mutantes.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseMySql("server=localhost;port=3306;database=mutantes;uid=root;password=''", new MySqlServerVersion(new Version(8, 0, 18))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
        m.Abilities = mutantDTO.Abilities;
        
        Console.WriteLine(mutantDTO.ProfessorId);
        User U = db.Users.Where(p => p.Id == mutantDTO.ProfessorId).First();
        Console.WriteLine(U.Id);
        m.Professor = U;
        string path = image.UploadedFile(mutantDTO.Photo);
        m.Photo = path;
        
        db.Mutants.Add(m);
        Console.WriteLine("ok");
        db.SaveChanges();
        return Results.Ok();

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
        
        if (!MiniValidator.TryValidate(mutantDTO, out var errors))
        {
            return Results.BadRequest(errors);
        }
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
            return Results.NotFound("Nome não pode ser vazio");
        }
        if(mutantDTO.Abilities == ""){
              return Results.NotFound("O mutante deve possuir no minimo uma abilidade");
        }
        User U;
          if(mutantDTO.ProfessorId != 0 ){
             return Results.NotFound("O mutante deve possuir um responsavel");
        }
        m.Name =  mutantDTO.Name;
        m.Abilities = mutantDTO.Abilities;
        m.Abilities = mutantDTO.Abilities;
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

app.MapGet("Mutant/{id}", async (AppDbContext db, [FromQuery] int id) =>
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
        string[] words = m.Abilities.Split(delimiterChars);
        foreach (var word in words)
        {
           if(!word.Equals(""))
           m.Abilits.Add(word);
        }
        return Results.Ok(m);

    }
    catch (Exception ex)
    {
         Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
});

app.MapDelete("Mutant/{id}", async (AppDbContext db, [FromQuery] int id) =>
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

