using Files.Services.Abstractions;
using iText.Kernel.Pdf;
using iText.Signatures;
using Microsoft.Extensions.Logging;

namespace Files.Services.Implementations;

public class PdfSignatureVerifier : IPdfSignatureVerifier
{
    private readonly ILogger<PdfSignatureVerifier> logger;

    public PdfSignatureVerifier(ILogger<PdfSignatureVerifier> logger)
    {
        this.logger = logger;
    }

    public async Task<bool> HasValidSignaturesAsync(string pdfPath, CancellationToken cancellationToken = default)
    {
        try
        {
            using var reader = new PdfReader(pdfPath);
            using var pdfDoc = new PdfDocument(reader);
            var util = new SignatureUtil(pdfDoc);
            var names = util.GetSignatureNames();

            //logger.LogInformation("Se encontraron {Count} firmas en el PDF {File}", names.Count, Path.GetFileName(pdfPath));

            foreach (var name in names)
            {
                var pkcs7 = util.ReadSignatureData(name);
                bool isValid = pkcs7.VerifySignatureIntegrityAndAuthenticity();

                logger.LogInformation("Firma {Name}: Válida = {IsValid}, Firmado por: {Signer}, Fecha: {Date}",
                    name, isValid, pkcs7.GetSignName(), pkcs7.GetSignDate());

                if (!isValid)
                    return false;
            }

            return names.Count > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al verificar firmas del archivo {File}", Path.GetFileName(pdfPath));
            return false;
        }
    }
}