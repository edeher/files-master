namespace Files.Entities.Configuration;

public class AppSettings
{
    public SignatureVerifier SignatureVerifier { get; set; } = default!;
}

public class SignatureVerifier
{
    public int IterationLengthInSeconds { get; set; }
}