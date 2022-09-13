using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mutantes.Models;

namespace Mutantes.Data
{
    public class AppDbContext : DbContext {
    public AppDbContext (DbContextOptions<AppDbContext> options)
        : base(options)
    {
         
    }

    public DbSet<User> Users {get;set;}
    public DbSet<Mutant> Mutants  {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<User>().Property(u => u.Email).IsUnicode().IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();

        modelBuilder.Entity<Mutant>().Property(m => m.Name).IsRequired().IsUnicode();
        modelBuilder.Entity<Mutant>().Property(m => m.Photo).IsRequired();
        modelBuilder.Entity<Mutant>().Ignore(m => m.Abilits);
    
       

    }

}
    
}