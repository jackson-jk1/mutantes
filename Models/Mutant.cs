using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mutantes.Models
{
    public class Mutant
    {
       public int Id {get;set;}
       public string? Name {get;set;}
        
       public string? Photo {get;set;}
       
       [JsonIgnore]
       public string? Abilities_one {get;set;}
       
       [JsonIgnore]
       public string? Abilities_two {get;set;}

       [JsonIgnore]
       public string? Abilities_tree {get;set;}
       
       [JsonIgnore]
       public virtual User? Professor {get;set;}

       public List<string> Abilits {get;set;}

    }
}