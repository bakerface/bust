using System.Linq;
using System.Threading.Tasks;

namespace Bust {
  partial class Actions {
    public async Task<SpawnResult> Run() {
      var services = await Services.FetchAll();
      var task = await Task.WhenAny(services.Select(Spawn));
      return await task;
    }
  }
}
