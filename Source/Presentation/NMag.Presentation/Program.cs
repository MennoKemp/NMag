using Auxilia.Delegation.Commands;
using Auxilia.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NMag.Persistence;
using NMag.Presentation.Commands;
using NMag.Services;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace NMag.Presentation
{
	public class Program
    {
        private static readonly string[] HelpKeywords =
        {
            "help",
            "?"
        };

        public static IHost Host { get; private set; }
		
        public static IHost CreateHost(string[] args)
        {
	        return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
			{
				services.AddSingleton<ILoggerFactory>(sc =>
				{
					LoggerProviderCollection providerCollection = sc.GetService<LoggerProviderCollection>();
					SerilogLoggerFactory factory = new SerilogLoggerFactory(null, true, providerCollection);

					foreach (ILoggerProvider provider in sc.GetServices<ILoggerProvider>())
						factory.AddProvider(provider);

					return factory;
				});
				services.AddSingleton<ICommandContext>(new CommandContext());
				services.AddSingleton<IGraphCreationService, GraphCreationService>();
				services.AddTransient<IDataSetSerializer, DataSetSerializer>();
				services.AddTransient(CommandTypes);
				services.AddLogging(l => l.AddConsole());
			}).Build();
        }

        private static IEnumerable<Type> CommandTypes
        {
	        get => typeof(Program).Assembly.GetTypesWithAttribute<CommandAttribute>().Where(t => !t.IsAbstract);
        }

        private static void Initialize(string[] args)
        {
	        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
	        AppDomain.CurrentDomain.ProcessExit += OnExit;
			
	        Log.Logger = new LoggerConfiguration()
		        .MinimumLevel.Debug()
		        .WriteTo.Console(new LogFormatter())
		        .CreateLogger();

	        Host = CreateHost(args);
		}

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
	        if (Host?.Services?.GetService<ILogger<Program>>() is ILogger logger)
	        {
				logger.LogCritical((Exception)e.ExceptionObject, "A critical error occurred.");
	        }
	        else
	        {
				Console.WriteLine("A critical error occurred.");
				Console.WriteLine(e.ExceptionObject.ToString());
	        }
        }

        [STAThread]
        static void Main(string[] args)
        {
			Initialize(args);
			
	        CommandParser<CommandBase> commandParser = new CommandParser<CommandBase>(new CommandFactory(), Properties.Resources.ResourceManager, CommandTypes);
			
			ILogger<Program> logger = Host.Services.GetRequiredService<ILogger<Program>>();

			Console.WriteLine(Properties.Resources.About);
			Console.WriteLine(string.Empty);
            
			while (true)
			{
				try
				{
					Console.Write(">");
					string input = Console.ReadLine()?.Trim() ?? string.Empty;

					if (string.IsNullOrWhiteSpace(input))
					{
						Console.WriteLine();
						continue;
					}

					string[] splitInput = input.Split(' ');

					if (input.Equals("clear", StringComparison.OrdinalIgnoreCase))
					{
						Console.Clear();
						continue;
					}

					if (HelpKeywords.Contains(input.Split(' ').Last(), StringComparer.OrdinalIgnoreCase))
					{
						switch (splitInput.Length)
						{
							case 1:
							{
                                logger.LogInformation(Properties.Resources.Help);
								continue;
							}
							case 2:
							{
								string commandName = commandParser.ParseCommandName(splitInput.First());
								logger.LogInformation(commandParser.GetFormattedCommandInfo(commandName));
								continue;
							}
						}
					}

					if (input.Equals("commands"))
					{
                        logger.LogInformation(commandParser.RegisteredCommands.Combine(Environment.NewLine));
					}
					else
					{
						CommandBase command = commandParser.ParseCommand(input);

                        if(command.CanExecute())
                            command.Execute();
					}
				}
				catch (CommandParsingException commandParsingException)
				{
                    logger.LogError(commandParsingException.Message);
				}
				catch (Exception exception)
				{
                    logger.LogError(exception, "An error occurred.");
				}
			}
        }

        private static void OnExit(object sender, EventArgs e)
        {
			Host.Dispose();
        }
    }
}
