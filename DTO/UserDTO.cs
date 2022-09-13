using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mutantes.DTO
{
    public class UserDTO
    {
        [FromRoute]
        [Required(ErrorMessage = "Email obrigatorio")]
        public string Email {get;set;}
       
        [FromRoute]
        [Required(ErrorMessage = "Password obrigatorio")]
        public string Password {get;set;}
    }
}