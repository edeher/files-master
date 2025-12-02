using System.ComponentModel.DataAnnotations;
using Files.Dto.Validations;
using Microsoft.AspNetCore.Http;

namespace Files.Dto;

public class DocumentRequestDto
{
    [Required]
    [FileSizeValidation(MaxSizeInMegabytes: 10)]
    [FileTypeValidation(FileTypeGroup.Pdf)]
    public IFormFile? Document { get; set; }
}