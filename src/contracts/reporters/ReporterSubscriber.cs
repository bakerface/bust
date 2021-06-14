using System;

namespace Bust {
  class ReporterSubscriber : IReporterSubscriber {
    public Action<string, string> OutputLineReceived { get; set; }
    public Action<string, string> ErrorLineReceived { get; set; }
    public Action<string, int> Exit { get; set; }

    public void OnErrorLineReceived(string id, string line) {
      ErrorLineReceived(id, line);
    }

    public void OnOutputLineReceived(string id, string line) {
      OutputLineReceived(id, line);
    }

    public void OnExit(string id, int code) {
      Exit(id, code);
    }
  }
}
