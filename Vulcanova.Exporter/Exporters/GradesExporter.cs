using Vulcanova.Uonet.Api;
using Vulcanova.Uonet.Api.Grades;

namespace Vulcanova.Exporter.Exporters;

public class GradesExporter : Exporter
{
    public GradesExporter(string outputDir) : base(outputDir)
    {
    }

    public override async Task ExportDataAsync(ExporterContext context)
    {
        foreach (var period in context.Account.Periods)
        {
            var query = new GetGradesByPupilQuery(context.Account.Unit.Id, context.Account.Pupil.Id, period.Id,
                DateTime.MinValue, 500);

            var data = await context.ApiClient.GetAllAsync(GetGradesByPupilQuery.ApiEndpoint, query)
                .ToArrayAsync();

            await SaveToFileAsync($"grades_{period.Level}_{period.Number}.json", data);
        }
    }
}