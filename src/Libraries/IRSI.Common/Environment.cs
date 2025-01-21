using IRSI.Common.Abstractions;

namespace IRSI.Common;

public class Environment : IEnvironment
{
    public string? GetEnvironmentVariable(string variable) => System.Environment.GetEnvironmentVariable(variable);
    public string[] GetCommandLineArgs() => System.Environment.GetCommandLineArgs();
}