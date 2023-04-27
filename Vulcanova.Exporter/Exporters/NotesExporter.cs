using Vulcanova.Uonet.Api;
using Vulcanova.Uonet.Api.Notes;

namespace Vulcanova.Exporter.Exporters;

public class NotesExporter : Exporter
{
    public NotesExporter(string outputDir) : base(outputDir)
    {
    }

    public override async Task ExportDataAsync(ExporterContext context)
    {
        var query = new GetNotesByPupilQuery(context.Account.Pupil.Id, DateTime.MinValue);

        var data = await context.ApiClient.GetAllAsync(GetNotesByPupilQuery.ApiEndpoint, query)
            .ToArrayAsync();

        await SaveToFileAsync("notes.json", data);
    }
}