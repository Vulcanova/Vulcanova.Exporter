# Vulcanova.Exporter
A utility to export data from the Vulcan UONET+ register into JSON files

## How to use?

1. Clone the repository `git clone --recurse-submodules https://github.com/Vulcanova/Vulcanova.Exporter`
2. Build the code with `dotnet build`
3. Run `dotnet run -- --token <mobile_access_token> --symbol <mobile_access_symbol> --pin <mobile_access_pin>`. Results will appear in a directory named `json`.
