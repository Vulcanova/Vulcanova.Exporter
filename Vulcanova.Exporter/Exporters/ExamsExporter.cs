using Vulcanova.Uonet.Api;
using Vulcanova.Uonet.Api.Exams;

namespace Vulcanova.Exporter.Exporters;

public class ExamsExporter : Exporter
{
    public ExamsExporter(string outputDir) : base(outputDir)
    {
    }

    public override async Task ExportDataAsync(ExporterContext context)
    {
        var query = new GetExamsByPupilQuery(context.Account.Unit.Id, context.Account.Pupil.Id, DateTime.MinValue,
            500);

        var data = await context.ApiClient.GetAllAsync(GetExamsByPupilQuery.ApiEndpoint, query)
            .ToArrayAsync();

        await SaveToFileAsync("exams.json", data);
    }
}