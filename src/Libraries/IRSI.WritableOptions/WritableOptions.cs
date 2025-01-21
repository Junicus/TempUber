using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IRSI.WritableOptions;

public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
{
    private readonly string _file;
    private readonly string _section;
    private readonly IOptionsMonitor<T> _options;
    private readonly IConfigurationRoot _configurationRoot;
    private readonly JsonSerializerOptions _serializerOptions;

    public WritableOptions(
        string file,
        string section,
        IOptionsMonitor<T> options,
        IConfigurationRoot configurationRoot,
        Func<JsonSerializerOptions>? defaultSerializerOptions = null)
    {
        _file = file;
        _section = section;
        _options = options;
        _configurationRoot = configurationRoot;

        _serializerOptions = (defaultSerializerOptions is null)
            ? JsonFileHelper.DefaultJsonSerializerOptions.Invoke()
            : defaultSerializerOptions.Invoke();
    }

    public T Value => _options.CurrentValue;
    public T Get(string? name) => _options.Get(name);

    public void Update(Action<T> applyChanges, bool reload = true, JsonSerializerOptions? serializerOptions = null)
    {
        JsonFileHelper.AddOrUpdateSection(_file, _section, applyChanges, serializerOptions ?? _serializerOptions);
        if (reload) _configurationRoot.Reload();
    }
}