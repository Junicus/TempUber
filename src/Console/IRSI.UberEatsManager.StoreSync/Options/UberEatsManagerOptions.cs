namespace IRSI.UberEatsManager.SyncCli.Options;

public class UberEatsManagerOptions
{
    public const string SectionName = "UberEatsManager";
    
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}