using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bust {
  partial class Actions {
    public class SpawnResult {
      public Service Service { get; set; }
      public int ExitCode { get; set; }
    }

    public async Task<SpawnResult> Spawn(Service service) {
      var endOfStdOut = new TaskCompletionSource();
      var endOfStdErr = new TaskCompletionSource();

      var command = service.Command.Split(' ');
      var filename = command[0];
      var arguments = string.Join(' ', command.Skip(1));

      var process = new Process {
        EnableRaisingEvents = true,
        StartInfo = {
          FileName = filename,
          Arguments = arguments,
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
        }
      };

      foreach (var item in service.Environment) {
        process.StartInfo.EnvironmentVariables.Add(item.Key, item.Value);
      }

      process.OutputDataReceived += (sender, e) => {
        if (e.Data == null) {
          endOfStdOut.SetResult();
        } else {
          Reporter.ReportOutputLine(service.Id, e.Data);
        }
      };

      process.ErrorDataReceived += (sender, e) => {
        if (e.Data == null) {
          endOfStdErr.SetResult();
        } else {
          Reporter.ReportErrorLine(service.Id, e.Data);
        }
      };

      process.Start();
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();

      var subscriber = new ReporterSubscriber {
        OutputLineReceived = (id, line) => {
          if (service.Id != id) {
            process.StandardInput.WriteLine(line);
          }
        },
        ErrorLineReceived = (id, line) => {
          // one of the services has reported an error
        },
        Exit = (id, code) => {
          // one of the services has exited
        },
      };

      using (Reporter.Subscribe(subscriber)) {
        await process.WaitForExitAsync();

        // wait for output streams to finish parsing
        await endOfStdOut.Task;
        await endOfStdErr.Task;

        var result = new SpawnResult {
          Service = service,
          ExitCode = process.ExitCode
        };

        process.Close();
        Reporter.ReportExit(service.Id, result.ExitCode);
        return result;
      }
    }
  }
}
