using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bust {
  class JsonDirectoryServiceStorage : IServiceStorage {
    public string Root { get; set; } = "conf";
    public string Extension { get; set; } = ".service.json";

    public async Task<IEnumerable<Service>> FetchAll() {
      var services = new List<Service>();

      foreach (var file in Directory.GetFiles(Root, "*" + Extension)) {
        var filename = Path.GetFileName(file);
        var id = filename.Remove(filename.Length - Extension.Length);

        services.Add(await FetchOne(id));
      }

      return services;
    }

    public async Task<Service> FetchOne(string id) {
      var options = new JsonSerializerOptions {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };

      var filename = Path.Join(Root, id + Extension);
      var json = await File.ReadAllTextAsync(filename);
      var service = JsonSerializer.Deserialize<Service>(json, options);

      service.Id = id;

      return service;
    }
  }
}
