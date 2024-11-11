using Framework.Application;

namespace ServiceHost;

public class FileUploader : IFileUploader
{
    private readonly IWebHostEnvironment _environment;

    public FileUploader(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public string Upload(IFormFile? file, string path)
    {
        if (file == null) return "";

        var directoryPath = $"{_environment.WebRootPath}/ProductPictures/{path}";
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
        var filePath = $"{directoryPath}/{fileName}";

        using var output = File.Create(filePath);
        file.CopyTo(output);
        return $"{path}/{fileName}";
    }
}