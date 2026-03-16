using Microsoft.AspNetCore.Http;
namespace DocumentSystemApi.DTOs;

public class UploadDocumentDTO
{
    public required IFormFile File {get;set;}
}