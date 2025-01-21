using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace IRSI.WritableOptions;

public static class ServiceCollectionExtensions
{
    public class WritableOptionsSettings
    {
        public string FileName { get; set; } = "appsettings.json";
        public string? SectionName { get; set; }
        public Func<JsonSerializerOptions>? JsonSerializerOptionsFactory { get; set; }
        public IConfigurationRoot? ConfigurationRoot { get; set; }
    }

    public static void ConfigureWritable<T>(this IServiceCollection services,
        Action<WritableOptionsSettings>? configure = null)
        where T : class, new()
    {
        var settings = new WritableOptionsSettings();
        configure?.Invoke(settings);

        var fileName = settings.FileName;
        var configRoot = settings.ConfigurationRoot;
        var sectionName = settings.SectionName ?? typeof(T).Name;
        var configSection = configRoot.GetSection(sectionName);
        var jsonSerializerOptionsFactory = settings.JsonSerializerOptionsFactory;

        services.Configure<T>(configSection);
        services.AddTransient<IWritableOptions<T>>(provider =>
        {
            string jsonFilePath;
            var environment = provider.GetService<IHostEnvironment>();
            if (environment is not null)
            {
                var fileProvider = environment.ContentRootFileProvider;
                var fileInfo = fileProvider.GetFileInfo(fileName);
                jsonFilePath = fileInfo.PhysicalPath;
            }
            else
            {
                jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    fileName);
            }

            var options = provider.GetRequiredService<IOptionsMonitor<T>>();
            return new WritableOptions<T>(
                jsonFilePath,
                sectionName,
                options,
                configRoot,
                jsonSerializerOptionsFactory);
        });
    }

    public static void ConfigureWritable<T>(this IServiceCollection services, IConfigurationRoot configurationRoot,
        string sectionName, string file = "appsettings.json",
        Func<JsonSerializerOptions>? jsonSerializerOptionsFactory = null) where T : class, new()
    {
        services.ConfigureWritable<T>(options =>
        {
            options.FileName = file;
            options.ConfigurationRoot = configurationRoot;
            options.SectionName = sectionName;
            options.JsonSerializerOptionsFactory = jsonSerializerOptionsFactory;
        });
    }
}