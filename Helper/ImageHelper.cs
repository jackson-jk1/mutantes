using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Biblioteca.Helper
{
    
    public class ImageHelper
    {
         public string UploadedFile(IFormFile Foto)
        {
            string nomeUnicoArquivo = null;
            if (Foto != null)
            {
                string pastaFotos = Path.Combine("Images");
                nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + Foto.FileName;
                string caminhoArquivo = Path.Combine(pastaFotos, nomeUnicoArquivo);
                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    Foto.CopyTo(fileStream);
                }
            }
          
            return nomeUnicoArquivo;
        }
         public void DestroyFile(string name)
        {
            if (name != null)
            {
                string pastaFotos = Path.Combine("Images");
               
                string caminhoArquivo = Path.Combine(pastaFotos, name);
                File.Delete(caminhoArquivo);
            }
        }

    }
}