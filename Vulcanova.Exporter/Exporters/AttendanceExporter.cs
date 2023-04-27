using Vulcanova.Uonet.Api;
using Vulcanova.Uonet.Api.Lessons;

namespace Vulcanova.Exporter.Exporters;

public class AttendanceExporter : Exporter
{
    public AttendanceExporter(string outputDir) : base(outputDir)
    {
    }

    public override async Task ExportDataAsync(ExporterContext context)
    {
        foreach (var period in context.Account.Periods)
        {
            var query = new GetLessonsByPupilQuery(context.Account.Pupil.Id,
                DateTimeOffset.FromUnixTimeMilliseconds(period.Start.Timestamp).DateTime,
                DateTimeOffset.FromUnixTimeMilliseconds(period.End.Timestamp).DateTime, DateTime.MinValue);

            var data = await context.ApiClient.GetAllAsync(GetLessonsByPupilQuery.ApiEndpoint, query)
                .ToArrayAsync();

            await SaveToFileAsync($"attendance_{period.Level}_{period.Number}.json", data);
        }
    }
}