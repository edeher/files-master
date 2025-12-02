using Files.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Files.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IWebHostEnvironment environment;
    
    public DocumentsController(IWebHostEnvironment environment)
    {
        this.environment = environment;
    }

    /// <summary>
    /// Este endpoint sube archivos PDF a la carpeta nofirmado
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    [HttpPost("uploadFile")]
    public async Task<IActionResult> SubirPdf([FromForm] DocumentRequestDto document)
    {
        //estableciendo la ruta
        var rutaNofirmado = Path.Combine(environment.WebRootPath, "nofirmado");

        if (!Directory.Exists(rutaNofirmado))
            Directory.CreateDirectory(rutaNofirmado);

        var rutaArchivo = Path.Combine(rutaNofirmado, document.Document!.FileName);

        await using (var stream = new FileStream(rutaArchivo, FileMode.Create))
        {
            await document.Document.CopyToAsync(stream);
        }

        return Ok("Archivo subido correctamente.");
    }
}