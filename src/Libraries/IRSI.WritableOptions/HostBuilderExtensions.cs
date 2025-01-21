using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IRSI.WritableOptions;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseWritableOptions<T>(
        this IHostBuilder hostBuilder,
        string sectionName,
        string fileName = "appsettings.json",
        Func<JsonSerializerOptions>? jsonSerializerOptionsFactory = null)
        where T : class, new()
    {
        hostBuilder
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddJsonFile(fileName, optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.ConfigureWritable<T>(options =>
                {
                    options.FileName = fileName;
                    options.SectionName = sectionName;
                    options.ConfigurationRoot = context.Configuration as IConfigurationRoot;
                    options.JsonSerializerOptionsFactory = jsonSerializerOptionsFactory;
                });
            });

        return hostBuilder;
    }
}