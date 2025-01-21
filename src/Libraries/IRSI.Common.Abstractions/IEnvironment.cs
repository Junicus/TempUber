namespace IRSI.Common.Abstractions;

public interface IEnvironment
{
    string? GetEnvironmentVariable(string variable);
    string[] GetCommandLineArgs();
}