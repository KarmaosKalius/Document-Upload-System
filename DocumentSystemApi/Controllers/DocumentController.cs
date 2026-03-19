using Microsoft.AspNetCore.Mvc;
using DocumentSystemApi.Data;
using DocumentSystemApi.DTOs;
using DocumentSystemApi.Models;
using DocumentSystemApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
    [HttpGet("list")]
    public async Task<IActionResult> GetDocumentsUserMade()
    {
        var documents = await _context.Documents.Select(d => new
        {
            d.Id,
            d.FileName,
            d.ContentType,
            d.Size,
            d.UploadDate
        }).ToListAsync();
        return Ok(documents);
    }
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadDocumentUserMade(int id)
    {
        var document = await _context.Documents.FindAsync(id);
        if(document == null)
        {
            return NotFound("Document Not Found");
        }
        var filePath = Path.Combine(Directory.GetCurrentDirectory(),"Storage","Uploads", document.StoredFileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found on server");
        }
        var filesBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(filesBytes,document.ContentType, document.FileName);
    }
    [HttpGet("view/{id}")]
    public async Task<IActionResult> ViewDocumentUserMade(int id)
    {
        var document = await _context.Documents.FindAsync(id);
        if(document == null)
        {
            return NotFound("Document not found ");
        }
        var filePath = Path.Combine(Directory.GetCurrentDirectory(),"Storage", "Uploads", document.StoredFileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found on the server");
        }
        var fileStream = new FileStream(filePath,FileMode.Open, FileAccess.Read);
        return File(fileStream,document.ContentType);
    }
}
