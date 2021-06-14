using System.Collections.Generic;

namespace Bust {
  public class Service {
    public string Id { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public Dictionary<string, string> Environment { get; set; }
      = new Dictionary<string, string>();
  }
}
