using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bust {
  class Program {
    static string VERSION = typeof(Program).Assembly.GetName().Version.ToString();

    static void Main(string[] args) {
      if (args.Any((x) => x == "-v" || x == "--version")) {
        Console.WriteLine(VERSION);
        Environment.Exit(0);
      }

      Run(CreateActions()).Wait();
    }

    static async Task Run(Actions actions) {
      var result = await actions.Run();
      var id = result.Service.Id;
      var code = result.ExitCode;

      Environment.Exit(code);
    }

    static Actions CreateActions() {
      var actions = new Actions {
        Reporter = new InMemoryReporter(),
        Services = new JsonDirectoryServiceStorage(),
      };

      actions.Reporter.Subscribe(new ConsoleReporterSubscriber());

      return actions;
    }
  }
}
