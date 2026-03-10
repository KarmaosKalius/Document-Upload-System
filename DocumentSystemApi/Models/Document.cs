namespace DocumentSystemApi.Models;

public class Document
{
    public int Id {get; set;}
    public string FileName {get;set;} = string.Empty;
    public string StoredFileName {get;set;} = string.Empty;
    public string ContentType {get; set;} = string.Empty;
    public long Size {get;set;}
    public DateTime UploadDate {get;set;}
}

//The meta date table for the upload with the ORM Entity framework