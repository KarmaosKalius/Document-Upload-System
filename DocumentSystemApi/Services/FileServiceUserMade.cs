using Microsoft.AspNetCore.Http;
namespace DocumentSystemApi.Services;

public class FileServicesUserMade
{
    private readonly string _uploadFolderPath;
    public FileServicesUserMade()
    {
        _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(),"Storage", "Uploads");
        if (!Directory.Exists(_uploadFolderPath))
        {
            Directory.CreateDirectory(_uploadFolderPath);
        }
    }
    public async Task<string> SaveFileUSerMade(IFormFile file)
    {
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_uploadFolderPath,uniqueFileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return uniqueFileName;
    }
}