using System;
using System.Collections.Generic;

namespace Bust {
  class Disposer : IDisposable {
    private Action _dipose;

    public Disposer(Action dispose) {
      _dipose = dispose;
    }

    public void Dispose() {
      _dipose();
    }
  }

  class InMemoryReporter : IReporter {
    private List<IReporterSubscriber> _subscribers;

    public InMemoryReporter() {
      _subscribers = new List<IReporterSubscriber>();
    }

    public void ReportErrorLine(string service, string line) {
      foreach (var sub in _subscribers.ToArray()) {
        sub.OnErrorLineReceived(service, line);
      }
    }

    public void ReportOutputLine(string service, string line) {
      foreach (var sub in _subscribers.ToArray()) {
        sub.OnOutputLineReceived(service, line);
      }
    }

    public void ReportExit(string service, int code) {
      foreach (var sub in _subscribers.ToArray()) {
        sub.OnExit(service, code);
      }
    }

    public IDisposable Subscribe(IReporterSubscriber sub) {
      this._subscribers.Add(sub);
      return new Disposer(() => this._subscribers.Remove(sub));
    }
  }
}
