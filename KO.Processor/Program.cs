using KO.Core.Constants;
using KO.Core.Helpers.Memory;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.IO;

namespace KO.Processor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WinApi.ShowWindow(WinApi.GetConsoleWindow(), 0);

                var cmd = new CommandLineApplication();
                var pathCommand = cmd.Option("--p <value>", "Path", CommandOptionType.SingleValue);
                var nameCommand = cmd.Option("--n <value>", "Name", CommandOptionType.SingleValue);
                var argumentsCommand = cmd.Option("--a <value>", "Arguments", CommandOptionType.SingleValue);

                cmd.OnExecute(() =>
                {
                    var path = pathCommand.Value();
                    var name = nameCommand.Value();
                    var arguments = argumentsCommand.Value();
                    var fileInfo = new FileInfo(path);

                    var startInfo = new ProcessStartInfo(fileInfo.Name) { WorkingDirectory = fileInfo.Directory.FullName };

                    if (!string.IsNullOrEmpty(arguments))
                        startInfo.Arguments = arguments;

                    var process = Process.Start(startInfo);

                    MemoryHelper.Wait(2000);
                    MemoryHelper.CloseMutant(process, Settings.GameDefaultTitle);
                    WinApi.SetWindowText(process.MainWindowHandle, name);

                    return 0;
                });

                cmd.HelpOption("-? | -h | --help");
                cmd.Execute(args);
            }
            catch (Exception ex)
            {
                WinApi.ShowWindow(WinApi.GetConsoleWindow(), 1);
                Console.WriteLine("! ERROR !");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
