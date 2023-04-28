using Vulcanova.Uonet.Api.Schedule;

namespace Vulcanova.Exporter.Exporters;

public class TimetableChangesExporter : Exporter
{
    public TimetableChangesExporter(string outputDir) : base(outputDir)
    {
    }

    public override async Task ExportDataAsync(ExporterContext context)
    {
        foreach (var period in context.Account.Periods)
        {
            var from = DateTimeOffset.FromUnixTimeMilliseconds(period.Start.Timestamp).LocalDateTime;
            var to = DateTimeOffset.FromUnixTimeMilliseconds(period.End.Timestamp).LocalDateTime;

            var query = new GetScheduleChangesEntriesByPupilQuery(context.Account.Pupil.Id,
                from, to, DateTime.MinValue, 5000);

            var data = await context.ApiClient.GetAsync(GetScheduleChangesEntriesByPupilQuery.ApiEndpoint, query);

            await SaveToFileAsync($"timetable-changes_{period.Level}_{period.Number}.json", data.Envelope);
        }
    }
}