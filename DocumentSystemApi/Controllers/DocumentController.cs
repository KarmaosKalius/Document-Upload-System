using Microsoft.AspNetCore.Mvc;
using DocumentSystemApi.Data;
using DocumentSystemApi.DTOs;
using DocumentSystemApi.Models;
using DocumentSystemApi.Services;

namespace DocumentSystemApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly FileServicesUserMade _fileServices;
    public DocumentController(AppDbContext context,FileServicesUserMade fileService)
    {
        _context = context;
        _fileServices = fileService;
    }
    [HttpPost("upload")]
    public async Task<IActionResult> UploadDocumentUserMade([FromForm]UploadDocumentDTO request)
    {
        if(request.File == null || request.File.Length == 0)
        {
            return BadRequest("file is missing");
        }
        var storedFileName = await _fileServices.SaveFileUSerMade(request.File);
        var document = new Document
        {
            FileName = request.File.FileName,
            StoredFileName = storedFileName,
            ContentType = request.File.ContentType,
            Size = request.File.Length,
            UploadDate = DateTime.UtcNow
        };
        _context.Documents.Add(document);
        await _context.SaveChangesAsync();
        return Ok(new
        {
            message = "File uploaded successfully",
            documentId = document.Id
        });
    }
}