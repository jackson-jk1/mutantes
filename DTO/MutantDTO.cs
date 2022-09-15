using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mutantes.Models;

namespace Mutantes.DTO
{
    public class MutantDTO
    {
       [Required(ErrorMessage = "Name obrigatorio")]
       public string Name {get;set;}
       [Required(ErrorMessage = "Photo obrigatorio")]
       public IFormFile Photo {get;set;}

       [Required(ErrorMessage = "Abilidade Primaria obrigatoria")]
       public string? Abilities_one {get;set;}
      
       public string? Abilities_two {get;set;}

       public string? Abilities_tree {get;set;}

       [Required(ErrorMessage = "Professor obrigatorio")]
       public int ProfessorId {get;set;}

    public static ValueTask<MutantDTO?> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
     {
        var firstFile = context.Request.Form.Files[0];
        var name  = context.Request.Form["name"];
        var profId = context.Request.Form["professorId"];
        var ab1 = context.Request.Form["abilities_one"];
        var ab2 = context.Request.Form["abilities_two"];
        var ab3 = context.Request.Form["abilities_tree"];
       
     
        return ValueTask.FromResult<MutantDTO?>(new MutantDTO
        {
            Name = name,
            Photo = firstFile,
            ProfessorId = Int32.Parse(profId),
            Abilities_one =ab1,
            Abilities_two =ab2,
            Abilities_tree =ab3
            
        });
     }

    }
}