using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Api.Controllers;
[Route("api/files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
            ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
    }

    [HttpGet("fileId")]
    public ActionResult GetFile(string fileId)
    {
        // for demo purposses we are not looking up the file by id.
        var pathToFile = "tammy.gif";

        // check if file exists
        if (!System.IO.File.Exists(pathToFile))
        {
            return NotFound();
        }

        if(!_fileExtensionContentTypeProvider.TryGetContentType(
            pathToFile, out var contentType))
        {
            // Default content type for arbitrary binary data
            contentType = "application/octet-stream";
        }

        var bytes = System.IO.File.ReadAllBytes(pathToFile);
        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }
}
