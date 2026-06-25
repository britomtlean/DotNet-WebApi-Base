using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2026.Settings
{
    public class FilesSettings
    {
        public async Task<string> Download(IFormFile arquivo)
        {
            // Criar pasta
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "Public/Images");
            Console.WriteLine("Pasta:");
            Console.WriteLine(pasta);

            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            // Nome único do arquivo

            // Padrão
            //var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(arquivo.FileName);
            //Console.WriteLine($"Nome: {nomeArquivo}");

            // Teste
            Console.WriteLine($"Nome: {arquivo.FileName}");

            // Diretório
            var diretorioImagem = Path.Combine(pasta, arquivo.FileName);


            // Salva a imagem
            using (var stream = new FileStream(diretorioImagem, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // Drive
            //var driveId = await UploadArquivo(arquivo);

            return arquivo.FileName;
        }

    }
}
