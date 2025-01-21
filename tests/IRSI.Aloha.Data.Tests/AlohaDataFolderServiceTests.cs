using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using IRSI.Common.Abstractions;
using NSubstitute;
using Shouldly;

namespace IRSI.Aloha.Data.Tests;

public class AlohaDataFolderServiceTests
{
    private readonly IFileSystem _fileSystem;
    private readonly IEnvironment _environment;
    private readonly AlohaDataFolderService _sut;

    public AlohaDataFolderServiceTests()
    {
        _environment = Substitute.For<IEnvironment>();
        _fileSystem = new MockFileSystem();
        _sut = new(_environment, _fileSystem);
    }

    [Fact]
    public void GetDataFolder_Should_ThrowException_When_Folder_DoesNotExist()
    {
        Should.Throw<DirectoryNotFoundException>(() => _sut.GetDataFolder("C:\\Data"));
    }

    [Fact]
    public void GetDataFolder_Should_Return_AlohaDataTable_When_Folder_Exists()
    {
        _fileSystem.Directory.CreateDirectory("C:\\Data");
        var result = _sut.GetDataFolder("C:\\Data");
        result.ShouldNotBeNull();
    }

    [Fact]
    public void GetDataFolder_Should_ThrowException_When_EnvironmentVariable_Not_Set()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns((string)null!);
        Should.Throw<InvalidOperationException>(() => _sut.GetDataFolder());
    }

    [Fact]
    public void GetDataFolder_Should_Return_AlohaDataTable_When_EnvironmentVariable_Set_And_Folder_Exists()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        _fileSystem.Directory.CreateDirectory("C:\\Data\\Data");

        var result = _sut.GetDataFolder();

        result.ShouldNotBeNull();
    }

    [Fact]
    public void GetDataFolder_Should_Return_Cached_AlohaDataFolder()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        _fileSystem.Directory.CreateDirectory("C:\\Data\\Data");

        var first = _sut.GetDataFolder();
        var cached = _sut.GetDataFolder();

        first.ShouldBeSameAs(cached);
    }

    [Fact]
    public void GetDataFolder_WithSkipCache_Should_Return_AlohaDataFolder_NotCached()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        _fileSystem.Directory.CreateDirectory("C:\\Data\\Data");

        var first = _sut.GetDataFolder();
        var uncached = _sut.GetDataFolder(true);

        first.ShouldNotBeSameAs(uncached);
    }

    [Fact]
    public void GetDataFolder_WithSkipCache_Should_NotCache_AlohaDataFolder()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        _fileSystem.Directory.CreateDirectory("C:\\Data\\Data");

        var first = _sut.GetDataFolder();
        var uncached = _sut.GetDataFolder(true);
        var cached = _sut.GetDataFolder();

        first.ShouldNotBeSameAs(uncached);
        first.ShouldBeSameAs(cached);
        cached.ShouldNotBeSameAs(uncached);
    }

    [Fact]
    public void GetBusinessDateFolder_Should_ThrowException_When_EnvironmentVariable_Not_Set()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns((string)null!);
        Should.Throw<InvalidOperationException>(() => _sut.GetBusinessDateFolder(DateOnly.FromDateTime(DateTime.Now)));
    }

    [Fact]
    public void GetBusinessDateFolder_Should_Return_AlohaDataTable_When_EnvironmentVariable_Set_And_Folder_Exists()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        var expectedDate = DateOnly.FromDateTime(DateTime.Now);
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{expectedDate:yyyyMMdd}");

        var result = _sut.GetBusinessDateFolder(expectedDate);

        result.ShouldNotBeNull();
    }

    [Fact]
    public void GetBusinessDateFolder_Should_Return_Cached_AlohaDataFolder()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        var expectedDate = DateOnly.FromDateTime(DateTime.Now);
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{expectedDate:yyyyMMdd}");

        var first = _sut.GetBusinessDateFolder(expectedDate);
        var cached = _sut.GetBusinessDateFolder(expectedDate);

        first.ShouldBeSameAs(cached);
    }

    [Fact]
    public void GetBusinessDateFolder_WithSkipCache_Should_Return_AlohaDataFolder_NotCached()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        var expectedDate = DateOnly.FromDateTime(DateTime.Now);
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{expectedDate:yyyyMMdd}");

        var first = _sut.GetBusinessDateFolder(expectedDate);
        var uncached = _sut.GetBusinessDateFolder(expectedDate, true);

        first.ShouldNotBeSameAs(uncached);
    }


    [Fact]
    public void GetBusinessDateFolder_WithSkipCache_Should_NotCache_AlohaDataFolder()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        var expectedDate = DateOnly.FromDateTime(DateTime.Now);
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{expectedDate:yyyyMMdd}");

        var first = _sut.GetBusinessDateFolder(expectedDate);
        var uncached = _sut.GetBusinessDateFolder(expectedDate, true);
        var cached = _sut.GetBusinessDateFolder(expectedDate);

        first.ShouldNotBeSameAs(uncached);
        first.ShouldBeSameAs(cached);
        cached.ShouldNotBeSameAs(uncached);
    }

    [Fact]
    public void GetBusinessDateFolders_Should_Return_Cached_AlohaDataFolders()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var endDate = DateOnly.FromDateTime(DateTime.Now);
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{startDate:yyyyMMdd}");
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{startDate.AddDays(1):yyyyMMdd}");
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{endDate:yyyyMMdd}");

        var first = _sut.GetBusinessDateFolders(startDate, endDate);
        var cached = _sut.GetBusinessDateFolders(startDate, endDate);

        foreach (var (key, value) in first)
        {
            cached[key].ShouldBeSameAs(value);
        }
    }

    [Fact]
    public void GetBusinessDateFolders_WithSkipCache_Should_Return_AlohaDataFolders_NotCached()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var endDate = DateOnly.FromDateTime(DateTime.Now);
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{startDate:yyyyMMdd}");
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{startDate.AddDays(1):yyyyMMdd}");
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{endDate:yyyyMMdd}");

        var first = _sut.GetBusinessDateFolders(startDate, endDate);
        var uncached = _sut.GetBusinessDateFolders(startDate, endDate, true);

        foreach (var (key, value) in first)
        {
            uncached[key].ShouldNotBeSameAs(value);
        }
    }

    [Fact]
    public void GetBusinessDateFolders_WithSkipCache_Should_NotCache_AlohaDataFolders()
    {
        _environment.GetEnvironmentVariable("IBERDIR").Returns("C:\\Data");
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var endDate = DateOnly.FromDateTime(DateTime.Now);
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{startDate:yyyyMMdd}");
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{startDate.AddDays(1):yyyyMMdd}");
        _fileSystem.Directory.CreateDirectory($"C:\\Data\\{endDate:yyyyMMdd}");

        var first = _sut.GetBusinessDateFolders(startDate, endDate);
        var uncached = _sut.GetBusinessDateFolders(startDate, endDate, true);
        var cached = _sut.GetBusinessDateFolders(startDate, endDate);

        foreach (var (key, value) in first)
        {
            uncached[key].ShouldNotBeSameAs(value);
        }

        foreach (var (key, value) in first)
        {
            cached[key].ShouldBeSameAs(value);
        }

        foreach (var (key, value) in cached)
        {
            uncached[key].ShouldNotBeSameAs(value);
        }
    }
}