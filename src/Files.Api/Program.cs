using Files.Api.HostedService;
using Files.Entities.Configuration;
using Files.Services.Abstractions;
using Files.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPdfSignatureVerifier, PdfSignatureVerifier>();
builder.Services.AddHostedService<SignatureVerifiersHostedService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
