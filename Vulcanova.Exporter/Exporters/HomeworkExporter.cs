using Vulcanova.Uonet.Api;
using Vulcanova.Uonet.Api.Homework;

namespace Vulcanova.Exporter.Exporters;

public class HomeworkExporter : Exporter
{
    public HomeworkExporter(string outputDir) : base(outputDir)
    {
    }

    public override async Task ExportDataAsync(ExporterContext context)
    {
        var query = new GetHomeworkByPupilQuery(context.Account.Pupil.Id,
            // the API will return all results no matter the period id
            context.Account.Periods.First().Id,
            DateTime.MinValue);

        var data = await context.ApiClient.GetAllAsync(GetHomeworkByPupilQuery.ApiEndpoint, query)
            .ToArrayAsync();

        await SaveToFileAsync($"homework.json", data);
    }
}