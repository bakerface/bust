using System;

namespace Bust {
  interface IReporter {
    void ReportOutputLine(string service, string line);
    void ReportErrorLine(string service, string line);
    void ReportExit(string service, int code);
    IDisposable Subscribe(IReporterSubscriber sub);
  }
}
