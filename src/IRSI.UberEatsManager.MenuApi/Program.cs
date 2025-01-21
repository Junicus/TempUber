using System.Reflection.Metadata;
using APIWeaver;
using Asp.Versioning;
using IRSI.UberEatsManager.MenuApi.Extensions;
using IRSI.UberEatsManager.MenuApi.Middleware;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.Services
    .AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = ApiVersion.Default;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddOpenApi("v1",
    options =>
    {
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Info = new()
            {
                Title = "Menu API v1",
                Version = "1.0.0"
            };

            return Task.CompletedTask;
        });

        options.AddSecurityScheme("x-api-key", scheme =>
        {
            scheme.Type = SecuritySchemeType.ApiKey;
            scheme.In = ParameterLocation.Header;
            scheme.Name = "x-api-key";
        });
    });

builder.Services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 5 * 1024 * 1024; });

const string fileStorageLocation = "/tmp/data";
if (!Directory.Exists(fileStorageLocation))
{
    Directory.CreateDirectory(fileStorageLocation);
}

builder.Services.AddApiKeyAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Menu API")
            .WithTheme(ScalarTheme.Kepler)
            .WithSearchHotKey("s")
            .WithModels(false)
            .WithDownloadButton(false)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithPreferredScheme("x-api-key")
            .WithApiKeyAuthentication(x => x.Token = "secret-api-key");
    });
}

app.UseAuthorization();

string[] allowedExtensions = [".zip"];
app.MapPost("/upload", async (IFormFile file, ILogger<Program> logger) =>
    {
        if (file.Length == 0)
        {
            logger.LogWarning("Uploaded file {FileName} is empty", file.FileName);
            return Results.BadRequest("File is empty");
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            logger.LogWarning("Uploaded file {FileName} has an invalid extension", file.FileName);
            return Results.BadRequest("Invalid file extension");
        }

        var storeFileName = Path.Combine(fileStorageLocation, file.FileName);
        await using (var stream = new FileStream(storeFileName, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        logger.LogInformation("Upload of {FileName} was successful", file.FileName);
        return Results.Ok($"Upload of {file.FileName} was successful");
    })
    .DisableAntiforgery()
    .WithName("UploadFile");

app.MapDefaultEndpoints();
app.Run();