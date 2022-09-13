using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mutantes.Models;

namespace Mutantes.DTO
{
    public class MUpdateDTO
    {
       public string? Name {get;set;}
       public IFormFile? Photo {get;set;}
       public string? Abilities {get;set;}

       public int ProfessorId {get;set;}

    public static ValueTask<MUpdateDTO?> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
     {
        
        
    
        var firstFile = context.Request.Form.Files.FirstOrDefault();
        var name  = context.Request.Form["name"];
        var abilities  = context.Request.Form["abilities"];
        var profId = context.Request.Form["professorId"];
        int idP = 0;
        try{
             idP = Int32.Parse(profId);
        }
        catch(Exception e){
             idP = 0;
        }
        
        
     
        return ValueTask.FromResult<MUpdateDTO?>(new MUpdateDTO
        {
            Name = name,
            Photo = firstFile,
            Abilities = abilities,
            ProfessorId = idP
            
        });
     }

    }
}