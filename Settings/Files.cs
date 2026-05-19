using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2026.Settings
{
    public class Files
    {
        public async Task<string> Download(IFormFile arquivo)
        {
            // Criar pasta
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "Images");

            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            // Nome único do arquivo
            var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(arquivo.FileName);

            // Diretório
            var diretorioImagem = Path.Combine(pasta, nomeArquivo);


            // Salva a imagem
            using (var stream = new FileStream(diretorioImagem, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // Drive
            //var driveId = await UploadArquivo(arquivo);

            return diretorioImagem;
        }

    }
}
