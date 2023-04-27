using Vulcanova.Uonet.Api.Schedule;

namespace Vulcanova.Exporter.Exporters;

public class TimetableExporter : Exporter
{
    public TimetableExporter(string outputDir) : base(outputDir)
    {
    }

    public override async Task ExportDataAsync(ExporterContext context)
    {
        foreach (var period in context.Account.Periods)
        {
            var from = DateTimeOffset.FromUnixTimeMilliseconds(period.Start.Timestamp).LocalDateTime;
            var to = DateTimeOffset.FromUnixTimeMilliseconds(period.End.Timestamp).LocalDateTime;

            var query = new GetScheduleEntriesByPupilQuery(context.Account.Pupil.Id,
                from, to, DateTime.MinValue, 5000);

            var data = await context.ApiClient.GetAsync(GetScheduleEntriesByPupilQuery.ApiEndpoint, query);

            await SaveToFileAsync($"timetable_{period.Level}_{period.Number}.json", data.Envelope);
        }
    }
}