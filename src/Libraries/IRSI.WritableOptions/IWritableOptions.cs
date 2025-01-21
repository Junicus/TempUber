using System.Text.Json;
using Microsoft.Extensions.Options;

namespace IRSI.WritableOptions;

public interface IWritableOptions<T> : IOptionsSnapshot<T> where T : class, new()
{
    void Update(Action<T> applyChanges, bool reload = true, JsonSerializerOptions? serializerOptions = null);
}