using System.Text.Json.Serialization;
using Cocona;
using IRSI.Common.Abstractions;
using IRSI.UberEatsManager.SyncCli.Commands;
using IRSI.UberEatsManager.SyncCli.Extensions;
using IRSI.UberEatsManager.SyncCli.Options;
using Microsoft.Extensions.DependencyInjection;
using Environment = IRSI.Common.Environment;

var builder = CoconaApp.CreateBuilder();

builder.Services.Configure<UberEatsManagerOptions>(
    builder.Configuration.GetSection(UberEatsManagerOptions.SectionName));

builder.UseWritableOptions<Hashes>(Hashes.SectionName, "hashes.json", () => new()
{
    WriteIndented = true,
    Converters = { new JsonStringEnumConverter() }
});

builder.Services.AddSingleton<IEnvironment, Environment>();

var app = builder.Build();

app.AddCommands<UploadDataCommand>();

app.Run();