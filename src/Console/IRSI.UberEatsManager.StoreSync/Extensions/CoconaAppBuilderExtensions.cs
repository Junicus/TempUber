using System.Text.Json;
using Cocona.Builder;
using IRSI.WritableOptions;
using Microsoft.Extensions.Configuration;

namespace IRSI.UberEatsManager.SyncCli.Extensions;

public static class CoconaHostBuilderExtensions
{
    public static CoconaAppBuilder UseWritableOptions<T>(
        this CoconaAppBuilder hostBuilder,
        string sectionName,
        string fileName = "appsettings.json",
        Func<JsonSerializerOptions>? jsonSerializerOptionsFactory = null)
        where T : class, new()
    {
        hostBuilder.Configuration.AddJsonFile(fileName, optional: true, reloadOnChange: true);
        hostBuilder.Services.ConfigureWritable<T>(options =>
        {
            options.FileName = fileName;
            options.SectionName = sectionName;
            options.ConfigurationRoot = hostBuilder.Configuration;
            options.JsonSerializerOptionsFactory = jsonSerializerOptionsFactory;
        });

        return hostBuilder;
    }
}