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

       public int ProfessorId {get;set;}

       public string? Abilities_one {get;set;}
      
       public string? Abilities_two {get;set;}

       public string? Abilities_tree {get;set;}

    public static ValueTask<MUpdateDTO?> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
     {
        
        
    
        var firstFile = context.Request.Form.Files.FirstOrDefault();
        var name  = context.Request.Form["name"];
        var profId = context.Request.Form["professorId"];
        var ab1 = context.Request.Form["abilities_one"];
        var ab2 = context.Request.Form["abilities_two"];
        var ab3 = context.Request.Form["abilities_tree"];
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
            ProfessorId = idP,
            Abilities_one =ab1,
            Abilities_two =ab2,
            Abilities_tree =ab3
            
        });
     }

    }
}