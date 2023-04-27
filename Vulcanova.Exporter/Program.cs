// See https://aka.ms/new-console-template for more information

using System.CommandLine;
using System.Reflection;
using Vulcanova.Exporter;
using Vulcanova.Exporter.Exporters;

var tokenOption = new Option<string>(
        name: "--token",
        description: "The mobile access token")
    {IsRequired = true};

var symbolOption = new Option<string>(
        name: "--symbol",
        description: "The mobile access symbol")
    {IsRequired = true};

var pinOption = new Option<string>(
        name: "--pin",
        description: "The mobile access PIN")
    {IsRequired = true};

var outputDirOption = new Option<string>(
    name: "--output-dir",
    description: "The path to output the JSON files to",
    getDefaultValue: () => "json");

var rootCommand = new RootCommand("A utility to export data from the Vulcan UONET+ register into JSON files");

rootCommand.AddOption(tokenOption);
rootCommand.AddOption(symbolOption);
rootCommand.AddOption(pinOption);
rootCommand.AddOption(outputDirOption);

rootCommand.SetHandler(async (token, symbol, pin, outputDir) =>
{
    if (!Directory.Exists(outputDir))
    {
        Directory.CreateDirectory(outputDir);
    }

    var exporters = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsAssignableTo(typeof(Exporter)) && !t.IsAbstract)
        .Select(t => (Exporter) Activator.CreateInstance(t, outputDir)!);

    var context = await ExporterContext.Create(token, symbol, pin);
    
    foreach (var exporter in exporters)
    {
        await exporter.ExportDataAsync(context);
    }
}, tokenOption, symbolOption, pinOption, outputDirOption);

await rootCommand.InvokeAsync(args);