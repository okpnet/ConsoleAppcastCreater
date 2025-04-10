// See https://aka.ms/new-console-template for more information

using AppcastCreater;

if (args.Length == 0)
{
    Console.WriteLine("Argment is null,Json file path use.");
    var buffer = System.Text.Json.JsonSerializer.Serialize(new AppcastConfig());
    Console.WriteLine($"json ... {buffer}");
    return;
}

var config=AppcastHelper.ReadAppcastConfig(args[0]);

if (config is null)
{
    Console.WriteLine($"'{args[0]}' is not appcast config json.");
    var buffer=System.Text.Json.JsonSerializer.Serialize(new AppcastConfig());
    Console.WriteLine($"json ... {buffer}");
    return;
}

AppcastHelper.CreateAppcast(config);

Console.WriteLine($"create done.to '{config.OutputXml}'");