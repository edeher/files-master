using Files.Entities.Configuration;
using Files.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Files.Api.HostedService;

public class SignatureVerifiersHostedService : BackgroundService
{
    private readonly IWebHostEnvironment environment;
    private readonly ILogger<SignatureVerifiersHostedService> logger;
    private readonly IPdfSignatureVerifier signatureVerifier;
    private readonly IOptions<AppSettings> options;

    public SignatureVerifiersHostedService(
        IWebHostEnvironment environment,
        ILogger<SignatureVerifiersHostedService> logger,
        IPdfSignatureVerifier signatureVerifier,
        IOptions<AppSettings> options)
    {
        this.environment = environment;
        this.logger = logger;
        this.signatureVerifier = signatureVerifier;
        this.options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var carpetaNofirmado = Path.Combine(environment.WebRootPath, "nofirmado");
        var carpetaFirmado = Path.Combine(environment.WebRootPath, "firmado");
        var signatureVerifierSettings = options.Value.SignatureVerifier;
        
        if (!Directory.Exists(carpetaFirmado))
            Directory.CreateDirectory(carpetaFirmado);

        while (!stoppingToken.IsCancellationRequested)
        {
            var archivos = Directory.GetFiles(carpetaNofirmado, "*.pdf");
            int counter = 0;
            foreach (var archivo in archivos)
            {
                try
                {
                    if (await signatureVerifier.HasValidSignaturesAsync(archivo, stoppingToken))
                    {
                        counter++;
                        var destino = Path.Combine(carpetaFirmado, Path.GetFileName(archivo));
                        File.Move(archivo, destino);
                        logger.LogInformation("Archivo firmado detectado y movido: {File}", Path.GetFileName(archivo));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error al procesar el archivo {File}", archivo);
                }
            }
            logger.LogInformation($"Hosted service procesó {counter} pdfs con firmas electrónicas.");
            await Task.Delay(TimeSpan.FromSeconds(signatureVerifierSettings.IterationLengthInSeconds), stoppingToken);
        }
    }
}
