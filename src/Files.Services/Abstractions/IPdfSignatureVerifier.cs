namespace Files.Services.Abstractions;

public interface IPdfSignatureVerifier
{
    Task<bool> HasValidSignaturesAsync(string pdfPath, CancellationToken cancellationToken = default);
}
