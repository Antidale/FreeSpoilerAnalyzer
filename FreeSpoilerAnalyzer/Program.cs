using FreeSpoilerAnalyzer.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<KeyItemCommand>("parse");
});
return await app.RunAsync(args);
