using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Files.Dto.Validations;

public class FileTypeValidation : ValidationAttribute
{
    private readonly string[]? validTypes;

    public FileTypeValidation(string[] validTypes)
    {
        this.validTypes = validTypes;
    }

    public FileTypeValidation(FileTypeGroup fileTypeGroup)
    {
        if (fileTypeGroup is FileTypeGroup.Pdf)
        {
            validTypes = ["application/pdf"];
        }
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        if (validTypes is null || validTypes.Length == 0)
            return new ValidationResult("No valid types were found, please contact the admin.");

        IFormFile? formfile = value as IFormFile;

        if (formfile is null)
            return ValidationResult.Success;

        if (!validTypes.Contains(formfile.ContentType))
            return new ValidationResult($"File type not valid, it must be one of the following: {string.Join(",", validTypes)}");

        return ValidationResult.Success;
    }
}
public enum FileTypeGroup
{
    Pdf
}