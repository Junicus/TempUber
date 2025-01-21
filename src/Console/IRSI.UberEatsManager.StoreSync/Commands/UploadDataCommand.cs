using System.Security.Cryptography;
using Cocona;
using IRSI.Common.Abstractions;
using IRSI.UberEatsManager.SyncCli.Options;
using IRSI.WritableOptions;
using Microsoft.Extensions.Options;

namespace IRSI.UberEatsManager.SyncCli.Commands;

public class UploadDataCommand(
    IEnvironment environment,
    IWritableOptions<Hashes> hashes,
    IOptions<UberEatsManagerOptions> uberEatsManagerOptions)
{
    private readonly SHA256 _sha256 = SHA256.Create();

    private readonly List<string> _menuFiles =
    [
        "mnu.dbf",
        "mod.dbf",
        "sub.dbf",
        "itm.dbf",
    ];

    private const string IBERDIR = "IBERDIR";

    [Command("upload", Description = "Upload aloha current data to IRSI.UberEatsManager")]
    public async Task UploadData()
    {
        var iberDirPath = environment.GetEnvironmentVariable(IBERDIR) ??
                          throw new InvalidOperationException($"{IBERDIR} environment variable is not set");

        var dataPath = Path.Combine(iberDirPath, "Data");

        foreach (var menuFile in _menuFiles)
        {
            var filePath = Path.Combine(dataPath, menuFile);
            var hash = await GetFileHash(filePath);
            var fileHash = hashes.Value.Files.FirstOrDefault(x => x.FileName == menuFile);
            if (fileHash == null || fileHash.Hash != hash)
            {
                // Upload file
                Console.WriteLine($"Uploading {menuFile}");

                // Update hashes.json file
                fileHash ??= new() { FileName = menuFile };
                fileHash.Hash = hash;

                hashes.Update((options) =>
                {
                    var existingFileHash = options.Files.FirstOrDefault(x => x.FileName == menuFile);
                    if (existingFileHash != null)
                    {
                        existingFileHash.Hash = hash;
                    }
                    else
                    {
                        options.Files.Add(fileHash);
                    }
                });
            }
        }
    }

    private async Task<string> GetFileHash(string filePath, CancellationToken cancellationToken = default)
    {
        await using var fileStream = File.OpenRead(filePath);
        var hash = await _sha256.ComputeHashAsync(fileStream, cancellationToken);
        return hash.Aggregate(string.Empty, (current, b) => current + b.ToString("x2"));
    }
}