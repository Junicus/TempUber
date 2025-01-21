using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using IRSI.Aloha.Data.Abstractions;
using Shouldly;

namespace IRSI.Aloha.Data.Tests;

public class AlohaDataFolderTests
{
    private readonly IFileSystem _fileSystem;
    private readonly AlohaDataFolder _dataSut;
    private readonly AlohaDataFolder _businessDateSut;
    private readonly DateOnly _businessDate = new(2022, 1, 1);

    public AlohaDataFolderTests()
    {
        _fileSystem = new MockFileSystem();

        _dataSut = new(_fileSystem, "C:\\Data\\Data");
        _businessDateSut = new(_fileSystem, "C:\\Data", _businessDate);
    }

    [Fact]
    public void GetAlohaDataTable_Should_Return_Null_When_File_DoesNotExist()
    {
        var result = _dataSut.GetAlohaDataTable<ItmDataTable>();
        result.ShouldBeNull();
    }

    [Fact]
    public void GetAlohaDataTable_ItmDataTable_Should_Return_Data()
    {
        _fileSystem.Directory.CreateDirectory("C:\\Data\\Data");
        _fileSystem.File.WriteAllBytes("C:\\Data\\Data\\itm.dbf", File.ReadAllBytes("SampleData\\20241201\\itm.dbf"));
        var result = _dataSut.GetAlohaDataTable<ItmDataTable>();
        result.ShouldNotBeNull();
    }

    [Fact]
    public void GetAlohaDataTable_MnuDataTable_Should_Return_Data()
    {
        _fileSystem.Directory.CreateDirectory("C:\\Data\\Data");
        _fileSystem.File.WriteAllBytes("C:\\Data\\Data\\mnu.dbf", File.ReadAllBytes("SampleData\\20241201\\mnu.dbf"));
        var result = _dataSut.GetAlohaDataTable<MnuDataTable>();
        result.ShouldNotBeNull();
    }

    [Fact]
    public void GetAlohaDataTable_SubDataTable_Should_Return_Data()
    {
        _fileSystem.Directory.CreateDirectory("C:\\Data\\Data");
        _fileSystem.File.WriteAllBytes("C:\\Data\\Data\\sub.dbf", File.ReadAllBytes("SampleData\\20241201\\sub.dbf"));
        var result = _dataSut.GetAlohaDataTable<SubDataTable>();
        result.ShouldNotBeNull();
    }
}