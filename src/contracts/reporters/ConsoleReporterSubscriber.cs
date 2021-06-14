using System;

namespace Bust {
  class ConsoleReporterSubscriber : IReporterSubscriber {
    public void OnErrorLineReceived(string id, string line) {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Error.WriteLine(Format(id, line));
      Console.ResetColor();
    }

    public void OnOutputLineReceived(string id, string line) {
      Console.WriteLine(Format(id, line));
    }

    public void OnExit(string id, int code) {
      Console.ForegroundColor = code == 0 ? ConsoleColor.Green : ConsoleColor.Red;
      Console.Error.WriteLine($"The '{id}' service exited with code {code}");
      Console.ResetColor();
    }

    public string Format(string id, string line) {
      // var timestamp = DateTime.Now.ToUniversalTime().ToString("o");
      // return $"{timestamp} {id}> {line}";
      return line;
    }
  }
}
