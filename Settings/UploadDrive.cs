using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;

namespace WebApi2026.Settings
{
    public class UploadDrive
    {
        /*
        public async Task<string> UploadArquivo(IFormFile arquivo)
        {
            GoogleCredential credential;

            using (var stream = new FileStream(
                "Credentials/google-drive.json",
                FileMode.Open,
                FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(DriveService.Scope.Drive);
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "WebApi2026"
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Guid.NewGuid().ToString() + Path.GetExtension(arquivo.FileName)
            };

            using var memoria = arquivo.OpenReadStream();

            var request = service.Files.Create(
                fileMetadata,
                memoria,
                arquivo.ContentType);

            request.Fields = "id";

            await request.UploadAsync();

            return request.ResponseBody.Id;
        }
        */
    }
}
