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
       [Required(ErrorMessage = "Abilidades obrigatorios")]
       public string Abilities {get;set;}

       [Required(ErrorMessage = "Professor obrigatorio")]
       public int ProfessorId {get;set;}

    public static ValueTask<MutantDTO?> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
     {
        var firstFile = context.Request.Form.Files[0];
        var name  = context.Request.Form["name"];
        var abilities  = context.Request.Form["abilities"];
        var profId = context.Request.Form["professorId"];
       
     
        return ValueTask.FromResult<MutantDTO?>(new MutantDTO
        {
            Name = name,
            Photo = firstFile,
            Abilities = abilities,
            ProfessorId = Int32.Parse(profId)
            
        });
     }

    }
}