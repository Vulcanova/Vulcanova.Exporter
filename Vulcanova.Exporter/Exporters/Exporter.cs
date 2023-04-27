using System.Text.Json;

namespace Vulcanova.Exporter.Exporters;

public abstract class Exporter
{
    private readonly string _outputDir;

    protected Exporter(string outputDir)
    {
        _outputDir = outputDir;
    }

    public abstract Task ExportDataAsync(ExporterContext context);

    protected async Task SaveToFileAsync<T>(string fileName, T data)
    {
        var jsonContents = JsonSerializer.Serialize(data);

        var outputPath = Path.Combine(_outputDir, fileName);

        await File.WriteAllTextAsync(outputPath, jsonContents);
    }
}