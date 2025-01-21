namespace IRSI.UberEatsManager.SyncCli.Options;

public class Hashes
{
    public const string SectionName = "Hashes";
    public List<FileHash> Files { get; set; } = [];
}

public class FileHash
{
    public string FileName { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
}