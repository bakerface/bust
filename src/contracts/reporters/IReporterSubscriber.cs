namespace Bust {
  interface IReporterSubscriber {
    void OnOutputLineReceived(string id, string line);
    void OnErrorLineReceived(string id, string line);
    void OnExit(string id, int code);
  }
}
